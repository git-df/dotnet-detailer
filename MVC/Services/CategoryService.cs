using AutoMapper;
using Data.Entity;
using Data.Repositories;
using Data.Repositories.interfaces;
using Data.Responses;
using Microsoft.CodeAnalysis;
using MVC.Models;
using MVC.Services.Interfaces;

namespace MVC.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(IMapper mapper, ICategoryRepository categoryRepository)
        {
            _mapper = mapper;
            _categoryRepository = categoryRepository;
        }

        public async Task<BaseResponse<int>> Active(int categoryid)
        {
            var data = await _categoryRepository.ActiveCategory(categoryid);

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

        public async Task<BaseResponse<int>> AddCategory(CategoryAddModel category)
        {
            var data = await _categoryRepository.CreateCategory(_mapper.Map<Category>(category));

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

        public async Task<BaseResponse<int>> DeActive(int categoryid)
        {
            var data = await _categoryRepository.DeActiveCategory(categoryid);

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

        public async Task<BaseResponse<List<CategoryListModel>>> GetCategoryList()
        {
            var data = await _categoryRepository.GetAllCategories();

            if (data.Success && data.Data != null)
            {
                return new BaseResponse<List<CategoryListModel>>() 
                { 
                    Data = _mapper.Map<List<CategoryListModel>>(data.Data.OrderBy(c => c.Id))
                };
            }

            return new BaseResponse<List<CategoryListModel>>
            {
                Success = false,
                Message = "Problem z systemem, prosimy spróbowac za jakiś czas"
            };
        }
    }
}
