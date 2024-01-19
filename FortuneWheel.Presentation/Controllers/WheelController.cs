﻿using FortuneWheel.Application.Services.Auth;
using FortuneWheel.Presentation.Models.Auth;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using FortuneWheel.Services;
using FortuneWheel.Models.Wheels;

namespace FortuneWheel.Controllers
{
    public class WheelController : Controller
    {
        private readonly IWheelService WheelService;

        public WheelController(IWheelService wheelService)
        {
            WheelService = wheelService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWheels()
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var model = new GetAllWheelsModel
            {
                Wheels = (List<Domain.WheelsOfFortune.WheelOfFortune>)await WheelService.GetUserWheels(userId)
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
