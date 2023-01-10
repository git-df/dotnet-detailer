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
    public class OrderRepository : DbRepository<Order>, IOrderRepository
    {
        public OrderRepository(DapperDbContext dapper) 
            : base(dapper) { }

        public async Task<BaseResponse<int>> ConfirmOrder(Order order)
        {
            var sql = "update public.order set isconfirmed = true, datestart = @DateStart, dateend = @DateEnd where id = @Id";
            return await EditData(sql, order);
        }

        public async Task<BaseResponse<int>> CreateOrder(Order order)
        {
            var sql = "insert  into public.order (userid, datestart, dateend, price, isconfirmed, ispaid, isdone) values(@UserId, @DateStart, @DateEnd, @Price, @IsConfirmed, @IsPaid, @IsDone) returning id";
            return await EditDataGetId(sql, order);
        }

        public async Task<BaseResponse<int>> DeleteOrder(int orderid)
        {
            var sql = "delete from public.order where id = @orderid";
            return await EditData(sql, new { orderid });
        }

        public async Task<BaseResponse<int>> DoOrder(int orderid)
        {
            var sql = "update public.order set isdone = true where id = @orderid";
            return await EditData(sql, new { orderid });
        }

        public async Task<BaseResponse<List<Order>>> GetAllOrders()
        {
            var sql = "select * from public.order";
            return await GetAll(sql, new { });
        }

        public async Task<BaseResponse<Order>> GetOrderById(int orderid)
        {
            var sql = "select * from public.order where id=@orderid";
            return await GetAsync(sql, new { orderid });
        }

        public async Task<BaseResponse<List<Order>>> GetOrdersByUserId(int userid)
        {
            var sql = "select * from public.order where userid=@userid";
            return await GetAll(sql, new { userid });
        }

        public async Task<BaseResponse<int>> SetPaid(int orderid)
        {
            var sql = "update public.order set ispaid = true where id = @orderid";
            return await EditData(sql, new { orderid });
        }
    }
}
