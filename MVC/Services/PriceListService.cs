using AutoMapper;
using Data.Repositories.interfaces;
using Data.Responses;
using MVC.Models;
using MVC.Services.Interfaces;

namespace MVC.Services
{
	public class PriceListService : IPriceListService
	{
		private readonly IMapper _mapper;
		private readonly IProductRepository _productRepository;
		private readonly ICategoryRepository _categoryRepository;

		public PriceListService(IMapper mapper, IProductRepository productRepository, ICategoryRepository categoryRepository)
		{
			_mapper = mapper;
			_productRepository = productRepository;
            _categoryRepository = categoryRepository;
		}

		public async Task<BaseResponse<List<CategoryWithProductsPriceListModel>>> GetPricelist()
		{
			var categories = await _categoryRepository.GetAllCategories();
			var products = await _productRepository.GetAllProducts();

			if (categories.Success && products.Success)
			{
				if (categories.Data != null && products.Data != null)
				{
					var data = new List<CategoryWithProductsPriceListModel>();

                    foreach (var category in categories.Data.Where(c => c.IsActive))
                    {
						data.Add(new CategoryWithProductsPriceListModel()
						{
							Name = category.Name,
							Products = _mapper.Map<List<ProductInPriceListModel>>(products.Data.Where(p => p.CategoryId == category.Id && p.IsActive == true ))
						});
                    }

                    return new BaseResponse<List<CategoryWithProductsPriceListModel>>() { Data = data };
                }
				else
				{
                    return new BaseResponse<List<CategoryWithProductsPriceListModel>>()
                    {
                        Success = false,
                        Message = "Obecnie brak produktów"
                    };
                }
			}

			return new BaseResponse<List<CategoryWithProductsPriceListModel>>()
			{
				Success = false,
				Message = "Problem z systemem, prosimy spróbowac za jakiś czas"
            };
		}

		public async Task<BaseResponse<List<CategoryWithProductsPriceListModel>>> GetUserPricelist(int userid)
		{
            var categories = await _categoryRepository.GetAllCategories();
            var products = await _productRepository.GetUserProducts(userid);

            if (categories.Success && products.Success)
            {
                if (categories.Data != null && products.Data != null)
                {
                    var data = new List<CategoryWithProductsPriceListModel>();

                    foreach (var category in categories.Data)
                    {
                        data.Add(new CategoryWithProductsPriceListModel()
                        {
                            Name = category.Name,
                            Products = _mapper.Map<List<ProductInPriceListModel>>(products.Data.Where(p => p.CategoryId == category.Id && p.IsActive == true))
                        });
                    }

                    return new BaseResponse<List<CategoryWithProductsPriceListModel>>() { Data = data };
                }
                else
                {
                    return new BaseResponse<List<CategoryWithProductsPriceListModel>>()
                    {
                        Success = false,
                        Message = "Obecnie brak produktów"
                    };
                }
            }

            return new BaseResponse<List<CategoryWithProductsPriceListModel>>()
            {
                Success = false,
                Message = "Problem z systemem, prosimy spróbowac za jakiś czas"
            };
        }
	}
}
