using Data.Responses;
using MVC.Models;

namespace MVC.Services.Interfaces
{
    public interface IUserService
    {
        Task<BaseResponse<int>> EditUser(UserEditModel userEdit);
    }
}
