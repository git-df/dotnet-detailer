using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.Services.Interfaces;

namespace MVC.Controllers
{
	public class PriceListController : Controller
	{
        private readonly ILogger<HomeController> _logger;
        private readonly IPriceListService _priceListService;

		public PriceListController(IPriceListService priceListService, ILogger<HomeController> logger)
		{
			_priceListService = priceListService;
			_logger = logger;
		}

        [Authorize(Policy = "MustBeAdmin")]
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
	}
}
