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
        Task<BaseResponse<int>> SendOrder(OrderSummaryModel orderModel);
        Task<BaseResponse<List<OrderInListModel>>> GetMyOrders(int userid);
        Task<BaseResponse<List<OrderInListModel>>> GetOrdersToConfirm();
        Task<BaseResponse<List<OrderInListModel>>> GetOrdersToDo();
        Task<BaseResponse<List<OrderInListModel>>> GetDoneOrdersToPay();
        Task<BaseResponse<int>> Pay(int orderid);
        Task<BaseResponse<int>> Delete(int orderid);
    }
}
