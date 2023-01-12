using Data.Responses;
using MVC.Models;

namespace MVC.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<BaseResponse<List<CategoryListModel>>> GetCategoryList();
        Task<BaseResponse<int>> AddCategory(CategoryAddModel category);
    }
}
