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

            ViewData["Message"] = "Musisz wybrać produkty";
            return RedirectToAction("Index");
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SendOrder(OrderSummaryModel order)
        {
            var data = await _orderService.SendOrder(order);

            if (data.Success)
            {
                return View();
            }

            return RedirectToAction("Index");
        }

        [Authorize]
        public async Task<IActionResult> MyOrders()
        {
            var userid = Int32.Parse(User.FindFirstValue("UserId"));
            var data = await _orderService.GetMyOrders(userid);

            if (data.Success && data.Data != null)
            {
                return View(data.Data);
            }

            return RedirectToAction("Index", "User");
        }

        [Authorize]
        public async Task<IActionResult> Pay([FromRoute]int id)
        {
            var data = await _orderService.Pay(id);

            if (data.Success)
            {
                return View();
            }

            return RedirectToAction("MyOrders", "Order");
        }

        [Authorize]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var data = await _orderService.Delete(id);

            return RedirectToAction("MyOrders", "Order");
        }

        [Authorize(Policy = "MustBeEmployee")]
        public async Task<IActionResult> OrdersToConfirm()
        {
            var data = await _orderService.GetOrdersToConfirm();

            if (data.Success && data.Data != null)
            {
                return View(data.Data);
            }

            return RedirectToAction("Index", "User");
        }

        [Authorize(Policy = "MustBeEmployee")]
        public async Task<IActionResult> OrdersToDo()
        {
            var data = await _orderService.GetOrdersToDo();

            if (data.Success && data.Data != null)
            {
                return View(data.Data);
            }

            return RedirectToAction("Index", "User");
        }

        [Authorize(Policy = "MustBeEmployee")]
        public async Task<IActionResult> OrdersNoPaid()
        {
            var data = await _orderService.GetDoneOrdersToPay();

            if (data.Success && data.Data != null)
            {
                return View(data.Data);
            }

            return RedirectToAction("Index", "User");
        }

        [Authorize(Policy = "MustBeEmployee")]
        public async Task<IActionResult> Confirm([FromRoute] int id)
        {
            var data = await _orderService.GetOrder(id);

            if (data.Success && data.Data != null)
            {
                return View(data.Data);
            }

            return RedirectToAction("OrdersToConfirm", "Order");
        }

        [Authorize(Policy = "MustBeEmployee")]
        [HttpPost]
        public async Task<IActionResult> Confirm(OrderInListModel order)
        {
            if (!ModelState.IsValid) return RedirectToAction("Confirm", null, order.Id, null);

            var data = await _orderService.Confirm(order);

            if (data.Success)
            {
                return RedirectToAction("OrdersToConfirm");
            }

            return RedirectToAction("Confirm", null, order.Id, null);
        }

        [Authorize(Policy = "MustBeEmployee")]
        public async Task<IActionResult> Do([FromRoute] int id)
        {
            var data = await _orderService.Do(id);

            return RedirectToAction("OrdersToDo", "Order");
        }
    }
}
