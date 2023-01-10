using Data.Entity;
using Data.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories.interfaces
{
    public interface IOrderProductRepository
    {
        Task<BaseResponse<int>> CreateOrderProduct(OrderProduct orderProduct);
        Task<BaseResponse<List<OrderProduct>>> GetProductsByOrderId(int orderid);
        Task<BaseResponse<int>> DeleteOrderProductsByOrderId(int orderid);
    }
}
