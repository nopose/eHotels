﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using eHotels.Models;
using eHotels.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Authorization;
using Npgsql;
using System.Collections;

namespace eHotels.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private IMemoryCache _cache;

        public HomeController(
                    UserManager<ApplicationUser> userManager,
                    SignInManager<ApplicationUser> signInManager,
                    ApplicationDbContext context,
                    IMemoryCache cache)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _cache = cache;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult EmployeeSection()
        {
            if (isEmployee())
            {
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Account");
            }
        }

        #region ManageHotel

        [HttpGet]
        public IActionResult ManageHotels()
        {
            if (isEmployee())
            {
                List<Hotel> hotels = new DBManipulation(_context).getHotelsFullNav();
                return View(hotels);
            }
            else
            {
                return RedirectToAction("AccessDenied", "Account");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteHotel([FromBody]Hotel data)
        {
            if (isEmployee())
            {
                if (deleteHotel(Convert.ToInt32(data.Hid)))
                {
                    return Json("Success");
                }
                else
                {
                    return Json("Error: Could not delete the hotel");
                }
            }
            else
            {
                return RedirectToAction("AccessDenied", "Account");
            }

        }

        [HttpGet]
        public IActionResult CreateHotel()
        {
            if (isEmployee())
            {
                HotelViewModel model = new HotelViewModel(_context);
                return View(model);
            }
            else
            {
                return RedirectToAction("AccessDenied", "Account");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateHotel(HotelViewModel model, string returnUrl = null)
        {
            if (isEmployee())
            {
                ViewData["ReturnUrl"] = returnUrl;
                if (ModelState.IsValid)
                {
                    Boolean insertResult = createHotel(model);
                    if (insertResult)
                    {
                        TempData["SuccessMessage"] = "Hotel created";
                        return RedirectToAction("ManageHotels");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, TempData["ErrorMessage"].ToString());
                    }
                }
                model.initModel(_context);
                // If we got this far, something failed, redisplay form
                return View(model);
            }
            else
            {
                return RedirectToAction("AccessDenied", "Account");
            }
        }

        [HttpGet]
        public IActionResult EditHotel(String Hid)
        {
            if (isEmployee())
            {
                Hotel hotel = getHotel(Convert.ToInt32(Hid));
                HotelViewModel model = new HotelViewModel(_context, hotel);
                return View(model);
            }
            else
            {
                return RedirectToAction("AccessDenied", "Account");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditHotel(HotelViewModel model)
        {
            if (isEmployee())
            {
                ModelState.Remove("PhoneAdd");
                if (ModelState.IsValid)
                {
                    Boolean insertResult = updateHotel(convertModelToHotel(model));
                    if (insertResult)
                    {
                        TempData["SuccessMessage"] = "Hotel updated";
                        return RedirectToAction("ManageHotels");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, TempData["ErrorMessage"].ToString());
                    }
                }
                // If we got this far, something failed, redisplay form
                model.initModel(_context, Convert.ToInt32(model.Hid));
                return View(model);
            }
            else
            {
                return RedirectToAction("AccessDenied", "Account");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteHotelPhone([FromBody]Hotelphone data)
        {
            if (isEmployee())
            {
                if (deleteHotelPhone(data.Hid, data.PhoneNumber))
                {
                    return Json("Success");
                }
                else
                {
                    return Json("Error: Could not delete the phone number");
                }
            }
            else
            {
                return RedirectToAction("AccessDenied", "Account");
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddHotelPhone([FromBody]Hotelphone data)
        {
            if (isEmployee())
            {
                Boolean insertResult = addHotelPhone(data.Hid, data.PhoneNumber);
                if (insertResult)
                {
                    return Json("Success");
                }
                else
                {
                    return Json("Error: Could not add the phone number");
                }
            }
            else
            {
                return RedirectToAction("AccessDenied", "Account");
            }
        }

        #endregion

        #region ManageRooms

        [HttpGet]
        public IActionResult ManageRooms()
        {
            if (isEmployee())
            {
                ManageRoomViewModel model = new ManageRoomViewModel(_context);
                return View(model);
            }
            else
            {
                return RedirectToAction("AccessDenied", "Account");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ManageRooms(ManageRoomViewModel model)
        {
            if (isEmployee())
            {
                model.initModel(_context, model.SelectedHotel);
                return View(model);
            }
            else
            {
                return RedirectToAction("AccessDenied", "Account");
            }
        }

        [HttpGet]
        public IActionResult CreateRoom()
        {
            if (isEmployee())
            {
                RoomViewModel model = new RoomViewModel(_context);
                return View(model);
            }
            else
            {
                return RedirectToAction("AccessDenied", "Account");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateRoom(RoomViewModel model)
        {
            if (isEmployee())
            {
                if (ModelState.IsValid)
                {
                    Boolean insertResult = createRoom(model);
                    if (insertResult)
                    {
                        TempData["SuccessMessage"] = "Room created";
                        return RedirectToAction("ManageRooms");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, TempData["ErrorMessage"].ToString());
                    }
                }
                model.initModel(_context);
                // If we got this far, something failed, redisplay form
                return View(model);
            }
            else
            {
                return RedirectToAction("AccessDenied", "Account");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteRoom([FromBody]Room data)
        {
            if (isEmployee())
            {
                if (deleteRoom(Convert.ToInt32(data.Rid)))
                {
                    return Json("Success");
                }
                else
                {
                    return Json("Error: Could not delete the room");
                }
            }
            else
            {
                return RedirectToAction("AccessDenied", "Account");
            }

        }

        [HttpGet]
        public IActionResult EditRoom(String Rid)
        {
            if (isEmployee())
            {
                Room room = getRoom(Convert.ToInt32(Rid));
                RoomViewModel model = new RoomViewModel(_context, room);
                return View(model);
            }
            else
            {
                return RedirectToAction("AccessDenied", "Account");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditRoom(RoomViewModel model)
        {
            if (isEmployee())
            {
                //ModelState.Remove("PhoneAdd");
                if (ModelState.IsValid)
                {
                    Boolean insertResult = updateRoom(convertModelToRoom(model));
                    if (insertResult)
                    {
                        TempData["SuccessMessage"] = "Room updated";
                        return RedirectToAction("ManageRooms");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, TempData["ErrorMessage"].ToString());
                    }
                }
                // If we got this far, something failed, redisplay form
                model.initModel(_context, Convert.ToInt32(model.Rid));
                return View(model);
            }
            else
            {
                return RedirectToAction("AccessDenied", "Account");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteRoomDamage([FromBody]Damage data)
        {
            if (isEmployee())
            {
                if (deleteRoomDamage(data.Did))
                {
                    return Json("Success");
                }
                else
                {
                    return Json("Error: Could not delete the damage");
                }
            }
            else
            {
                return RedirectToAction("AccessDenied", "Account");
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddRoomDamage([FromBody]Damage data)
        {
            if (isEmployee())
            {
                int insertResult = addRoomDamage(data.Rid, data.Damage1);
                if (insertResult > 0)
                {
                    return Json(insertResult);
                }
                else
                {
                    return Json("Error: Could not add the damage");
                }
            }
            else
            {
                return RedirectToAction("AccessDenied", "Account");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteRoomAmenity([FromBody]Amenity data)
        {
            if (isEmployee())
            {
                if (deleteRoomAmenity(data.Aid))
                {
                    return Json("Success");
                }
                else
                {
                    return Json("Error: Could not delete the amenity");
                }
            }
            else
            {
                return RedirectToAction("AccessDenied", "Account");
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddRoomAmenity([FromBody]Amenity data)
        {
            if (isEmployee())
            {
                int insertResult = addRoomAmenity(data.Rid, data.Amenity1);
                if (insertResult > 0)
                {
                    return Json(insertResult);
                }
                else
                {
                    return Json("Error: Could not add the amenity");
                }
            }
            else
            {
                return RedirectToAction("AccessDenied", "Account");
            }
        }

        #endregion

        #region ManageBookings

        [HttpGet]
        public IActionResult ManageBookings()
        {
            if (isEmployee())
            {
                ManageBookingsViewModel model = new ManageBookingsViewModel(_context);
                return View(model);
            }
            else
            {
                return RedirectToAction("AccessDenied", "Account");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ManageBookings(ManageBookingsViewModel model)
        {
            if (isEmployee())
            {
                model.initModel(_context, model.SelectedHotel);
                return View(model);
            }
            else
            {
                return RedirectToAction("AccessDenied", "Account");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RentBooking(PaymentViewModel model)
        {
            if (isEmployee())
            {
                if(bookingToRenting(Convert.ToInt32(model.booking.Bid)) > 0)
                {
                    TempData["SuccessMessage"] = "Payment successful, booking converted to renting";
                    return RedirectToAction("ManageBookings");
                }
                else
                {
                    TempData["ErrorMessage"] = "Payment failed, the booking is invalid";
                    return RedirectToAction("ManageBookings");
                }
            }
            else
            {
                return RedirectToAction("AccessDenied", "Account");
            }
        }

        [HttpGet]
        public IActionResult Payment(Dictionary<string, string> parms)
        {
            if (isEmployee())
            {
                DBManipulation db = new DBManipulation(_context);
                int rid = Convert.ToInt32(parms.GetValueOrDefault("roomid"));
                int bid = Convert.ToInt32(parms.GetValueOrDefault("bid"));
                int ssn = Convert.ToInt32(parms.GetValueOrDefault("customerSSN"));
                DateTime startdate = DateTime.Parse(parms.GetValueOrDefault("startdate"));
                DateTime enddate = DateTime.Parse(parms.GetValueOrDefault("enddate"));

                Room room = db.getRoom(rid);

                Booking newBooking = new Booking()
                {
                    CustomerSsn = ssn,
                    Bid = bid,
                    R = room,
                    Rid = room.Rid,
                    StartDate = startdate,
                    EndDate = enddate
                };
                PaymentViewModel model = new PaymentViewModel(newBooking);
                return View(model);
            }
            else
            {
                return RedirectToAction("AccessDenied", "Account");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteBooking([FromBody]Booking data)
        {
            if (isEmployee())
            {
                if (deleteBooking(Convert.ToInt32(data.Bid)))
                {
                    return Json("Success");
                }
                else
                {
                    return Json("Error: Could not delete the booking");
                }
            }
            else
            {
                return RedirectToAction("AccessDenied", "Account");
            }

        }

        [HttpGet]
        public IActionResult ArchiveRenting()
        {
            if (isEmployee())
            {
                TempData["SuccessMessage"] = "Archiving successful";
                archiveRenting();
                return RedirectToAction("EmployeeSection");
            }
            else
            {
                return RedirectToAction("AccessDenied", "Account");
            }
        }
        #endregion

        #region ManageHotelChain

        [HttpGet]
        public IActionResult ManageHotelChains()
        {
            if (isEmployee())
            {
                List<Hotelchain> hotelChains = new DBManipulation(_context).getHotelChains();
                return View(hotelChains);
            }
            else
            {
                return RedirectToAction("AccessDenied", "Account");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteHotelChain([FromBody]Hotelchain data)
        {
            if (isEmployee())
            {
                if (deleteHotelChain(Convert.ToInt32(data.Hcid)))
                {
                    return Json("Success");
                }
                else
                {
                    return Json("Error: Could not delete the hotel");
                }
            }
            else
            {
                return RedirectToAction("AccessDenied", "Account");
            }

        }

        [HttpGet]
        public IActionResult CreateHotelChain()
        {
            if (isEmployee())
            {
                HotelChainViewModel model = new HotelChainViewModel();
                return View(model);
            }
            else
            {
                return RedirectToAction("AccessDenied", "Account");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateHotelChain(HotelChainViewModel model)
        {
            if (isEmployee())
            {
                if (ModelState.IsValid)
                {
                    Boolean insertResult = createHotelChain(model);
                    if (insertResult)
                    {
                        TempData["SuccessMessage"] = "Hotel chain created";
                        return RedirectToAction("ManageHotelChains");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, TempData["ErrorMessage"].ToString());
                    }
                }
                // If we got this far, something failed, redisplay form
                return View(model);
            }
            else
            {
                return RedirectToAction("AccessDenied", "Account");
            }
        }

        [HttpGet]
        public IActionResult EditHotelChain(String Hcid)
        {
            if (isEmployee())
            {
                Hotelchain hotelChain = getHotelChain(Convert.ToInt32(Hcid));
                HotelChainViewModel model = new HotelChainViewModel(_context, hotelChain);
                return View(model);
            }
            else
            {
                return RedirectToAction("AccessDenied", "Account");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditHotelChain(HotelChainViewModel model)
        {
            if (isEmployee())
            {
                ModelState.Remove("PhoneAdd");
                ModelState.Remove("EmailAdd");
                if (ModelState.IsValid)
                {
                    Boolean insertResult = updateHotelChain(convertModelToHotelChain(model));
                    if (insertResult)
                    {
                        TempData["SuccessMessage"] = "Hotel chain updated";
                        return RedirectToAction("ManageHotelChains");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, TempData["ErrorMessage"].ToString());
                    }
                }
                // If we got this far, something failed, redisplay form
                model.initModel(_context, Convert.ToInt32(model.Hcid));
                return View(model);
            }
            else
            {
                return RedirectToAction("AccessDenied", "Account");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteHotelChainPhone([FromBody]Hotelchainphone data)
        {
            if (isEmployee())
            {
                if (deleteHotelChainPhone(data.Hcid, data.PhoneNumber))
                {
                    return Json("Success");
                }
                else
                {
                    return Json("Error: Could not delete the phone number");
                }
            }
            else
            {
                return RedirectToAction("AccessDenied", "Account");
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddHotelChainPhone([FromBody]Hotelchainphone data)
        {
            if (isEmployee())
            {
                Boolean insertResult = addHotelChainPhone(data.Hcid, data.PhoneNumber);
                if (insertResult)
                {
                    return Json("Success");
                }
                else
                {
                    return Json("Error: Could not add the phone number");
                }
            }
            else
            {
                return RedirectToAction("AccessDenied", "Account");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteHotelChainEmail([FromBody]Hotelchainemail data)
        {
            if (isEmployee())
            {
                if (deleteHotelChainEmail(data.Hcid, data.Email))
                {
                    return Json("Success");
                }
                else
                {
                    return Json("Error: Could not delete the email");
                }
            }
            else
            {
                return RedirectToAction("AccessDenied", "Account");
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddHotelChainEmail([FromBody]Hotelchainemail data)
        {
            if (isEmployee())
            {
                Boolean insertResult = addHotelChainEmail(data.Hcid, data.Email);
                if (insertResult)
                {
                    return Json("Success");
                }
                else
                {
                    return Json("Error: Could not add the email");
                }
            }
            else
            {
                return RedirectToAction("AccessDenied", "Account");
            }
        }
        #endregion

        #region DBLogic
        private Boolean createHotel(HotelViewModel model)
        {
            return insertHotel(convertModelToHotel(model));
        }

        private Boolean insertHotel(Hotel model)
        {
            try
            {
                Object[] insertArray = new object[] { model.Hcid, model.HotelName, model.Manager, model.Category, model.NumRooms, model.StreetNumber, model.StreetName,
                model.AptNumber, model.City,model.HState,model.Zip, model.Email };
                _context.Database.ExecuteSqlCommand(
                   "INSERT INTO eHotel.Hotel (hcid,hotel_name,manager,category,num_rooms,street_number,street_name,apt_number,city,h_state,zip,email)" +
                   "VALUES ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11})",
                   parameters: insertArray);
                return true;
            }
            catch (PostgresException ex)
            {
                //TODO better error handling
                TempData["ErrorMessage"] = Constants.GENERICPOSTGREERROR;
                return false;
            }
        }

        private Boolean updateHotel(Hotel model)
        {
            Object[] insertArray = new object[] { model.Hcid, model.HotelName, model.Manager, model.Category, model.StreetNumber, model.StreetName,
                model.AptNumber, model.City,model.HState,model.Zip, model.Email, model.Hid };
            try
            {
                _context.Database.ExecuteSqlCommand(
                   "UPDATE eHotel.Hotel SET hcid={0}, hotel_name={1}, manager={2}, category={3}, street_number={4}, street_name={5}, apt_number={6}, city={7}, h_state={8}, zip={9}, email={10} " +
                   "WHERE hid={11}",
                   parameters: insertArray);
                return true;
            }
            catch (PostgresException ex)
            {
                //TODO better error handling
                TempData["ErrorMessage"] = Constants.GENERICPOSTGREERROR;
                return false;
            }
        }

        private Boolean deleteHotel(int Hid)
        {
            try
            {
                var numDelete = _context.Database.ExecuteSqlCommand("DELETE FROM eHotel.Hotel WHERE hid={0}", parameters: Hid);
                return numDelete == 1;
            }
            catch (PostgresException ex)
            {
                //TODO better error handling
                TempData["ErrorMessage"] = Constants.GENERICPOSTGREERROR;
                return false;
            }
        }

        private Hotel getHotel(int Hid)
        {
            try
            {
                Hotel hotel = _context.Hotel.FromSql("SELECT * FROM eHotel.hotel WHERE Hid={0}", parameters: Hid).ToList()[0];
                return hotel;
            }
            catch (PostgresException ex)
            {
                //TODO better error handling
                TempData["ErrorMessage"] = Constants.GENERICPOSTGREERROR;
                return null;
            }
        }

        private Boolean deleteHotelPhone(int Hid, string PhoneNumber)
        {
            try
            {
                var numDelete = _context.Database.ExecuteSqlCommand("DELETE FROM eHotel.HotelPhone WHERE hid={0} AND phone_number={1}", parameters: new object[] { Hid, PhoneNumber});
                return numDelete == 1;
            }
            catch (PostgresException ex)
            {
                //TODO better error handling
                TempData["ErrorMessage"] = Constants.GENERICPOSTGREERROR;
                return false;
            }
        }

        private Boolean addHotelPhone(int Hid, string PhoneNumber)
        {
            try
            {
                _context.Database.ExecuteSqlCommand("INSERT INTO eHotel.hotelphone VALUES ({0},{1})", parameters: new object[] { PhoneNumber, Hid });
                return true;
            }
            catch (PostgresException ex)
            {
                //TODO better error handling
                TempData["ErrorMessage"] = Constants.GENERICPOSTGREERROR;
                return false;
            }
        }

        private Boolean createRoom(RoomViewModel model)
        {
            return insertRoom(convertModelToRoom(model));
        }

        private Boolean insertRoom(Room model)
        {
            try
            {
                Object[] insertArray = new object[] { model.RoomNum, model.Hid, model.Price, model.Capacity, model.Isextandable, model.Landscape };
                _context.Database.ExecuteSqlCommand("INSERT INTO eHotel.Room (room_num,hid,price,capacity,isextandable,landscape)" +
                   "VALUES ({0},{1},{2},{3},{4},{5})", parameters: insertArray);
                return true;
            }
            catch (PostgresException ex)
            {
                //TODO better error handling
                TempData["ErrorMessage"] = Constants.GENERICPOSTGREERROR;
                return false;
            }
        }

        private Boolean deleteRoom(int Rid)
        {
            try
            {
                var numDelete = _context.Database.ExecuteSqlCommand("DELETE FROM eHotel.Room WHERE rid={0}", parameters: Rid);
                return numDelete == 1;
            }
            catch (PostgresException ex)
            {
                //TODO better error handling
                TempData["ErrorMessage"] = Constants.GENERICPOSTGREERROR;
                return false;
            }
        }

        private Room getRoom(int rid)
        {
            try
            {
                Room room = _context.Room.FromSql("SELECT * FROM eHotel.room WHERE rid={0}", parameters: rid).ToList()[0];
                return room;
            }
            catch (PostgresException ex)
            {
                //TODO better error handling
                TempData["ErrorMessage"] = Constants.GENERICPOSTGREERROR;
                return null;
            }
        }

        private Boolean updateRoom(Room model)
        {
            try
            {
                Object[] insertArray = new object[] { model.RoomNum, model.Hid, model.Price, model.Capacity, model.Isextandable, model.Landscape, model.Rid };
                _context.Database.ExecuteSqlCommand("UPDATE eHotel.room SET room_num={0},hid={1},price={2},capacity={3},isextandable={4},landscape={5} WHERE rid={6}", parameters: insertArray);
                return true;
            }
            catch (PostgresException ex)
            {
                //TODO better error handling
                TempData["ErrorMessage"] = Constants.GENERICPOSTGREERROR;
                return false;
            }
        }

        private Boolean deleteRoomDamage(int Did)
        {
            try
            {
                var numDelete = _context.Database.ExecuteSqlCommand("DELETE FROM eHotel.Damage WHERE did={0}", parameters: Did);
                return numDelete == 1;
            }
            catch (PostgresException ex)
            {
                //TODO better error handling
                TempData["ErrorMessage"] = Constants.GENERICPOSTGREERROR;
                return false;
            }
        }

        private int addRoomDamage(int Rid, string damage)
        {
            try
            {
                _context.Database.ExecuteSqlCommand("INSERT INTO eHotel.Damage (damage,rid) VALUES ({0},{1})", parameters: new object[] { damage, Rid });
                var damageResult = _context.Damage.FromSql("SELECT * FROM eHotel.Damage WHERE damage={0} AND rid={1}", parameters: new object[] { damage, Rid }).ToList()[0];
                return damageResult.Did;
            }
            catch (PostgresException ex)
            {
                //TODO better error handling
                TempData["ErrorMessage"] = Constants.GENERICPOSTGREERROR;
                return -1;
            }
        }

        private Boolean deleteRoomAmenity(int Aid)
        {
            try
            {
                var numDelete = _context.Database.ExecuteSqlCommand("DELETE FROM eHotel.Amenity WHERE aid={0}", parameters: Aid);
                return numDelete == 1;
            }
            catch (PostgresException ex)
            {
                //TODO better error handling
                TempData["ErrorMessage"] = Constants.GENERICPOSTGREERROR;
                return false;
            }
        }

        private int addRoomAmenity(int Rid, string amenity)
        {
            try
            {
                _context.Database.ExecuteSqlCommand("INSERT INTO eHotel.Amenity (amenity,rid) VALUES ({0},{1})", parameters: new object[] { amenity, Rid });
                var amenityResult = _context.Amenity.FromSql("SELECT * FROM eHotel.Amenity WHERE amenity={0} AND rid={1}", parameters: new object[] { amenity, Rid }).ToList()[0];
                return amenityResult.Aid;
            }
            catch (PostgresException ex)
            {
                //TODO better error handling
                TempData["ErrorMessage"] = Constants.GENERICPOSTGREERROR;
                return -1;
            }
        }

        private int bookingToRenting(int bid)
        {
            try
            {
                //GET booking from bid
                Booking booking = _context.Booking.FromSql("SELECT * FROM eHotel.Booking WHERE bid={0}", parameters: bid).ToList()[0];

                //insert new renting created from the booking
                Object[] insertArray = new object[] { booking.Rid, booking.CustomerSsn, getUser().SSN, booking.StartDate, booking.EndDate };
                _context.Database.ExecuteSqlCommand("INSERT INTO eHotel.Renting (rid,customer_ssn,employee_ssn,start_date,end_date) VALUES ({0},{1},{2},{3},{4})", parameters: insertArray);

                //get inserted renting
                Renting rentingResult = _context.Renting.FromSql("SELECT * FROM eHotel.Renting WHERE rid={0} AND customer_ssn={1} AND employee_ssn={2} AND start_date={3} AND end_date={4}", parameters: insertArray).ToList()[0];

                //delete old booking
                var numDelete = _context.Database.ExecuteSqlCommand("DELETE FROM eHotel.Booking WHERE bid={0}", parameters: booking.Bid);

                //insert old booking in the archive table
                insertArray = new object[] { booking.Bid, booking.Rid, booking.CustomerSsn, booking.StartDate, booking.EndDate };
                _context.Database.ExecuteSqlCommand("INSERT INTO eHotel.bookingarc (baid,rid,customer_ssn,start_date,end_date) VALUES ({0},{1},{2},{3},{4})", parameters: insertArray);

                return rentingResult.Rentid;
            }
            catch (PostgresException ex)
            {
                //TODO better error handling
                TempData["ErrorMessage"] = Constants.GENERICPOSTGREERROR;
                return -1;
            }
        }

        private Booking getBooking(int bid)
        {
            try
            {
                //GET booking from bid
                Booking booking = _context.Booking.FromSql("SELECT * FROM eHotel.Booking WHERE bid={0}", parameters: bid).ToList()[0];
                return booking;
            }
            catch (PostgresException ex)
            {
                //TODO better error handling
                TempData["ErrorMessage"] = Constants.GENERICPOSTGREERROR;
                return null;
            }
        }

        private Boolean deleteBooking(int Bid)
        {
            try
            {
                var numDelete = _context.Database.ExecuteSqlCommand("DELETE FROM eHotel.booking WHERE bid={0}", parameters: Bid);
                return numDelete == 1;
            }
            catch (PostgresException ex)
            {
                //TODO better error handling
                TempData["ErrorMessage"] = Constants.GENERICPOSTGREERROR;
                return false;
            }
        }

        private Boolean deleteHotelChain(int Hcid)
        {
            try
            {
                var numDelete = _context.Database.ExecuteSqlCommand("DELETE FROM eHotel.hotelchain WHERE hcid={0}", parameters: Hcid);
                return numDelete == 1;
            }
            catch (PostgresException ex)
            {
                //TODO better error handling
                TempData["ErrorMessage"] = Constants.GENERICPOSTGREERROR;
                return false;
            }
        }

        private Boolean createHotelChain(HotelChainViewModel model)
        {
            return insertHotelChain(convertModelToHotelChain(model));
        }

        private Boolean insertHotelChain(Hotelchain model)
        {
            try
            {
                Object[] insertArray = new object[] { model.HotelChainName, model.StreetNumber, model.StreetName,
                model.AptNumber, model.City, model.HcState,model.Zip, model.NumHotels };
                _context.Database.ExecuteSqlCommand(
                   "INSERT INTO eHotel.hotelchain (hotel_chain_name,street_number,street_name,apt_number,city,hc_state,zip,num_hotels)" +
                   "VALUES ({0},{1},{2},{3},{4},{5},{6},{7})",
                   parameters: insertArray);
                return true;
            }
            catch (PostgresException ex)
            {
                //TODO better error handling
                TempData["ErrorMessage"] = Constants.GENERICPOSTGREERROR;
                return false;
            }
        }

        private Hotelchain getHotelChain(int Hcid)
        {
            try
            {
                Hotelchain hotelChain = _context.Hotelchain.FromSql("SELECT * FROM eHotel.hotelchain WHERE Hcid={0}", parameters: Hcid).ToList()[0];
                return hotelChain;
            }
            catch (PostgresException ex)
            {
                //TODO better error handling
                TempData["ErrorMessage"] = Constants.GENERICPOSTGREERROR;
                return null;
            }
        }

        private Boolean updateHotelChain(Hotelchain model)
        {
            Object[] insertArray = new object[] { model.HotelChainName, model.StreetNumber, model.StreetName,
                model.AptNumber, model.City,model.HcState,model.Zip,model.Hcid };
            try
            {
                _context.Database.ExecuteSqlCommand("UPDATE eHotel.hotelchain SET hotel_chain_name={0}, street_number={1}, street_name={2}, apt_number={3}, city={4}, hc_state={5}, zip={6} WHERE hcid={7}", parameters: insertArray);
                return true;
            }
            catch (PostgresException ex)
            {
                //TODO better error handling
                TempData["ErrorMessage"] = Constants.GENERICPOSTGREERROR;
                return false;
            }
        }

        private Boolean deleteHotelChainPhone(int Hcid, string PhoneNumber)
        {
            try
            {
                var numDelete = _context.Database.ExecuteSqlCommand("DELETE FROM eHotel.hotelchainphone WHERE hcid={0} AND phone_number={1}", parameters: new object[] { Hcid, PhoneNumber });
                return numDelete == 1;
            }
            catch (PostgresException ex)
            {
                //TODO better error handling
                TempData["ErrorMessage"] = Constants.GENERICPOSTGREERROR;
                return false;
            }
        }

        private Boolean addHotelChainPhone(int Hcid, string PhoneNumber)
        {
            try
            {
                _context.Database.ExecuteSqlCommand("INSERT INTO eHotel.hotelchainphone VALUES ({0},{1})", parameters: new object[] { PhoneNumber, Hcid });
                return true;
            }
            catch (PostgresException ex)
            {
                //TODO better error handling
                TempData["ErrorMessage"] = Constants.GENERICPOSTGREERROR;
                return false;
            }
        }

        private Boolean deleteHotelChainEmail(int Hcid, string Email)
        {
            try
            {
                var numDelete = _context.Database.ExecuteSqlCommand("DELETE FROM eHotel.hotelchainemail WHERE hcid={0} AND email={1}", parameters: new object[] { Hcid, Email });
                return numDelete == 1;
            }
            catch (PostgresException ex)
            {
                //TODO better error handling
                TempData["ErrorMessage"] = Constants.GENERICPOSTGREERROR;
                return false;
            }
        }

        private Boolean addHotelChainEmail(int Hcid, string Email)
        {
            try
            {
                _context.Database.ExecuteSqlCommand("INSERT INTO eHotel.hotelchainemail VALUES ({0},{1})", parameters: new object[] { Email, Hcid });
                return true;
            }
            catch (PostgresException ex)
            {
                //TODO better error handling
                TempData["ErrorMessage"] = Constants.GENERICPOSTGREERROR;
                return false;
            }
        }

        private Boolean archiveRenting()
        {
            try
            {
                //GET old renting
                List<Renting> rentings = _context.Renting.FromSql("SELECT * FROM eHotel.renting WHERE end_date<{0}", parameters: DateTime.Today.AddMonths(-1)).ToList();

                string query = "";
                Boolean first = true;
                foreach (Renting r in rentings)
                {
                    if (first)
                    {
                        query += r.Rentid.ToString();
                        first = false;
                    }
                    else
                    {
                        query += "," + r.Rentid.ToString();
                    }
                }

                //Delete old rentings
                var numDelete = _context.Database.ExecuteSqlCommand("DELETE FROM eHotel.renting WHERE rentid IN ("+query+")");

                //Insert renting in archive table
                foreach (Renting r in rentings)
                {
                    _context.Database.ExecuteSqlCommand("INSERT INTO eHotel.rentingarc (rentaid,rid,customer_ssn,employee_ssn,start_date,end_date) VALUES ({0},{1},{2},{3},{4},{5})",
                        r.Rentid, r.Rid, r.CustomerSsn, r.EmployeeSsn, r.StartDate, r.EndDate);
                }

                 
                return true;
            }
            catch (PostgresException ex)
            {
                //TODO better error handling
                TempData["ErrorMessage"] = Constants.GENERICPOSTGREERROR;
                return false;
            }
        }
        #endregion

        #region Helpers

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

        private Hotel convertModelToHotel(HotelViewModel model)
        {
            return new Hotel
            {
                Hid = Convert.ToInt32(model.Hid),
                Hcid = model.HotelChainID,
                HotelName = model.HotelName,
                Manager = model.ManagerSSN,
                Category = model.Category,
                NumRooms = 0,
                StreetNumber = model.StreetNumber,
                StreetName = model.StreetName,
                AptNumber = model.AptNumber,
                City = model.City,
                HState = model.State,
                Zip = model.Zip,
                Email = model.Email
            };
        }

        private Room convertModelToRoom(RoomViewModel model)
        {
            return new Room
            {
                Rid = Convert.ToInt32(model.Rid),
                Hid = model.HotelID,
                RoomNum = model.RoomNum,
                Price = model.Price,
                Capacity = Convert.ToInt16(model.Capacity),
                Landscape = model.Landscape,
                Isextandable = model.Isextandable
            };
        }

        private Hotelchain convertModelToHotelChain(HotelChainViewModel model)
        {
            return new Hotelchain
            {
                Hcid = Convert.ToInt32(model.Hcid),
                HotelChainName = model.HotelChainName,
                NumHotels = 0,
                StreetNumber = model.StreetNumber,
                StreetName = model.StreetName,
                AptNumber = model.AptNumber,
                City = model.City,
                HcState = model.State,
                Zip = model.Zip
            };
        }

        #endregion
    }
}
