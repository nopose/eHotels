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
            var allowed = isEmployee();
            allowed.Wait();
            if (allowed.Result)
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
            else
            {
                return RedirectToAction("About");
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Lockout()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            //var allowed = isEmployee();
            //allowed.Wait();
            //if (allowed.Result)
            //{
                ViewData["ReturnUrl"] = returnUrl;
                return View();
            //}
            //else
            //{
            //    return RedirectToAction("About");
            //}
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                int ssn = Convert.ToInt32(model.SSN);
                var user = new ApplicationUser { UserName = model.Email,
                                                Email = model.Email,
                                                SSN = ssn,
                                                Role = Constants.EMPLOYEE };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    //var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);
                    //await _emailSender.SendEmailConfirmationAsync(model.Email, callbackUrl);

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    _logger.LogInformation("User created a new account with password.");


                    //var success = createCustomer(model);
                    //success.Wait();
                    //if (success.Result)
                    var success = createCustomer(model);

                        return RedirectToLocal(returnUrl);
                }
                AddErrors(result);
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
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{userId}'.");
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

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

        private Boolean createCustomer(RegisterViewModel model)
        {
            //string connectionString = "Data Source=localhost:5432; Initial Catalog = LANDesk; Persist Security Info = True; User ID=postgres; Password=password1";

            //using (var context = new SqlConnection(connectionString))
            //{
            //    var cmd = new SqlCommand
            //    {
            //        CommandText = "INSERT INTO eHotel.Person" +
            //        "VALUES ("+model.SSN+","+model.FullName+","+model.StreetNumber + "," +model.StreetName+
            //        "," + model.AptNumber + "," + model.City + "," + model.State + "," + model.Zip+"); ",
            //        CommandType = CommandType.Text,
            //        Connection = context
            //    };

            //    context.Open();

            //    var reader = cmd.ExecuteReader();

            //    reader.Close();
            //}
            int ssn = Convert.ToInt32(model.SSN);
            Object[] testArray = new object[] {ssn,model.FullName,model.StreetNumber,model.StreetName,
                model.AptNumber,model.City,model.State,model.Zip };
            //var test = await _context.Database.ExecuteSqlCommandAsync(
            //    "INSERT INTO eHotel.Person VALUES ({0},{1},{2},{3},{4},{5},{6},{7})",
            //    parameters: testArray );
            var test2 = _context.Database.ExecuteSqlCommand(
               "INSERT INTO eHotel.Person VALUES ({0},{1},{2},{3},{4},{5},{6},{7})",
               parameters: testArray);
            return true;
        }

        private async Task<Boolean> isEmployee()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if(user != null)
            {
                return user.Role.Equals(Constants.EMPLOYEE);
            }
            else
            {
                return false;
            }
        }

        #endregion
    }
}
