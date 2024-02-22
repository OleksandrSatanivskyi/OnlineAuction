using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using FortuneWheel.Services;
using FortuneWheel.Models.Wheels;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using FortuneWheel.Domain.WheelsOfFortune;

namespace FortuneWheel.Controllers
{
    public class WheelController : Controller
    {
        private readonly IWheelService WheelService;
        private Guid UserId => Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

        private string CurrentWheelId
        {
            get
            {
                return HttpContext.Session.GetString("CurrentWheelId");
            }
            set
            {
                HttpContext.Session.SetString("CurrentWheelId", value);
            }
        }

        private string CurrentWheelType
        {
            get
            {
                return HttpContext.Session.GetString("CurrentWheelType");
            }
            set
            {
                HttpContext.Session.SetString("CurrentWheelType", value);
            }
        }

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

            model.UserId = UserId;
            await WheelService.CreateWheel(model);

            return RedirectToAction("GetAll", "Wheel");
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var model = new GetAllWheelsModel
            {
                Wheels = await WheelService.GetAll(UserId)
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
            if(CurrentWheelId == id.ToString())
            {
                CurrentWheelId = "";
                CurrentWheelType = "";
            }

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

                    CurrentWheelId = wheelId.ToString();
                    CurrentWheelType = type.ToString();

                    return View("ClassicWheelOptions", classicModel);

                case WheelType.Point:
                    var pointModel = new PointWheelModel
                    {
                        Wheel = await WheelService.GetPointWheel(wheelId)
                    };

                    CurrentWheelId = wheelId.ToString();
                    CurrentWheelType = type.ToString();

                    return View("PointWheelOptions", pointModel);

                default:
                    throw new InvalidOperationException("Unsupported type of wheel");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Options()
        {
            if(CurrentWheelId.IsNullOrEmpty() 
                || CurrentWheelType.IsNullOrEmpty())
            {
                var wheels = await WheelService.GetAll(UserId);

                if(wheels != null && wheels.Count > 0)
                {
                    var wheel = wheels[0];

                    CurrentWheelId = wheel.Id.ToString();
                    CurrentWheelType = wheel.Type.ToString();
                }
                else
                {
                    return View();
                }
            }

            Enum.TryParse(CurrentWheelType, out WheelType type);
            Guid.TryParse(CurrentWheelId, out Guid id);

            switch (type)
            {
                case WheelType.Classic:
                    var classicModel = new ClassicWheelModel
                    {
                        Wheel = await WheelService.GetClassicWheel(id)
                    };

                    return View("ClassicWheelOptions", classicModel);

                case WheelType.Point:
                    var pointModel = new PointWheelModel
                    {
                        Wheel = await WheelService.GetPointWheel(id)
                    };

                    return View("PointWheelOptions", pointModel);

                default:
                    return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> PointWheelOptions(PointWheelModel model)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddSegment(PointWheelModel model)
        {
            await WheelService.AddPointSegment(model.Wheel.Id, model.Title, model.Points, model.ColorHex);

            return View("PointWheelOptions");
        }
    }
}
