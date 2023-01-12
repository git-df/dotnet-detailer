using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using MVC.Services.Interfaces;

namespace MVC.Controllers
{
    public class OfferuserController : Controller
    {
        private readonly ILogger<OfferuserController> _logger;
        private readonly IOfferUserService _offerUserService;

        public OfferuserController(ILogger<OfferuserController> logger, IOfferUserService offerUserService)
        {
            _logger = logger;
            _offerUserService = offerUserService;
        }

        [Authorize(Policy = "MustBeAdmin")]
        public async Task<IActionResult> Index()
        {
            var data = await _offerUserService.GetOfferList();

            if (data.Success && data.Data != null)
            {
                return View(data.Data);
            }

            return RedirectToAction("Index", "User");
        }

        [Authorize(Policy = "MustBeAdmin")]
        public async Task<IActionResult> DeleteOffer([FromRoute] int id)
        {
            await _offerUserService.DeleteOffer(id);

            return RedirectToAction("Index", "OfferUser");
        }

        [Authorize(Policy = "MustBeAdmin")]
        public async Task<IActionResult> AddOffer()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Policy = "MustBeAdmin")]
        public async Task<IActionResult> AddOffer(OfferUserAddModel offerUser)
        {
            var data = await _offerUserService.AddOfferUser(offerUser);

            if (data.Success)
            {
                return RedirectToAction("Index", "OfferUser");
            }

            ViewData["Message"] = data.Message;
            return View();
        }
    }
}
