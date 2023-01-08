using Data.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using MVC.Services;
using MVC.Services.Interfaces;
using System.Security.Claims;

namespace MVC.Controllers
{
	public class PriceListController : Controller
	{
        private readonly ILogger<PriceListController> _logger;
        private readonly IPriceListService _priceListService;

		public PriceListController(IPriceListService priceListService, ILogger<PriceListController> logger)
		{
			_priceListService = priceListService;
			_logger = logger;
		}

        public async Task<IActionResult> Index()
		{
			var data = await _priceListService.GetPricelist();
			
			if(data.Success)
			{
                ViewData["Message"] = data.Message;
                return View(data.Data);
            }

            ViewData["Message"] = data.Message;
            return View();
		}

        [Authorize]
        public async Task<IActionResult> UserPriceList()
        {
            ClaimsPrincipal claimsPrincipal = HttpContext.User;
            var userid = Int32.Parse(claimsPrincipal.FindFirstValue("UserId"));

            var data = await _priceListService.GetUserPricelist(userid);

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
