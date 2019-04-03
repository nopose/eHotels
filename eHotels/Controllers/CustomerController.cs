using eHotels.Data;
using eHotels.Models;
using eHotels.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;

        public CustomerController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext context,
            IEmailSender emailSender,
            ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _emailSender = emailSender;
            _logger = logger;
        }

        [TempData]
        public string StatusMessage { get; set; }

        #region SearchRoom

        [HttpGet]
        public IActionResult Index()
        {
            var user = getUser();
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var model = new SearchRoomViewModel(_context)
            {
                UserId = user.SSN,

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
                ModelState.AddModelError("StartDate", "The start date must be greater or equal than today.");
                return View(model);
            }

            if (!(DateTime.Compare(e, DateTime.Now) > 0)) {
                ModelState.AddModelError("EndDate", "The end date must be greater or equal than today.");
                return View(model);
            } else if (!(DateTime.Compare(e, model.StartDate) > 0)) {
                ModelState.AddModelError("EndDate", "The end date must be greater (by at least 1 day) than the start date.");
                return View(model);
            }

            if (!model.isUsingTodaysRooms) {
                qry += "SELECT rid, room_num, room.hid, price, capacity, isExtandable, landscape FROM eHotel.room INNER JOIN eHotel.hotel ON room.hid = hotel.hid WHERE 0=0";

                List<Booking> Bookings = db.getBookingsByDates(s, e);
                if (Bookings.Count > 0) {
                    foreach(Booking b in Bookings) {
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

            } else {
                //TODO
            }

            StatusMessage = "Exactly " + model.Rooms.Count.ToString() + " have been loaded.";

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

    }
}
