using AutoMapper;
using Data.Entity;
using Data.Repositories.interfaces;
using Data.Responses;
using MVC.Models;
using MVC.Services.Interfaces;

namespace MVC.Services
{
    public class OrderService : IOrderService
    {
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IPromocodeRepository _promocodeRepository;

        public OrderService(IMapper mapper, IProductRepository productRepository, ICategoryRepository categoryRepository, IPromocodeRepository promocodeRepository)
        {
            _mapper = mapper;
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _promocodeRepository = promocodeRepository;
        }

        public async Task<BaseResponse<OrderProductWithCategoryListModel>> GetProductsToOrder()
        {
            var categories = await _categoryRepository.GetAllCategories();
            var products = await _productRepository.GetAllProducts();

            if (categories.Success && products.Success && products.Data != null && categories.Data != null)
            {
                OrderProductWithCategoryListModel data = new OrderProductWithCategoryListModel() { CategoryWithProducts = new List<CategoryWithProductsOrderModel>()};

                foreach (var category in categories.Data)
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
    }
}
