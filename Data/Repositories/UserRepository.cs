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
    public class UserRepository : DbRepository<User>, IUserRepository
    {
        public UserRepository(DapperDbContext dapper) 
            : base(dapper) { }

        public Task<BaseResponse<User>> GetUserByEmailAndPassword(string email, string password)
        {
            var sql = "select * from public.user where email=@email and password=@password";
            return GetAsync(sql, new { email, password });
        }
    }
}
