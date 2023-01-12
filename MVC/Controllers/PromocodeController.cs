using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;
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

        [Authorize(Policy = "MustBeAdmin")]
        public async Task<IActionResult> GetListForAdmin()
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

        [Authorize(Policy = "MustBeAdmin")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await _promocodeService.Delete(id);

            return RedirectToAction("GetListForAdmin", "Promocode");
        }

        [Authorize(Policy = "MustBeAdmin")]
        public async Task<IActionResult> Add()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Policy = "MustBeAdmin")]
        public async Task<IActionResult> Add(PromocodeAddModel promocode)
        {
            var data = await _promocodeService.Add(promocode);

            if (data.Success)
            {
                return RedirectToAction("GetListForAdmin", "Promocode");
            }

            ViewData["Message"] = data.Message;
            return View();
        }
    }
}
