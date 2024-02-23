﻿
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using FortuneWheel.Services;
using FortuneWheel.Models.Wheels;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using FortuneWheel.Domain.WheelsOfFortune;
using FortuneWheel.Exceptions;

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
        public async Task<IActionResult> Select(Guid id, WheelType type)
        {
            switch (type)
            {
                case WheelType.Classic:
                    var classicModel = new ClassicWheelModel
                    {
                        Wheel = await WheelService.GetClassicWheel(id)
                    };

                    CurrentWheelId = id.ToString();
                    CurrentWheelType = type.ToString();

                    return View("ClassicWheelOptions", classicModel);

                case WheelType.Point:
                    var pointModel = new PointWheelModel
                    {
                        Wheel = await WheelService.GetPointWheel(id)
                    };

                    CurrentWheelId = id.ToString();
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
            model.Wheel = await WheelService.GetPointWheel(model.Wheel.Id);

            return RedirectToAction("Options", "Wheel");
        }

        public async Task<IActionResult> DeleteSegment(Guid Id, WheelType Type)
        {
            switch (Type)
            {
                case WheelType.Classic:
                    throw new NotFoundException("Type was not found.");
                    break;
                case WheelType.Point:
                    await WheelService.DeletePointWheelSegment(Id);
                    break;
                default:
                    throw new NotFoundException("Type was not found.");
            }

            return await Options();
        }

        public async Task<IActionResult> SaveChanges([FromBody] UpdatePointOptionsModel model)
        {
            await WheelService.UpdatePointOptions(model);

            return RedirectToAction("Options", "Wheel");
        }
    }
}
