using FortuneWheel.Models;
using FortuneWheel.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WheelOfFortune.Presentation.Models;

namespace WheelOfFortune.Presentation.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAccountService AccountService;
        private Guid UserId => Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        
        public HomeController(IAccountService accountService)
        {
            AccountService = accountService;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> Settings()
        {
            var account = await AccountService.Get(UserId);

            return View(account);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAccount(AccountModel model)
        {
            await AccountService.Update(model);

            if (!string.IsNullOrWhiteSpace(model.Language))
            {
                Response.Cookies.Append(
                    CookieRequestCultureProvider.DefaultCookieName,
                    CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(model.Language)),
                    new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) });
            }

            return Redirect("Settings");
        }

        public async Task<IActionResult> Privacy()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Error()
        {
            var model = new ErrorModel()
            {
                Message = HttpContext.Request.Query["message"],
                PreviousPageRoute = HttpContext.Request.Query["route"]
            };

            return View(model);
        }
    }
}
