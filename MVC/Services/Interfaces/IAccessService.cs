using Data.Responses;
using MVC.Models;

namespace MVC.Services.Interfaces
{
    public interface IAccessService
    {
        Task<BaseResponse<UserInfoModel>> SignIn(UserLoginModel userLoginModel);
        Task<BaseResponse<int>> SignUp(UserRegisterModel userRegisterModel);
        Task<BaseResponse<UserInfoModel>> GetUserInfo(int userid);
        Task<BaseResponse<int>> PasswordChange(UserPasswordChangeModel userPasswordChange);
    }
}
