using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using FortuneWheel.Services;
using FortuneWheel.Models.Wheels;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using FortuneWheel.Domain.WheelsOfFortune;
using Microsoft.AspNetCore.Http;

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
        public async Task<IActionResult> Create(CreateWheelModel model)
        {
            if (model.Title.IsNullOrEmpty() || model.Title.Length < 2)
            {
                HttpContext.Request.Path = "/Wheel/GetAll";
                throw new ValidationException("Title must not be empty and must be at least 2 characters long.");
            }

            model.UserId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            await WheelService.CreateWheel(model);

            return RedirectToAction("GetAll", "Wheel");
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var model = new GetAllWheelsModel
            {
                Wheels = await WheelService.GetAll(userId)
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> GetAll(GetAllWheelsModel model)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Remove(Guid id, WheelType type)
        {
            await WheelService.Remove(id, type);

            return RedirectToAction("GetAll", "Wheel");
        }

        [HttpGet]
        public async Task<IActionResult> Select(Guid wheelId, WheelType type)
        {
            switch (type)
            {
                case WheelType.Classic:
                    var classicModel = new ClassicWheelModel
                    {
                        Wheel = await WheelService.GetClassicWheel(wheelId)
                    };

                    HttpContext.Session.SetString("CurrentWheelId", wheelId.ToString());
                    HttpContext.Session.SetString("CurrentWheelType", type.ToString());

                    return View(classicModel);

                case WheelType.Point:
                    var pointModel = new PointWheelModel
                    {
                        Wheel = await WheelService.GetPointWheel(wheelId)
                    };

                    HttpContext.Session.SetString("CurrentWheelId", wheelId.ToString());
                    HttpContext.Session.SetString("CurrentWheelType", type.ToString());

                    return View(pointModel);

                default:
                    throw new InvalidOperationException("Unsupported type of wheel");
            }

            
        }
    }
}
