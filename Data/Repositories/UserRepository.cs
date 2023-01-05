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

        public async Task<BaseResponse<int>> CreateUser(User user)
        {
            var sql = "insert into public.User(email, firstname, lastname, password, salt) values(@Email, @FirstName, @LastName, @Password, @Salt)";
            return await EditData(sql, user);
        }

        public async Task<BaseResponse<User>> GetUserByEmail(string email)
        {
            var sql = "select * from public.user where email=@email";
            return await GetAsync(sql, new { email });
        }

        public async Task<BaseResponse<User>> GetUserByEmailAndPassword(string email, string password)
        {
            var sql = "select * from public.user where email=@email and password=@password";
            return await GetAsync(sql, new { email, password });
        }
    }
}
