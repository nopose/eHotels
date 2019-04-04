using eHotels.Data;
using eHotels.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eHotels.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class CustomerController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;

        public CustomerController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext context,
            ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _logger = logger;
        }

        [TempData]
        public string StatusMessage { get; set; }

        #region SearchRoom

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index()
        {
            var model = new SearchRoomViewModel(_context)
            {
                // Filter values
                isUsingTodaysRooms = false,
                StartDate = DateTime.Today.AddHours(16),
                EndDate = DateTime.Today.AddDays(1).AddHours(12),
                Capacity = null,
                NumberOfRooms = null,
                PriceOfRooms = null,

                // Values for the dropdown lists.
                ChainSelected = null,
                CategorySelected = null,
                StateSelected = null
            };

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult Index(SearchRoomViewModel model)
        {
            DBManipulation db = new DBManipulation(_context);
            List<int> RoomsToExclude = new List<int>();
            string qry = "";
            DateTime s = model.StartDate.Date.AddHours(16);
            DateTime e = model.EndDate.Date.AddHours(12);

            model.Rooms = new List<Room>();

            model.initModel(_context);

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (!(DateTime.Compare(s, DateTime.Now) > 0)) {
                ModelState.AddModelError("StartDate", "The start date must be greater than today.");
                return View(model);
            }

            if (!(DateTime.Compare(e, DateTime.Now) > 0)) {
                ModelState.AddModelError("EndDate", "The end date must be greater than today.");
                return View(model);
            } else if (!(DateTime.Compare(e, model.StartDate) > 0)) {
                ModelState.AddModelError("EndDate", "The end date must be greater (by at least 1 day) than the start date.");
                return View(model);
            }

            if (!model.isUsingTodaysRooms) {
                qry += "SELECT rid, room_num, room.hid, price, capacity, isExtandable, landscape FROM eHotel.room INNER JOIN eHotel.hotel ON room.hid = hotel.hid WHERE 0=0";

                List<Booking> Bookings = db.getBookingsByDates(s, e);
                if (Bookings.Count > 0) {
                    foreach (Booking b in Bookings) {
                        RoomsToExclude.Add(b.Rid);
                    }
                }

                List<Renting> Rentings = db.getRentingsByDates(s, e);
                if (Rentings.Count > 0) {
                    foreach (Renting r in Rentings) {
                        RoomsToExclude.Add(r.Rid);
                    }
                }

                if (model.ChainSelected != null) {
                    qry += " AND hotel.hcid = " + model.ChainSelected;
                }

                if (model.Capacity != null) {
                    qry += " AND capacity >= " + model.Capacity;
                }

                if (model.CategorySelected != null) {
                    qry += " AND category = " + model.CategorySelected;
                }

                if (model.StateSelected != null) {
                    qry += " AND h_state = " + model.StateSelected;
                }

                if (model.NumberOfRooms != null) {
                    qry += " AND num_rooms >= " + model.NumberOfRooms;
                }

                if (model.PriceOfRooms != null) {
                    qry += " AND price <= " + model.PriceOfRooms;
                }

                if (RoomsToExclude.Count > 0)
                {
                    bool first = true;
                    qry += " AND rid NOT IN (";
                    foreach (int r in RoomsToExclude)
                    {
                        if (first)
                        {
                            qry += r;
                            first = false;
                        }
                        else
                            qry += ", " + r;
                    }
                    qry += ")";
                }

                model.Rooms = db.getRoomForSearch(qry);

                return View(model);

            }

            StatusMessage = "Exactly " + model.Rooms.Count.ToString() + " have been loaded.";

            return View(model);
        }

        #endregion

        #region BookRoom

        [HttpGet]
        public IActionResult BookRoomFromSearch(Dictionary<string, string> parms)
        {
            DBManipulation db = new DBManipulation(_context);

            string s_rid = "";
            int rid = -1;
            string s_startdate = "", s_enddate = "";
            DateTime startdate = new DateTime(), enddate = new DateTime();

            if (parms.TryGetValue("roomid", out s_rid))
                rid = Int32.Parse(s_rid);
            if (parms.TryGetValue("startdate", out s_startdate))
                startdate = DateTime.Parse(s_startdate);
            if (parms.TryGetValue("enddate", out s_enddate))
                enddate = DateTime.Parse(s_enddate);

            var user = getUser();
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            Person customer = db.getCustomer(user.SSN);
            Room room = db.getRoom(rid);
            Booking newBooking = new Booking()
            {
                CustomerSsnNavigation = customer.Customer,
                CustomerSsn = customer.Ssn,
                R = room,
                Rid = room.Rid,
                StartDate = startdate,
                EndDate = enddate
            };

            return View(newBooking);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult BookRoomFromSearch(Booking newBooking)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Object[] insertArray = new object[] { newBooking.Rid, newBooking.CustomerSsn, newBooking.StartDate, newBooking.EndDate };
                    _context.Database.ExecuteSqlCommand(
                       "INSERT INTO eHotel.booking (rid, customer_ssn, start_date, end_date)" +
                       "VALUES ({0},{1},{2},{3})",
                       parameters: insertArray);

                    TempData["SuccessMessage"] = "Booking successfully added to your account!!";
                    return RedirectToAction("CustomerBookings");
                }
            }
            catch (Npgsql.PostgresException ex)
            {
                //Log the error (uncomment ex variable name and write a log.
                TempData["ErrorMessage"] = ex.Message;
            }
            return View(newBooking);
        }

        #endregion

        #region CustomerBookings

        public IActionResult CustomerBookings()
        {
            DBManipulation db = new DBManipulation(_context);

            var user = getUser();
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            List<Booking> bookings = db.getPersonBookings(user.SSN);

            return View(bookings);
        }

        #endregion

        #region RentRoom

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RentRoomFromSearch(PaymentViewModel newRenting)
        {
            if (isEmployee())
            {
                try
                {
                    if (ModelState.IsValid)
                    {

                        Object[] insertArray = new object[] { newRenting.booking.Rid, Int32.Parse(newRenting.SSN), getUser().SSN, newRenting.booking.StartDate, newRenting.booking.EndDate };
                        _context.Database.ExecuteSqlCommand("INSERT INTO eHotel.Renting (rid,customer_ssn,employee_ssn,start_date,end_date) VALUES ({0},{1},{2},{3},{4})", parameters: insertArray);

                        TempData["SuccessMessage"] = "Renting successfully added to " + newRenting.FullName + "'s account!!";
                        return RedirectToAction("Index");
                    }
                }
                catch (Npgsql.PostgresException ex)
                {
                    //Log the error (uncomment ex variable name and write a log.
                    TempData["ErrorMessage"] = ex.Message;
                }
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("AccessDenied", "Account");
            }
        }

        #endregion

        #region DBViews

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ViewOne()
        {
            DBManipulation db = new DBManipulation(_context);
            ViewOneViewModel model = new ViewOneViewModel()
            {
                stateSelected = "All",
                viewones = db.getViewOne(),
                states = db.getHotelStates()
            };

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult ViewOne(ViewOneViewModel model)
        {
            DBManipulation db = new DBManipulation(_context);
            model.viewones = model.stateSelected.Equals("All") ? db.getViewOne() : db.getViewOne(model.stateSelected);
            model.states = db.getHotelStates();

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ViewTwo()
        {
            DBManipulation db = new DBManipulation(_context);
            ViewTwoViewModel model = new ViewTwoViewModel()
            {
                viewtwos = db.getViewTwo(),
                hotels = db.getHotels(),
                hotelSelected = -1
            };

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult ViewTwo(ViewTwoViewModel model)
        {
            DBManipulation db = new DBManipulation(_context);
            model.viewtwos = (model.hotelSelected == -1) ? db.getViewTwo() : db.getViewTwo(model.hotelSelected);
            model.hotels = db.getHotels();

            return View(model);
        }

        #endregion

        private async Task<ApplicationUser> getUserAsync()
        {
            return await _userManager.GetUserAsync(HttpContext.User);
        }

        private ApplicationUser getUser()
        {
            var userTask = getUserAsync();
            userTask.Wait();
            return userTask.Result;
        }

        private Boolean isEmployee()
        {
            var user = getUser();
            if (user != null)
            {
                return user.Role.Equals(Constants.EMPLOYEE);
            }
            else
            {
                return false;
            }
        }

    }
}
