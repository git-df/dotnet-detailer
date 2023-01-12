using AutoMapper;
using Data.Entity;
using Data.Repositories.interfaces;
using Data.Responses;
using MVC.Models;
using MVC.Services.Interfaces;
using static NuGet.Packaging.PackagingConstants;

namespace MVC.Services
{
    public class OrderService : IOrderService
    {
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IPromocodeRepository _promocodeRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderProductRepository _orderProductRepository;

        public OrderService(IMapper mapper, IProductRepository productRepository, ICategoryRepository categoryRepository, IPromocodeRepository promocodeRepository, IOrderRepository orderRepository, IOrderProductRepository orderProductRepository)
        {
            _mapper = mapper;
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _promocodeRepository = promocodeRepository;
            _orderRepository = orderRepository;
            _orderProductRepository = orderProductRepository;
        }

        public async Task<BaseResponse<int>> Delete(int orderid)
        {
            var data = await _orderProductRepository.DeleteOrderProductsByOrderId(orderid);
            if (data.Success)
            {
                await _orderRepository.DeleteOrder(orderid);
            }
            return data;
        }

        public async Task<BaseResponse<List<OrderInListModel>>> GetMyOrders(int userid)
        {
            var orders = await _orderRepository.GetOrdersByUserId(userid);
            var products = await _productRepository.GetAllProducts();

            if (orders.Success && orders.Data != null && products.Success && products.Data != null)
            {
                var ordersModel = _mapper.Map<List<OrderInListModel>>(orders.Data);

                foreach (var orderModel in ordersModel)
                {
                    var orderProducts = await _orderProductRepository.GetProductsByOrderId(orderModel.Id);

                    if (orderProducts.Success && orderProducts.Data != null)
                    {
                        orderModel.orderProducts = _mapper.Map<List<OrderProductInListModel>>(orderProducts.Data);

                        foreach (var product in orderModel.orderProducts)
                        {
                            product.Name = products.Data.SingleOrDefault(p => p.Id == product.ProductId).Name;
                        }
                    }
                }

                var ordersModelWithProducts = new List<OrderInListModel>();
                ordersModelWithProducts.AddRange(ordersModel.Where(o => o.orderProducts.Count() > 0).OrderBy(o => o.Id).Reverse());

                return new BaseResponse<List<OrderInListModel>>() { Data = ordersModelWithProducts };
            }

            return new BaseResponse<List<OrderInListModel>>()
            {
                Success = false,
                Message = "Problem z systemem, prosimy spróbowac za jakiś czas"
            };
        }

        public async Task<BaseResponse<List<OrderInListModel>>> GetOrdersToDo()
        {
            var orders = await _orderRepository.GetAllOrders();
            var products = await _productRepository.GetAllProducts();

            if (orders.Success && orders.Data != null && products.Success && products.Data != null)
            {
                var ordersToDo = _mapper.Map<List<OrderInListModel>>(orders.Data)
                    .Where(o => o.IsConfirmed && !o.IsDone)
                    .OrderBy(o => o.DateStart);

                foreach (var orderToDo in ordersToDo)
                {
                    var orderProducts = await _orderProductRepository.GetProductsByOrderId(orderToDo.Id);

                    if (orderProducts.Success && orderProducts.Data != null)
                    {
                        orderToDo.orderProducts = _mapper.Map<List<OrderProductInListModel>>(orderProducts.Data);

                        foreach (var product in orderToDo.orderProducts)
                        {
                            product.Name = products.Data.SingleOrDefault(p => p.Id == product.ProductId).Name;
                        }
                    }
                }


                var response = new List<OrderInListModel>();
                response.AddRange(ordersToDo.Where(o => o.orderProducts.Count() > 0));

                return new BaseResponse<List<OrderInListModel>>() { Data = response };
            }

            return new BaseResponse<List<OrderInListModel>>()
            {
                Success = false,
                Message = "Problem z systemem, prosimy spróbowac za jakiś czas"
            };
        }

        public async Task<BaseResponse<List<OrderInListModel>>> GetOrdersToConfirm()
        {
            var orders = await _orderRepository.GetAllOrders();
            var products = await _productRepository.GetAllProducts();

            if (orders.Success && orders.Data != null && products.Success && products.Data != null)
            {
                var ordersToConfirm = _mapper.Map<List<OrderInListModel>>(orders.Data)
                    .Where(o => !o.IsConfirmed)
                    .OrderBy(o => o.Id);

                foreach (var orderToConfirm in ordersToConfirm)
                {
                    var orderProducts = await _orderProductRepository.GetProductsByOrderId(orderToConfirm.Id);

                    if (orderProducts.Success && orderProducts.Data != null)
                    {
                        orderToConfirm.orderProducts = _mapper.Map<List<OrderProductInListModel>>(orderProducts.Data);

                        foreach (var product in orderToConfirm.orderProducts)
                        {
                            product.Name = products.Data.SingleOrDefault(p => p.Id == product.ProductId).Name;
                        }
                    }
                }


                var response = new List<OrderInListModel>();
                response.AddRange(ordersToConfirm.Where(o => o.orderProducts.Count() > 0));

                return new BaseResponse<List<OrderInListModel>>() { Data = response };
            }

            return new BaseResponse<List<OrderInListModel>>()
            {
                Success = false,
                Message = "Problem z systemem, prosimy spróbowac za jakiś czas"
            };
        }

        public async Task<BaseResponse<OrderProductWithCategoryListModel>> GetProductsToOrder()
        {
            var categories = await _categoryRepository.GetAllCategories();
            var products = await _productRepository.GetAllProducts();

            if (categories.Success && products.Success && products.Data != null && categories.Data != null)
            {
                OrderProductWithCategoryListModel data = new OrderProductWithCategoryListModel() { CategoryWithProducts = new List<CategoryWithProductsOrderModel>()};

                foreach (var category in categories.Data.Where(c => c.IsActive))
                {

                    var a = _mapper.Map<List<ProductInOrderModel>>(products.Data.Where(p => p.CategoryId == category.Id && p.IsActive == true));

                    data.CategoryWithProducts.Add(new CategoryWithProductsOrderModel() 
                    {
                        Name = category.Name,
                        Products = _mapper.Map<List<ProductInOrderModel>>(products.Data.Where(p => p.CategoryId == category.Id && p.IsActive == true))
                    });
                }

                return new BaseResponse<OrderProductWithCategoryListModel>() { Data = data };
            }


            return new BaseResponse<OrderProductWithCategoryListModel>()
            {
                Success = false,
                Message = "Problem z systemem, prosimy spróbowac za jakiś czas"
            };
        }

        public async Task<BaseResponse<int>> Pay(int orderid)
        {
            return await _orderRepository.SetPaid(orderid);
        }

        public async Task<BaseResponse<int>> SendOrder(OrderSummaryModel orderModel)
        {
            var order = _mapper.Map<Order>(orderModel);

            var data = await _orderRepository.CreateOrder(order);

            if (data.Success && data.Data != 0)
            {
                var orderproducts = _mapper.Map<List<OrderProduct>>(orderModel.OrderProducts);

                foreach (var orderproduct in orderproducts)
                {
                    orderproduct.OrderId = data.Data;
                    await _orderProductRepository.CreateOrderProduct(orderproduct);
                }

                return new BaseResponse<int>() { Data = data.Data };
            }

            return new BaseResponse<int>()
            {
                Success = false,
                Message = "Problem z systemem, prosimy spróbowac za jakiś czas"
            };
        }

        public async Task<BaseResponse<List<OrderProductModel>>> UseOfferUser(List<OrderProductModel> products, int userid)
        {
            var productsInOrder = products;
            var userProducts = await _productRepository.GetUserProducts(userid);
            decimal diff = 0;

            if (userProducts.Success && userProducts.Data != null)
            {
                foreach (var productInOrder in productsInOrder)
                {
                    var userProduct = userProducts.Data.FirstOrDefault(p => p.Id == productInOrder.ProductId);

                    if (userProduct != null && userProduct.Price < productInOrder.Price)
                    {
                        diff = diff + (productInOrder.Price - userProduct.Price);
                        productInOrder.Price = userProduct.Price;
                    }
                }
            }

            return new BaseResponse<List<OrderProductModel>>() { Data = productsInOrder, Message = diff.ToString() };
        }

        public async Task<BaseResponse<List<OrderProductModel>>> UsePromocode(List<ProductInOrderModel> products, string code)
        {
            var orderproducts = _mapper.Map<List<OrderProductModel>>(products);
            decimal diff = 0;

            if (code != null)
            {
                var promocodes = await _promocodeRepository.GetPromocodesByCode(code);

                if (promocodes.Success && promocodes.Data != null)
                {
                    foreach (var productInOrder in orderproducts)
                    {
                        var promocode = promocodes.Data.FirstOrDefault(p => p.ProductId == productInOrder.ProductId);

                        if (promocode != null && promocode.Price < productInOrder.Price)
                        {
                            diff = diff + (productInOrder.Price - promocode.Price);
                            productInOrder.Price = promocode.Price;
                        }
                    }
                }
            }

            return new BaseResponse<List<OrderProductModel>>() { Data = orderproducts, Message = diff.ToString() };
        }

        public async Task<BaseResponse<List<OrderInListModel>>> GetDoneOrdersToPay()
        {
            var orders = await _orderRepository.GetAllOrders();
            var products = await _productRepository.GetAllProducts();

            if (orders.Success && orders.Data != null && products.Success && products.Data != null)
            {
                var ordersNoPaid = _mapper.Map<List<OrderInListModel>>(orders.Data)
                    .Where(o => !o.IsPaid && o.IsDone)
                    .OrderBy(o => o.DateEnd);

                foreach (var orderNoPaid in ordersNoPaid)
                {
                    var orderProducts = await _orderProductRepository.GetProductsByOrderId(orderNoPaid.Id);

                    if (orderProducts.Success && orderProducts.Data != null)
                    {
                        orderNoPaid.orderProducts = _mapper.Map<List<OrderProductInListModel>>(orderProducts.Data);

                        foreach (var product in orderNoPaid.orderProducts)
                        {
                            product.Name = products.Data.SingleOrDefault(p => p.Id == product.ProductId).Name;
                        }
                    }
                }


                var response = new List<OrderInListModel>();
                response.AddRange(ordersNoPaid.Where(o => o.orderProducts.Count() > 0));

                return new BaseResponse<List<OrderInListModel>>() { Data = response };
            }

            return new BaseResponse<List<OrderInListModel>>()
            {
                Success = false,
                Message = "Problem z systemem, prosimy spróbowac za jakiś czas"
            };
        }

        public async Task<BaseResponse<OrderInListModel>> GetOrder(int orderid)
        {
            var order = await _orderRepository.GetOrderById(orderid);
            var products = await _productRepository.GetAllProducts();

            if (order.Success && order.Data != null && products.Success && products.Data != null)
            {
                var orderInListModel = _mapper.Map<OrderInListModel>(order.Data);
                var orderProducts = await _orderProductRepository.GetProductsByOrderId(orderid);

                if (orderProducts.Success && orderProducts.Data != null)
                {
                    orderInListModel.orderProducts = _mapper.Map<List<OrderProductInListModel>>(orderProducts.Data);

                    foreach (var product in orderInListModel.orderProducts)
                    {
                        product.Name = products.Data.SingleOrDefault(p => p.Id == product.ProductId).Name;
                    }
                }

                return new BaseResponse<OrderInListModel>() { Data = orderInListModel };
            }

            return new BaseResponse<OrderInListModel>()
            {
                Success = false,
                Message = "Problem z systemem, prosimy spróbowac za jakiś czas"
            };
        }

        public async Task<BaseResponse<int>> Confirm(OrderInListModel ordermodel)
        {
            var order = _mapper.Map<Order>(ordermodel);
            var data = await _orderRepository.ConfirmOrder(order);

            if (data.Success && data.Data != 0)
            {
                return data;
            }

            return new BaseResponse<int>()
            {
                Success = false,
                Message = "Problem z systemem, prosimy spróbowac za jakiś czas"
            };
        }

        public async Task<BaseResponse<int>> Do(int orderid)
        {
            return await _orderRepository.DoOrder(orderid);
        }
    }
}
