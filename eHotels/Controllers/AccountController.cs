using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using eHotels.Models;
using eHotels.Models.AccountViewModels;
using eHotels.Services;
using eHotels.Data;
using System.Data.SqlClient;
using System.Data;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace eHotels.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;

       

        public AccountController(
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
        public string ErrorMessage { get; set; }

        #region Login/Logout
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
                ViewData["ReturnUrl"] = returnUrl;
                if (ModelState.IsValid)
                {
                    // This doesn't count login failures towards account lockout
                    // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                    var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User logged in.");
                        return RedirectToLocal(returnUrl);
                    }
                    if (result.IsLockedOut)
                    {
                        _logger.LogWarning("User account locked out.");
                        return RedirectToAction(nameof(Lockout));
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                        return View(model);
                    }
                }

                // If we got this far, something failed, redisplay form
                return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Lockout()
        {
            return View();
        }
        #endregion

        #region Registers
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
                ViewData["ReturnUrl"] = returnUrl;
                return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            ModelState.Remove("DateEmployment");
            if (ModelState.IsValid)
            {
                Boolean insertResult = createCustomer(model);
                if(insertResult)
                {
                    int ssn = Convert.ToInt32(model.SSN);
                    var user = new ApplicationUser
                    {
                        UserName = model.Email,
                        Email = model.Email,
                        SSN = ssn,
                        Role = Constants.CUSTOMER
                    };
                    var result = await _userManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User created a new account with password.");

                        await _signInManager.SignInAsync(user, isPersistent: false);
                        _logger.LogInformation("User created a new account with password.");


                        return RedirectToLocal(returnUrl);
                    }
                    AddErrors(result);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, TempData["ErrorMessage"].ToString());
                }
                
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpGet]
        public IActionResult RegisterEmployee(string returnUrl = null)
        {
            if (isEmployee())
            {
                ViewData["ReturnUrl"] = returnUrl;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterEmployee(RegisterViewModel model, string returnUrl = null)
        {
            if (isEmployee())
            {
                ViewData["ReturnUrl"] = returnUrl;
                if (ModelState.IsValid)
                {
                    Boolean insertResult = createEmployee(model);
                    if (insertResult)
                    {
                        int ssn = Convert.ToInt32(model.SSN);
                        var user = new ApplicationUser
                        {
                            UserName = model.Email,
                            Email = model.Email,
                            SSN = ssn,
                            Role = Constants.EMPLOYEE
                        };
                        var result = await _userManager.CreateAsync(user, model.Password);
                        if (result.Succeeded)
                        {
                            await _signInManager.SignInAsync(user, isPersistent: false);

                            return RedirectToLocal(returnUrl);
                        }
                        AddErrors(result);
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

        #region Update

        [HttpGet]
        public IActionResult UpdateUser(string returnUrl = null)
        {
            //FINDQUERY
            ViewData["ReturnUrl"] = returnUrl;
            ApplicationUser user = getUser();
            Person userPerson = _context.Person.FromSql("SELECT * FROM eHotel.person WHERE SSN={0}", parameters: user.SSN).ToList()[0];
            RegisterViewModel model = new RegisterViewModel {
                SSN = userPerson.Ssn.ToString(),
                FullName = userPerson.FullName,
                StreetNumber = userPerson.StreetNumber,
                StreetName = userPerson.StreetName,
                AptNumber = userPerson.AptNumber,
                City = userPerson.City,
                State = userPerson.PState,
                Zip = userPerson.Zip
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateUser(RegisterViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            ModelState.Remove("SSN");
            ModelState.Remove("DateEmployment");
            ModelState.Remove("Email");
            ModelState.Remove("Password");
            ModelState.Remove("ConfirmPassword");
            ApplicationUser user = getUser();

            if (ModelState.IsValid && user.SSN.ToString().Equals(model.SSN))
            {
                Boolean insertResult = updatePerson(convertModelToPerson(model));
                if (insertResult)
                {
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, TempData["ErrorMessage"].ToString());
                }
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        #endregion

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

        #region DBLogic
        private Boolean createCustomer(RegisterViewModel model)
        {
            Boolean result = insertPerson(new Person
            {
                Ssn = Convert.ToInt32(model.SSN),
                FullName = model.FullName,
                StreetNumber = model.StreetNumber,
                StreetName = model.StreetName,
                AptNumber = model.AptNumber,
                City = model.City,
                PState = model.State,
                Zip = model.Zip
            });

            if (result)
            {
                result = insertCustomer(new Customer
                {
                    Ssn = Convert.ToInt32(model.SSN),
                    RegisterDate = DateTime.Now,
                    Username = model.Email,
                    Password = model.Password
                });
            }
            return result;
        }

        private Boolean insertPerson(Person model)
        {
            Object[] insertArray = new object[] {model.Ssn,model.FullName,model.StreetNumber,model.StreetName,
                model.AptNumber,model.City,model.PState,model.Zip };
            try
            {
                //FINDQUERY
                _context.Database.ExecuteSqlCommand(
                   "INSERT INTO eHotel.Person VALUES ({0},{1},{2},{3},{4},{5},{6},{7})",
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

        private Boolean updatePerson(Person model)
        {
            Object[] insertArray = new object[] {model.FullName,model.StreetNumber,model.StreetName,
                model.AptNumber,model.City,model.PState,model.Zip,model.Ssn };
            try
            {
                //FINDQUERY
                _context.Database.ExecuteSqlCommand(
                   "UPDATE eHotel.Person SET full_name={0}, street_number={1}, street_name={2}, apt_number={3}, city={4}, p_state={5}, zip={6} " +
                   "WHERE ssn={7}",
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

        private Boolean insertCustomer(Customer model)
        {
            try
            {
                //FINDQUERY
                Object[] insertArray = new object[] { model.Ssn, model.RegisterDate, model.Username, model.Password };
                _context.Database.ExecuteSqlCommand(
                   "INSERT INTO eHotel.Customer VALUES ({0},{1},{2},{3})",
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

        private Boolean createEmployee(RegisterViewModel model)
        {
            Boolean result = insertPerson(convertModelToPerson(model));

            if (result)
            {
                result = insertEmployee(new Employee
                {
                    Ssn = Convert.ToInt32(model.SSN),
                    DateOfEmployment = model.DateEmployment,
                    Username = model.Email,
                    Password = model.Password
                });
            }
            return result;
        }

        private Boolean insertEmployee(Employee model)
        {
            try
            {
                //FINDQUERY
                Object[] insertArray = new object[] { model.Ssn, model.DateOfEmployment, model.Username, model.Password };
                _context.Database.ExecuteSqlCommand(
                   "INSERT INTO eHotel.Employee VALUES ({0},{1},{2},{3})",
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
        #endregion


        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
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

        private Person convertModelToPerson(RegisterViewModel model)
        {
            return new Person {
                Ssn = Convert.ToInt32(model.SSN),
                FullName = model.FullName,
                StreetNumber = model.StreetNumber,
                StreetName = model.StreetName,
                AptNumber = model.AptNumber,
                City = model.City,
                PState = model.State,
                Zip = model.Zip
            };
        }

        #endregion
    }
}
