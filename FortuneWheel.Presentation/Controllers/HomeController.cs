using FortuneWheel.Presentation.Models;
using Microsoft.AspNetCore.Mvc;

namespace FortuneWheel.Presentation.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
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
