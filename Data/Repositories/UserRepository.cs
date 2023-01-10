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

        public async Task<BaseResponse<List<User>>> GetAllUsers()
        {
            var sql = "select * from public.user";
            return await GetAll(sql, new { });
        }

        public async Task<BaseResponse<User>> GetUserByEmail(string email)
        {
            var sql = "select * from public.user where email=@email";
            return await GetAsync(sql, new { email });
        }

        public async Task<BaseResponse<User>> GetUserById(int userid)
        {
            var sql = "select * from public.user where id=@userid";
            return await GetAsync(sql, new { userid });
        }

        public async Task<BaseResponse<int>> PasswordChange(User user)
        {
            var sql = "update public.user set password=@Password, salt=@Salt where id=@Id ";
            return await EditData(sql, user);
        }

        public async Task<BaseResponse<int>> UpdateNameUser(User user)
        {

            var sql = "update public.user set firstname=@FirstName, lastname=@LastName where id=@Id ";
            return await EditData(sql, user);
        }
    }
}
