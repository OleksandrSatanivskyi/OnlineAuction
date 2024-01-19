using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using FortuneWheel.Application.Services.Auth;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using FortuneWheel.Models.Auth;
using FortuneWheel.Presentation.Models.Auth;
using FortuneWheel.Exceptions;
using System.Web.Mvc;
using Controller = Microsoft.AspNetCore.Mvc.Controller;
using HttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;
using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;

namespace FortuneWheel.Presentation.Controllers
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
                return RedirectToAction("", "");

            return View();
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

            return RedirectToAction("LoginRedirect", "Auth");
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
