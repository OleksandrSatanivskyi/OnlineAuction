using FortuneWheel.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using WheelOfFortune.Domain.WheelsOfFortune;
using WheelOfFortune.Exceptions;
using WheelOfFortune.Models.Wheels;
using WheelOfFortune.Services;

namespace WheelOfFortune.Controllers
{
    public class WheelController : Controller
    {
        private readonly IWheelService WheelService;
        private readonly IGuestService GuestService;
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

        public WheelController(IWheelService wheelService, IGuestService guestService)
        {
            WheelService = wheelService;
            GuestService = guestService;
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

            if (User.Identity.IsAuthenticated)
            {
                await WheelService.CreateWheel(model);
            }
            else
            {
                await GuestService.CreateWheel(model, HttpContext);
            }

            return RedirectToAction("GetAll", "Wheel");
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            GetAllWheelsModel model;

            if (User.Identity.IsAuthenticated)
            {
                model = new GetAllWheelsModel
                {
                    Wheels = await WheelService.GetAll(UserId)
                };
            }
            else
            {
                model = new GetAllWheelsModel
                {
                    Wheels = await GuestService.GetAll(HttpContext)
                };
            }
            
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

            if (User.Identity.IsAuthenticated)
            {
                await WheelService.Remove(id, type);
            }
            else
            {
                await GuestService.Remove(id, type, HttpContext);
            }
            
            return RedirectToAction("GetAll", "Wheel");
        }

        [HttpGet]
        public async Task<IActionResult> Select(Guid id, WheelType type)
        {
            switch (type)
            {
                case WheelType.Classic:
                    ClassicWheelModel classicModel;

                    if (User.Identity.IsAuthenticated)
                    {
                        classicModel = new ClassicWheelModel
                        {
                            Wheel = await WheelService.GetClassicWheel(id)
                        };
                    }
                    else
                    {
                        classicModel = new ClassicWheelModel
                        {
                            Wheel = await GuestService.GetClassicWheel(id, HttpContext)
                        };
                    }

                    CurrentWheelId = id.ToString();
                    CurrentWheelType = type.ToString();

                    return View("ClassicWheelOptions", classicModel);

                case WheelType.Point:
                    PointWheelModel pointModel;

                    if (User.Identity.IsAuthenticated)
                    {
                        pointModel = new PointWheelModel
                        {
                            Wheel = await WheelService.GetPointWheel(id)
                        };
                    }
                    else
                    {
                        pointModel = new PointWheelModel
                        {
                            Wheel = await GuestService.GetPointWheel(id, HttpContext)
                        };
                    }

                    CurrentWheelId = id.ToString();
                    CurrentWheelType = type.ToString();

                    return View("PointWheelOptions", pointModel);

                default:
                    throw new InvalidOperationException("Unsupported type of wheel");
            }
        }

        [HttpGet]
        public async Task<IActionResult> NoWheelSelected()
        {
            return View();
        }
        
        [HttpGet]
        public async Task<IActionResult> Options()
        {
            if(CurrentWheelId.IsNullOrEmpty() 
                || CurrentWheelType.IsNullOrEmpty())
            {
                List<WheelItem> wheels;

                if (User.Identity.IsAuthenticated)
                {
                    wheels = await WheelService.GetAll(UserId);
                }
                else
                {
                    wheels = await GuestService.GetAll(HttpContext);
                }

                if (wheels != null && wheels.Count > 0)
                {
                    var wheel = wheels[0];

                    CurrentWheelId = wheel.Id.ToString();
                    CurrentWheelType = wheel.Type.ToString();
                }
                else
                {
                    return RedirectToAction("NoWheelSelected", "Wheel");
                }
            }

            Enum.TryParse(CurrentWheelType, out WheelType type);
            Guid.TryParse(CurrentWheelId, out Guid id);

            switch (type)
            {
                case WheelType.Classic:
                    ClassicWheelModel classicModel;

                    if (User.Identity.IsAuthenticated)
                    {
                        classicModel = new ClassicWheelModel
                        {
                            Wheel = await WheelService.GetClassicWheel(id)
                        };
                    }
                    else
                    {
                        classicModel = new ClassicWheelModel
                        {
                            Wheel = await GuestService.GetClassicWheel(id, HttpContext)
                        };
                    }                  

                    return View("ClassicWheelOptions", classicModel);

                case WheelType.Point:
                    PointWheelModel pointModel;

                    if (User.Identity.IsAuthenticated)
                    {
                        pointModel = new PointWheelModel
                        {
                            Wheel = await WheelService.GetPointWheel(id)
                        };
                    }
                    else
                    {
                        pointModel = new PointWheelModel
                        {
                            Wheel = await GuestService.GetPointWheel(id, HttpContext)
                        };
                    }

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

        [HttpGet]
        public async Task<IActionResult> ClassicWheelOptions(ClassicWheelModel model)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddPointSegment(PointWheelModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                await WheelService.AddPointSegment(model.Wheel.Id, model.Title, model.Points, model.ColorHex);
                model.Wheel = await WheelService.GetPointWheel(model.Wheel.Id);
            }
            else
            {
                await GuestService.AddPointSegment(model.Wheel.Id, model.Title, model.Points, model.ColorHex, HttpContext);
                model.Wheel = await GuestService.GetPointWheel(model.Wheel.Id, HttpContext);
            }

            return RedirectToAction("Options", "Wheel");
        }

        [HttpPost]
        public async Task<IActionResult> AddSegment(ClassicWheelModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                await WheelService.AddClassicSegment(model.Wheel.Id, model.Title, model.ColorHex);
                model.Wheel = await WheelService.GetClassicWheel(model.Wheel.Id);
            }
            else
            {
                await GuestService.AddClassicSegment(model.Wheel.Id, model.Title, model.ColorHex, HttpContext);
                model.Wheel = await GuestService.GetClassicWheel(model.Wheel.Id, HttpContext);
            }

            return RedirectToAction("Options", "Wheel");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSegment(Guid Id, WheelType Type)
        {
            switch (Type)
            {
                case WheelType.Classic:
                    if (User.Identity.IsAuthenticated)
                    {
                        await WheelService.DeleteClassicWheelSegment(Id);
                    }
                    else
                    {
                        await GuestService.DeleteClassicWheelSegment(Id, HttpContext);
                    }
                    
                    break;
                case WheelType.Point:
                    if (User.Identity.IsAuthenticated)
                    {
                        await WheelService.DeletePointWheelSegment(Id);
                    }
                    else
                    {
                        await GuestService.DeletePointWheelSegment(Id, HttpContext);
                    }
                    
                    break;
                default:
                    throw new NotFoundException("Type was not found.");
            }

            return Redirect("Options");
        }

        public async Task<IActionResult> UpdatePointWheelOptions([FromBody] UpdatePointWheelOptionsModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                await WheelService.UpdatePointWheelOptions(model);
            }
            else
            {
                await GuestService.UpdatePointWheelOptions(model, HttpContext);
            }
            
            return RedirectToAction("Options", "Wheel");
        }

        public async Task<IActionResult> UpdateClassicWheelOptions([FromBody] UpdateClassicWheelOptionsModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                await WheelService.UpdateClassicWheelOptions(model);
            }
            else
            {
                await GuestService.UpdateClassicWheelOptions(model, HttpContext);
            }

            return RedirectToAction("Options", "Wheel");
        }

        public async Task<IActionResult> Load()
        {
            if (CurrentWheelId.IsNullOrEmpty()
                || CurrentWheelType.IsNullOrEmpty())
            {
                List<WheelItem> wheels;

                if (User.Identity.IsAuthenticated)
                {
                    wheels = await WheelService.GetAll(UserId);
                }
                else
                {
                    wheels = await GuestService.GetAll(HttpContext);
                }

                if (wheels != null && wheels.Count > 0)
                {
                    var wheel = wheels[0];

                    CurrentWheelId = wheel.Id.ToString();
                    CurrentWheelType = wheel.Type.ToString();
                }
                else
                {
                    return View("NoWheelSelected");
                }
            }

            Enum.TryParse(CurrentWheelType, out WheelType type);
            Guid.TryParse(CurrentWheelId, out Guid id);

            if (type == WheelType.Classic) 
            {
                ClassicWheel wheel;

                if (User.Identity.IsAuthenticated)
                {
                    wheel = await WheelService.GetClassicWheel(id);
                }
                else
                {
                    wheel = await GuestService.GetClassicWheel(id, HttpContext);
                }

                var model = new LoadClassicWheelModel()
                {
                    WheelId = wheel.Id,
                    RemainingOptions = wheel.Segments
                };

                return View("LoadClassicWheel", model);
            }
            else if(type == WheelType.Point)
            {
                PointWheel wheel;

                if (User.Identity.IsAuthenticated)
                {
                    wheel = await WheelService.GetPointWheel(id);
                }
                else
                {
                    wheel = await GuestService.GetPointWheel(id, HttpContext);
                }

                var model = new LoadPointWheelModel()
                {
                    WheelId = wheel.Id,
                    RemainingOptions = wheel.Segments
                };

                return View("LoadPointWheel", model);
            }
            else
            {
                throw new InvalidOperationException("Wheel type is not correct.");
            }
        }

        public async Task<IActionResult> LoadClassicWheel(LoadClassicWheelModel model)
        {
            return View();
        }

        public async Task<IActionResult> LoadPointWheel(LoadPointWheelModel model)
        {
            return View();
        }
    }
}
