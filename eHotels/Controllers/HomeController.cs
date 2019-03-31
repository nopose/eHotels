using System;
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
        
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult HomePage()
        {
            return View();
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
                return RedirectToAction("AccessDenied");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteHotel([FromBody]Hotel data)
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
                return RedirectToAction("AccessDenied");
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
                        return View(model);
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
                return RedirectToAction("AccessDenied");
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
                //FINDQUERY
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
                TempData["ErrorMessage"] = ex.MessageText;
                return false;
            }
        }

        private Boolean deleteHotel(int Hid)
        {
            try
            {
                //FINDQUERY
                var numDelete = _context.Database.ExecuteSqlCommand("DELETE FROM eHotel.Hotel WHERE hid={0}", parameters: Hid);
                return numDelete == 1;
            }
            catch (PostgresException ex)
            {
                //TODO better error handling
                TempData["ErrorMessage"] = ex.MessageText;
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

        #endregion
    }
}
