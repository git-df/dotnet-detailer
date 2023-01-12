using AutoMapper;
using Data.Entity;
using Data.Repositories.interfaces;
using Data.Responses;
using MVC.Models;
using MVC.Services.Interfaces;

namespace MVC.Services
{
    public class ProductService : IProductService
    {
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ProductService(IMapper mapper, IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _mapper = mapper;
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<BaseResponse<int>> Active(int productid)
        {
            var data = await _productRepository.ActiveProduct(productid);

            if (data.Success && data.Data != 0)
            {
                return new BaseResponse<int>() { Data = data.Data };
            }

            return new BaseResponse<int>()
            {
                Success = false,
                Message = "Problem z systemem, prosimy spróbowac za jakiś czas"
            };
        }

        public async Task<BaseResponse<int>> AddProduct(ProductAddModel product)
        {
            var category = await _categoryRepository.GetAllCategories();

            if (category.Success && category.Data != null)
            {
                if (category.Data.SingleOrDefault(c => c.Id == product.CategoryId) != null)
                {
                    var data = await _productRepository.CreateProduct(_mapper.Map<Product>(product));

                    if (data.Success && data.Data != 0)
                    {
                        return new BaseResponse<int>() { Data = data.Data };
                    }
                }
                else
                {
                    return new BaseResponse<int>()
                    {
                        Success = false,
                        Message = "Błędne id kategori"
                    };
                }
            }

            return new BaseResponse<int>()
            {
                Success = false,
                Message = "Problem z systemem, prosimy spróbowac za jakiś czas"
            };
        }

        public async Task<BaseResponse<int>> DeActive(int productid)
        {
            var data = await _productRepository.DeActiveProduct(productid);

            if (data.Success && data.Data != 0)
            {
                return new BaseResponse<int>() { Data = data.Data };
            }

            return new BaseResponse<int>()
            {
                Success = false,
                Message = "Problem z systemem, prosimy spróbowac za jakiś czas"
            };
        }

        public async Task<BaseResponse<List<ProductListModel>>> GetProducts()
        {
            var data = await _productRepository.GetAllProducts();

            if (data.Success && data.Data != null)
            {
                return new BaseResponse<List<ProductListModel>>() { Data = _mapper.Map<List<ProductListModel>>(data.Data.OrderBy(p => p.Id)) };
            }

            return new BaseResponse<List<ProductListModel>>
            {
                Success = false,
                Message = "Problem z systemem, prosimy spróbowac za jakiś czas"
            };
        }
    }
}
