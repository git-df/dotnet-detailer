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
    public class PromocodeRepository : DbRepository<Promocode>, IPromocodeRepository
    {
        public PromocodeRepository(DapperDbContext dapper) 
            : base(dapper) { }

        public async Task<BaseResponse<int>> Add(Promocode promocode)
        {
            var sql = "insert into public.promocode (productid, code, price) values(@ProductId, @Code, @Price)";
            return await EditData(sql, promocode);
        }

        public async Task<BaseResponse<int>> Delete(int promocodeid)
        {
            var sql = "delete from public.promocode where id = @promocodeid";
            return await EditData(sql, new { promocodeid });
        }

        public async Task<BaseResponse<List<Promocode>>> GetAllPromocodes()
        {
            var sql = "select * from promocode";
            return await GetAll(sql, new { });
        }

        public async Task<BaseResponse<List<Promocode>>> GetPromocodesByCode(string code)
        {
            var sql = "select * from promocode where code=@code";
            return await GetAll(sql, new { code });
        }
    }
}
