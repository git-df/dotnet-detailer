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
        Task<BaseResponse<User>> GetUserByEmailAndPassword(string email, string password);
        Task<BaseResponse<User>> GetUserByEmail(string email);
        Task<BaseResponse<int>> CreateUser(User user);
    }
}
