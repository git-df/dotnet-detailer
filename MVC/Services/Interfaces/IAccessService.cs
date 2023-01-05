using Data.Responses;
using MVC.Models;

namespace MVC.Services.Interfaces
{
    public interface IAccessService
    {
        Task<BaseResponse<UserInfoModel>> LogIn(UserLoginModel userLoginModel);
        Task<BaseResponse<int>> Register(UserRegisterModel userRegisterModel);
    }
}
