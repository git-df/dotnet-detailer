﻿using Microsoft.AspNetCore.Mvc;
using MVC.Services;
using MVC.Services.Interfaces;

namespace MVC.Controllers
{
    public class PromocodeController : Controller
    {
        private readonly IPromocodeService _promocodeService;
        private readonly ILogger<PromocodeController> _logger;

        public PromocodeController(IPromocodeService promocodeService, ILogger<PromocodeController> logger)
        {
            _logger = logger;
            _promocodeService = promocodeService;
        }

        public async Task<IActionResult> Index()
        {
            var data = await _promocodeService.GetPromocodelist();

            if (data.Success)
            {
                ViewData["Message"] = data.Message;
                return View(data.Data);
            }

            ViewData["Message"] = data.Message;
            return View();
        }
    }
}
