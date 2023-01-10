using Data.Entity;
using Data.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories.interfaces
{
    public interface IOrderRepository
    {
        Task<BaseResponse<int>> CreateOrder(Order order);
        Task<BaseResponse<List<Order>>> GetOrdersByUserId(int userid);
        Task<BaseResponse<List<Order>>> GetAllOrders();
        Task<BaseResponse<int>> SetPaid(int orderid);
        Task<BaseResponse<int>> DeleteOrder(int orderid);
        Task<BaseResponse<Order>> GetOrderById(int orderid);
        Task<BaseResponse<int>> ConfirmOrder(Order order);
        Task<BaseResponse<int>> DoOrder(int orderid);
    }
}
