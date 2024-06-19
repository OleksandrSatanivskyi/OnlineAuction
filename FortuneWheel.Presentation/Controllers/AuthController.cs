using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using OnlineAuc.Application.Services.Auth;
using OnlineAuc.Models.Auth;
using OnlineAuc.Presentation.Models.Auth;
using System.Globalization;
using System.Security.Claims;
using System.Text.RegularExpressions;
using Controller = Microsoft.AspNetCore.Mvc.Controller;
using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;
using HttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;

namespace OnlineAuc.Presentation.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService AuthService;

        public AuthController(IAuthService authService)
        {
            AuthService = authService;
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
                return RedirectToAction("GetAll", "Auction");

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("GetAll", "Auction");
        }

        [HttpGet]
        public async Task<IActionResult> SignUp()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> SuccessSignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmEmail(ConfirmEmailModel model)
        {
            model.Email = HttpContext.Session.GetString("SignUpEmail");

            ValidateConfirmEmailModel(model);
            if (ViewBag.CodeError != null || ViewBag.EmailError != null)
            {
                return View(model);
            }

            HttpContext.Session.Remove("SignUpEmail");
            await AuthService.ConfirmEmail(model);

            return RedirectToAction("SuccessSignUp", "Auth");
        }

        private void ValidateConfirmEmailModel(ConfirmEmailModel model)
        {
            var cultureFeature = HttpContext.Features.Get<IRequestCultureFeature>();
            var culture = cultureFeature?.RequestCulture.UICulture ?? CultureInfo.CurrentUICulture;

            if (model == null)
            {
                ViewBag.Error = CultureHelper.Exception("Invalid input.", culture);
                return;
            }

            var codeRegex = new Regex("^[0-9]{5}$");
            if (model.Code == null || !codeRegex.IsMatch(model.Code))
            {
                ViewBag.CodeError = CultureHelper.Exception("Invalid code format. The code should consist of 5 digits.", culture);
            }

            if (model.Email == null || !IsEmailValid(model.Email))
            {
                ViewBag.EmailError = CultureHelper.Exception("Invalid email format.", culture);
            }
        }

        private bool IsEmailValid(string email)
        {
            var emailRegex = new Regex("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$");
            return emailRegex.IsMatch(email);
        }

        private void ValidateLoginModel(LoginModel model)
        {
            var cultureFeature = HttpContext.Features.Get<IRequestCultureFeature>();
            var culture = cultureFeature?.RequestCulture.UICulture ?? CultureInfo.CurrentUICulture;

            if (model == null)
            {
                ViewBag.Error = CultureHelper.Exception("Invalid input.", culture);
                return;
            }

            var emailRegex = new Regex("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$");
            if (model.Email == null || !emailRegex.IsMatch(model.Email))
            {
                ViewBag.EmailError = CultureHelper.Exception("Invalid email format.", culture);
            }

            var passwordRegex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,64}$");
            if (model.Password == null || !passwordRegex.IsMatch(model.Password))
            {
                ViewBag.PasswordError = CultureHelper.Exception("Incorrect password.", culture);
            }
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            ValidateLoginModel(model);
            if (ViewBag.EmailError != null || ViewBag.PasswordError != null)
            {
                return View(model);
            }

            var claims = await AuthService.Login(model);
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var properties = new AuthenticationProperties()
            {
                AllowRefresh = true,
                IsPersistent = true,
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), properties);

            return RedirectToAction("GetAll", "Auction");
        }


        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpModel model)
        {
            ValidateSignUpModel(model);

            if (ViewBag.EmailError != null || ViewBag.PasswordError != null || ViewBag.ConfirmPasswordError != null || ViewBag.NameError != null || ViewBag.SurnameError != null)
            {
                return View(model);
            }

            HttpContext.Session.SetString("SignUpEmail", model.Email);
            await AuthService.SignUp(model);
            return RedirectToAction("ConfirmEmail", "Auth");
        }

        private void ValidateSignUpModel(SignUpModel model)
        {
            var cultureFeature = HttpContext.Features.Get<IRequestCultureFeature>();
            var culture = cultureFeature?.RequestCulture.UICulture ?? CultureInfo.CurrentUICulture;

            if (model == null)
            {
                ViewBag.Error = CultureHelper.Exception("Invalid input.", culture);
                return;
            }

            var emailRegex = new Regex("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$");
            if (model.Email == null || !emailRegex.IsMatch(model.Email))
            {
                ViewBag.EmailError = CultureHelper.Exception("Invalid email format.", culture);
            }

            var passwordRegex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).*$");
            if (model.Password == null || !passwordRegex.IsMatch(model.Password))
            {
                ViewBag.PasswordError = CultureHelper.Exception("Password should contain at least one lowercase letter, one uppercase letter, and one digit.", culture);
            }
            if (model.Password == null || model.Password.Length < 8 || model.Password.Length > 64)
            {
                ViewBag.PasswordError = CultureHelper.Exception("Password length must be between 8 and 64 characters.", culture);
            }

            if (model.Password == null || model.ConfirmPassword == null || model.Password != model.ConfirmPassword)
            {
                ViewBag.ConfirmPasswordError = CultureHelper.Exception("Passwords do not match.", culture);
            }

            if (model.Name == null || string.IsNullOrWhiteSpace(model.Name))
            {
                ViewBag.NameError = CultureHelper.Exception("Name is required.", culture);
            }

            if (model.Surname == null || string.IsNullOrWhiteSpace(model.Surname))
            {
                ViewBag.SurnameError = CultureHelper.Exception("Surname is required.", culture);
            }
        }

    }
}
