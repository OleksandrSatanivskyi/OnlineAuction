using FortuneWheel.Presentation.Models.Auth;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using FortuneWheel.Application.Services.Auth;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace FortuneWheel.Presentation.Controllers
{
    public class AuthController : Controller
    {
        public IAuthService AuthService { get; set; }

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

        [HttpPost]
        public async Task<IActionResult> SubmitLogin(LoginModel model)
        {
            var claims = await AuthService.Login(model);
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var properties = new AuthenticationProperties()
            {
                AllowRefresh = true,
                IsPersistent = true,
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), properties);

            return RedirectToAction("", "");
        }

        [HttpPost]
        public async Task<IActionResult> SubmitSignUp(SignUpModel model)
        {
            await AuthService.SignUp(model);
            return RedirectToAction("Auth", "Login");
        }
    }
}
