using Data.Responses;
using MVC.Models;

namespace MVC.Services.Interfaces
{
    public interface IProductService
    {
        Task<BaseResponse<List<ProductListModel>>> GetProducts();
    }
}
