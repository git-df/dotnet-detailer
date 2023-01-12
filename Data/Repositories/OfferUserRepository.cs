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
    public class OfferUserRepository : DbRepository<OfferUser>, IOfferUserRepository
    {
        public OfferUserRepository(DapperDbContext dapper) 
            : base(dapper) { }

        public async Task<BaseResponse<int>> CreateOferUser(OfferUser offerUser)
        {
            var sql = "insert into public.offeruser (userid, productid, price) values(@UserId ,@ProductId ,@Price)";
            return await EditData(sql, offerUser);
        }

        public async Task<BaseResponse<int>> DeleteOferUser(int offerid)
        {
            var sql = "delete from public.offeruser where id = @offerid";
            return await EditData(sql, new { offerid });
        }

        public async Task<BaseResponse<List<OfferUser>>> GetOfferUserList()
        {
            var sql = "select * from public.offeruser";
            return await GetAll(sql, new { });
        }
    }
}
