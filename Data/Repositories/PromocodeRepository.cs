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

        public async Task<BaseResponse<List<Promocode>>> GetAllPromocodes()
        {
            var sql = "select * from promocode";
            return await GetAll(sql, new { });
        }
    }
}
