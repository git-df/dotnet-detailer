using Data.Responses;
using MVC.Models;

namespace MVC.Services.Interfaces
{
    public interface IProductService
    {
        Task<BaseResponse<List<ProductListModel>>> GetProducts();
        Task<BaseResponse<int>> Active(int productid);
        Task<BaseResponse<int>> DeActive(int productid);
        Task<BaseResponse<int>> AddProduct(ProductAddModel product);
    }
}
