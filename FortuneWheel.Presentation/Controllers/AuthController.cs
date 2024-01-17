using FortuneWheel.Presentation.Models.Auth;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Net.Http;

namespace FortuneWheel.Presentation.Controllers
{
    public class AuthController : Controller
    {
        public AuthController()
        {
            
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
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
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SubmitSignUp(SignUpModel model)
        {
            return View();
        }
    }
}
