using Data.Entity;
using Data.Repositories.interfaces;
using Data.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class OrderProductRepository : DbRepository<OrderProduct>, IOrderProductRepository
    {
        public OrderProductRepository(DapperDbContext dapper) 
            : base(dapper) { }

        public async Task<BaseResponse<int>> CreateOrderProduct(OrderProduct orderProduct)
        {
            var sql = "insert into public.orderproduct (orderid, productid, price, duration) values(@OrderId, @ProductId, @Price, @Duration) returning id";
            return await EditDataGetId(sql, orderProduct);
        }

        public async Task<BaseResponse<int>> DeleteOrderProductsByOrderId(int orderid)
        {
            var sql = "delete from public.orderproduct where orderid = @orderid";
            return await EditData(sql, new { orderid });
        }

        public async Task<BaseResponse<List<OrderProduct>>> GetProductsByOrderId(int orderid)
        {
            var sql = "select * from public.orderproduct where orderid=@orderid";
            return await GetAll(sql, new { orderid });
        }
    }
}
