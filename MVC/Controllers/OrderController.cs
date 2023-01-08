using Data.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using MVC.Services;
using MVC.Services.Interfaces;
using System.Security.Claims;

namespace MVC.Controllers
{
    public class OrderController : Controller
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IOrderService _orderService;

        public OrderController(ILogger<OrderController> logger, IOrderService orderService)
        {
            _logger = logger;
            _orderService = orderService;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var data = await _orderService.GetProductsToOrder();

            if (data.Success)
            {
                return View(data.Data);
            }

            ViewData["Message"] = data.Message;
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Recalculate(OrderProductWithCategoryListModel data)
        {
            var userid = Int32.Parse(User.FindFirstValue("UserId"));

            List<ProductInOrderModel> products = new List<ProductInOrderModel>();
            decimal diff = 0;
            decimal price = 0;
            int duration = 0;

            if (data.CategoryWithProducts != null) 
                foreach (var categoryWithProducts in data.CategoryWithProducts)
                {
                    if (categoryWithProducts.Products != null)
                        foreach (var product in categoryWithProducts.Products)
                        {
                            if (product.InOrder)
                            {
                                products.Add(product);
                            }
                        }
                }

            if (products.Count() > 0)
            {
                var productWithUseCode = await _orderService.UsePromocode(products, data.Code);

                diff = diff + Decimal.Parse(productWithUseCode.Message);

                if (productWithUseCode.Success && productWithUseCode.Data != null)
                {
                    productWithUseCode = await _orderService.UseOfferUser(productWithUseCode.Data, userid);

                    if (productWithUseCode.Success && productWithUseCode.Data != null)
                    {
                        diff = diff + Decimal.Parse(productWithUseCode.Message);

                        foreach (var product in productWithUseCode.Data)
                        {
                            price = price + product.Price;
                            duration = duration + product.Duration;
                        }

                        OrderSummaryModel order = new OrderSummaryModel()
                        {
                            OrderProducts = productWithUseCode.Data,
                            Price = price,
                            UserId = userid,
                            Duration = duration
                        };

                        ViewData["Message"] = diff;
                        return View(order);
                    }
                }
            }

            return RedirectToAction("Index");
        }
    }
}
