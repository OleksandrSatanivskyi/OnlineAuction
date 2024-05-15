using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WheelOfFortune.Application.Services.Auth;
using WheelOfFortune.Models.Auth;
using WheelOfFortune.Presentation.Models.Auth;
using Controller = Microsoft.AspNetCore.Mvc.Controller;
using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;
using HttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;

namespace WheelOfFortune.Presentation.Controllers
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
            var cultureCookieValue = Request.Cookies[CookieRequestCultureProvider.DefaultCookieName];

            if (HttpContext.User.Identity.IsAuthenticated)
                return RedirectToAction("GetAll", "Wheel");

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login", "Auth");
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
            if (!ModelState.IsValid) return View(model);

            
            HttpContext.Session.Remove("SignUpEmail");
            await AuthService.ConfirmEmail(model);

            return RedirectToAction("SuccessSignUp", "Auth");
        }

        public IActionResult SetCulture(string culture, string returnUrl)
        {
            if (!string.IsNullOrWhiteSpace(culture))
            {
                Response.Cookies.Append(
                    CookieRequestCultureProvider.DefaultCookieName,
                    CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                    new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
                );
            }

            return LocalRedirect(returnUrl);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if(!ModelState.IsValid) return View(model);

            var claims = await AuthService.Login(model);
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var properties = new AuthenticationProperties()
            {
                AllowRefresh = true,
                IsPersistent = true,
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), properties);

            return RedirectToAction("GetAllWheels", "Wheel");
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpModel model)
        {
            if (!ModelState.IsValid) return View(model);

            HttpContext.Session.SetString("SignUpEmail", model.Email);
            await AuthService.SignUp(model);
            return RedirectToAction("ConfirmEmail", "Auth");
        }
    }
}
