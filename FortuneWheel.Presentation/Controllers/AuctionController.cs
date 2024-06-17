using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OnlineAuc.Domain.Auctions;
using OnlineAuc.Exceptions;
using OnlineAuc.Models.Auctions;
using OnlineAuc.Services;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace OnlineAuc.Controllers
{
    public class AuctionController : Controller
    {
        private readonly IAuctionService AuctionService;
        private readonly IGuestService GuestService;
        private Guid UserId => Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

        private string CurrentAuctionId
        {
            get
            {
                return HttpContext.Session.GetString("CurrentAuctionId");
            }
            set
            {
                HttpContext.Session.SetString("CurrentAuctionId", value);
            }
        }

        private string CurrentAuctionType
        {
            get
            {
                return HttpContext.Session.GetString("CurrentAuctionType");
            }
            set
            {
                HttpContext.Session.SetString("CurrentAuctionType", value);
            }
        }

        public AuctionController(IAuctionService auctionService, IGuestService guestService)
        {
            AuctionService = auctionService;
            GuestService = guestService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAuctionModel model)
        {
            if (model.Title.IsNullOrEmpty() || model.Title.Length < 2)
            {
                HttpContext.Request.Path = "/Auction/GetAll";
                throw new ValidationException("Title must not be empty and must be at least 2 characters long.");
            }
            
            if (User.Identity.IsAuthenticated)
            {
                model.UserId = UserId;
                await AuctionService.Create(model);
            }
            else
            {
                await GuestService.Create(model, HttpContext);
            }

            return RedirectToAction("GetAll", "Auction");
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            GetAllAuctionsModel model;

            if (User.Identity.IsAuthenticated)
            {
                model = new GetAllAuctionsModel
                {
                    Wheels = await AuctionService.GetAll(UserId)
                };
            }
            else
            {
                model = new GetAllAuctionsModel
                {
                    Wheels = await GuestService.GetAll(HttpContext)
                };
            }
            
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> GetAll(GetAllAuctionsModel model)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Remove(Guid id, AuctionType type)
        {
            if(CurrentAuctionId == id.ToString())
            {
                CurrentAuctionId = "";
                CurrentAuctionType = "";
            }

            if (User.Identity.IsAuthenticated)
            {
                await AuctionService.Remove(id, type);
            }
            else
            {
                await GuestService.Remove(id, type, HttpContext);
            }
            
            return RedirectToAction("GetAll", "Auction");
        }

        [HttpGet]
        public async Task<IActionResult> Select(Guid id, AuctionType type)
        {
            switch (type)
            {
                case AuctionType.Classic:
                    ClassicAuctionModel classicModel;

                    if (User.Identity.IsAuthenticated)
                    {
                        classicModel = new ClassicAuctionModel
                        {
                            Auction = await AuctionService.GetClassic(id)
                        };
                    }
                    else
                    {
                        classicModel = new ClassicAuctionModel
                        {
                            Auction = await GuestService.GetClassic(id, HttpContext)
                        };
                    }

                    CurrentAuctionId = id.ToString();
                    CurrentAuctionType = type.ToString();

                    return View("ClassicAuctionOptions", classicModel);

                case AuctionType.Point:
                    PointAuctionModel pointModel;

                    if (User.Identity.IsAuthenticated)
                    {
                        pointModel = new PointAuctionModel
                        {
                            Auction = await AuctionService.GetPoint(id)
                        };
                    }
                    else
                    {
                        pointModel = new PointAuctionModel
                        {
                            Auction = await GuestService.GetPoint(id, HttpContext)
                        };
                    }

                    CurrentAuctionId = id.ToString();
                    CurrentAuctionType = type.ToString();

                    return View("PointAuctionOptions", pointModel);

                default:
                    throw new InvalidOperationException("Unsupported type of auction.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> NoAuctionSelected()
        {
            return View();
        }
        
        [HttpGet]
        public async Task<IActionResult> Options()
        {
            if(CurrentAuctionId.IsNullOrEmpty() 
                || CurrentAuctionType.IsNullOrEmpty())
            {
                List<AuctionItem> auctions;

                if (User.Identity.IsAuthenticated)
                {
                    auctions = await AuctionService.GetAll(UserId);
                }
                else
                {
                    auctions = await GuestService.GetAll(HttpContext);
                }

                if (auctions != null && auctions.Count > 0)
                {
                    var auction = auctions[0];

                    CurrentAuctionId = auction.Id.ToString();
                    CurrentAuctionType = auction.Type.ToString();
                }
                else
                {
                    return RedirectToAction("NoAuctionSelected", "Auction");
                }
            }

            Enum.TryParse(CurrentAuctionType, out AuctionType type);
            Guid.TryParse(CurrentAuctionId, out Guid id);

            switch (type)
            {
                case AuctionType.Classic:
                    ClassicAuctionModel classicModel;

                    if (User.Identity.IsAuthenticated)
                    {
                        classicModel = new ClassicAuctionModel
                        {
                            Auction = await AuctionService.GetClassic(id)
                        };
                    }
                    else
                    {
                        classicModel = new ClassicAuctionModel
                        {
                            Auction = await GuestService.GetClassic(id, HttpContext)
                        };
                    }                  

                    return View("ClassicAuctionOptions", classicModel);

                case AuctionType.Point:
                    PointAuctionModel pointModel;

                    if (User.Identity.IsAuthenticated)
                    {
                        pointModel = new PointAuctionModel
                        {
                            Auction = await AuctionService.GetPoint(id)
                        };
                    }
                    else
                    {
                        pointModel = new PointAuctionModel
                        {
                            Auction = await GuestService.GetPoint(id, HttpContext)
                        };
                    }

                    return View("PointAuctionOptions", pointModel);

                default:
                    return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> PointAuctionOptions(PointAuctionModel model)
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ClassicAuctionOptions(ClassicAuctionModel model)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddPointSegment(PointAuctionModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                await AuctionService.AddPointSegment(model.Auction.Id, model.Title, model.Points, model.ColorHex);
                model.Auction = await AuctionService.GetPoint(model.Auction.Id);
            }
            else
            {
                await GuestService.AddPointSegment(model.Auction.Id, model.Title, model.Points, model.ColorHex, HttpContext);
                model.Auction = await GuestService.GetPoint(model.Auction.Id, HttpContext);
            }

            return RedirectToAction("Options", "Auction");
        }

        [HttpPost]
        public async Task<IActionResult> AddSegment(ClassicAuctionModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                await AuctionService.AddClassicSegment(model.Auction.Id, model.Title, model.ColorHex);
                model.Auction = await AuctionService.GetClassic(model.Auction.Id);
            }
            else
            {
                await GuestService.AddClassicSegment(model.Auction.Id, model.Title, model.ColorHex, HttpContext);
                model.Auction = await GuestService.GetClassic(model.Auction.Id, HttpContext);
            }

            return RedirectToAction("Options", "Auction");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSegment(Guid Id, AuctionType Type)
        {
            switch (Type)
            {
                case AuctionType.Classic:
                    if (User.Identity.IsAuthenticated)
                    {
                        await AuctionService.DeleteClassicSegment(Id);
                    }
                    else
                    {
                        await GuestService.DeleteClassicSegment(Id, HttpContext);
                    }
                    
                    break;
                case AuctionType.Point:
                    if (User.Identity.IsAuthenticated)
                    {
                        await AuctionService.DeletePointSegment(Id);
                    }
                    else
                    {
                        await GuestService.DeletePointSegment(Id, HttpContext);
                    }
                    
                    break;
                default:
                    throw new NotFoundException("Type was not found.");
            }

            return Redirect("Options");
        }

        public async Task<IActionResult> UpdatePointWheelOptions([FromBody] UpdatePointAuctionOptionsModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                await AuctionService.UpdatePointWheelOptions(model);
            }
            else
            {
                await GuestService.UpdatePointWheelOptions(model, HttpContext);
            }
            
            return RedirectToAction("Options", "Auction");
        }

        public async Task<IActionResult> UpdateClassicWheelOptions([FromBody] UpdateClassicAuctionOptionsModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                await AuctionService.UpdateClassicWheelOptions(model);
            }
            else
            {
                await GuestService.UpdateClassicWheelOptions(model, HttpContext);
            }

            return RedirectToAction("Options", "Auction");
        }

        public async Task<IActionResult> Load()
        {
            if (CurrentAuctionId.IsNullOrEmpty()
                || CurrentAuctionType.IsNullOrEmpty())
            {
                List<AuctionItem> wheels;

                if (User.Identity.IsAuthenticated)
                {
                    wheels = await AuctionService.GetAll(UserId);
                }
                else
                {
                    wheels = await GuestService.GetAll(HttpContext);
                }

                if (wheels != null && wheels.Count > 0)
                {
                    var wheel = wheels[0];

                    CurrentAuctionId = wheel.Id.ToString();
                    CurrentAuctionType = wheel.Type.ToString();
                }
                else
                {
                    return View("NoAuctionSelected");
                }
            }

            Enum.TryParse(CurrentAuctionType, out AuctionType type);
            Guid.TryParse(CurrentAuctionId, out Guid id);

            if (type == AuctionType.Classic) 
            {
                ClassicAuction wheel;

                if (User.Identity.IsAuthenticated)
                {
                    wheel = await AuctionService.GetClassic(id);
                }
                else
                {
                    wheel = await GuestService.GetClassic(id, HttpContext);
                }

                var model = new LoadClassicAuctionModel()
                {
                    WheelId = wheel.Id,
                    RemainingOptions = wheel.Segments
                };

                return View("LoadClassicAuction", model);
            }
            else if(type == AuctionType.Point)
            {
                PointAuction wheel;

                if (User.Identity.IsAuthenticated)
                {
                    wheel = await AuctionService.GetPoint(id);
                }
                else
                {
                    wheel = await GuestService.GetPoint(id, HttpContext);
                }

                var model = new LoadPointAuctionModel()
                {
                    WheelId = wheel.Id,
                    RemainingOptions = wheel.Segments
                };

                return View("LoadPointAuction", model);
            }
            else
            {
                throw new InvalidOperationException("Auction type is not correct.");
            }
        }

        public async Task<IActionResult> LoadClassicWheel(LoadClassicAuctionModel model)
        {
            return View();
        }

        public async Task<IActionResult> LoadPointWheel(LoadPointAuctionModel model)
        {
            return View();
        }
    }
}
