using Data.Entity;
using Data.Responses;
using MVC.Models;

namespace MVC.Services.Interfaces
{
    public interface IOrderService
    {
        Task<BaseResponse<OrderProductWithCategoryListModel>> GetProductsToOrder();
        Task<BaseResponse<List<OrderProductModel>>> UsePromocode(List<ProductInOrderModel> products, string code);
        Task<BaseResponse<List<OrderProductModel>>> UseOfferUser(List<OrderProductModel> products, int userid);
    }
}
