using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using FortuneWheel.Services;
using FortuneWheel.Models.Wheels;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;

namespace FortuneWheel.Controllers
{
    public class WheelController : Controller
    {
        private readonly IWheelService WheelService;

        public WheelController(IWheelService wheelService)
        {
            WheelService = wheelService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateWheel(CreateWheelModel model)
        {
            if (model.Title.IsNullOrEmpty() || model.Title.Length < 2) 
            {
                HttpContext.Request.Path = "/Wheel/GetAllWheels";
                throw new ValidationException("Title must not be empty and must be at least 2 characters long.");
            }
            
            model.UserId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            await WheelService.CreateWheel(model);

            return RedirectToAction("GetAllWheels", "Wheel");
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWheels()
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var model = new GetAllWheelsModel
            {
                Wheels = await WheelService.GetUserWheels(userId)
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> GetAllWheels(GetAllWheelsModel model)
        {
            return View();
        }
    }
}
