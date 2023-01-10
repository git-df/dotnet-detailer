using Data.Entity;
using Data.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories.interfaces
{
    public interface IUserRepository
    {
        Task<BaseResponse<User>> GetUserByEmail(string email);
        Task<BaseResponse<User>> GetUserById(int userid);
        Task<BaseResponse<List<User>>> GetAllUsers();
        Task<BaseResponse<int>> CreateUser(User user);
        Task<BaseResponse<int>> UpdateNameUser(User user);
        Task<BaseResponse<int>> PasswordChange(User user);
    }
}
