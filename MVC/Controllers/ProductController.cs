using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using MVC.Services.Interfaces;

namespace MVC.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IProductService _productService;

        public ProductController(ILogger<ProductController> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        [Authorize(Policy = "MustBeAdmin")]
        public async Task<IActionResult> GetProductList()
        {
            var data = await _productService.GetProducts();

            if (data.Success && data.Data != null)
            {
                return View(data.Data);
            }

            return RedirectToAction("Index", "User");
        }

        [Authorize(Policy = "MustBeAdmin")]
        public async Task<IActionResult> Active([FromRoute] int id)
        {
            var data = await _productService.Active(id);

            return RedirectToAction("GetProductList", "product");
        }

        [Authorize(Policy = "MustBeAdmin")]
        public async Task<IActionResult> DeActive([FromRoute] int id)
        {
            var data = await _productService.DeActive(id);

            return RedirectToAction("GetProductList", "product");
        }

        [Authorize(Policy = "MustBeAdmin")]
        public IActionResult AddProduct()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Policy = "MustBeAdmin")]
        public async Task<IActionResult> AddProduct(ProductAddModel product)
        {
            var data = await _productService.AddProduct(product);

            if (data.Success)
            {
                return RedirectToAction("GetProductList", "product");
            }

            ViewData["Message"] = data.Message;
            return View();
        }
    }
}
