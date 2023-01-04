using AutoMapper;
using Data.Repositories.interfaces;
using Data.Responses;
using MVC.Models;
using MVC.Services.Interfaces;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;


namespace MVC.Services
{
    public class AccessService : IAccessService
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public AccessService(IMapper mapper, IUserRepository userRepository)
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task<BaseResponse<UserInfoModel>> LogIn(UserLoginModel userLoginModel)
        {
            var data = await _userRepository.GetUserByEmailAndPassword(userLoginModel.Email, userLoginModel.Password);

            if (data.Success)
            {
                if(data.Data != null)
                {
                    return new BaseResponse<UserInfoModel>
                    {
                        Data = _mapper.Map<UserInfoModel>(data.Data),
                        Message = "Zalogowano"
                    };
                }
                else
                {
                    return new BaseResponse<UserInfoModel>
                    {
                        Success = false,
                        Message = "Nie ma konta z takim mailem i hasłem"
                    };
                }
            }
            else
            {
                return new BaseResponse<UserInfoModel> 
                { 
                    Success = false,
                    Message = "Problem z systemem, prosimy spróbowac za jakiś czas"
                };
            }
        }
    }
}
