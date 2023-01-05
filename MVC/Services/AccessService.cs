using AutoMapper;
using Data.Repositories.interfaces;
using Data.Responses;
using MVC.Models;
using MVC.Services.Interfaces;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Cryptography;
using System.Text;
using Data.Entity;
using System.Data;

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
            var dataSalt = await _userRepository.GetUserSalt(userLoginModel.Email);
            
            if (dataSalt.Success)
            {
                if(dataSalt.Data != null)
                {
                    var dataUser = await _userRepository.GetUserByEmailAndPassword(userLoginModel.Email, HashPassword($"{userLoginModel.Password}{dataSalt.Data}"));

                    if (dataUser.Success)
                    {
                        if (dataUser.Data != null)
                        {
                            return new BaseResponse<UserInfoModel>
                            {
                                Data = _mapper.Map<UserInfoModel>(dataUser.Data),
                                Message = "Zalogowano"
                            };
                        }
                        else
                        {
                            return new BaseResponse<UserInfoModel>
                            {
                                Success = false,
                                Message = "Błędne hasło"
                            };
                        }
                    }
                }
                else
                {
                    return new BaseResponse<UserInfoModel>
                    {
                        Success = false,
                        Message = "Nie ma konta z takim mailem"
                    };
                }
            }

            return new BaseResponse<UserInfoModel>
            {
                Success = false,
                Message = "Problem z systemem, prosimy spróbowac za jakiś czas"
            };
        }

        public async Task<BaseResponse<int>> Register(UserRegisterModel userRegisterModel)
        {
            //Trzeba dodac zabezpieczenie by nie tworzyć z takim samym mailem
            var user = _mapper.Map<User>(userRegisterModel);

            user.Salt = DateTime.Now.ToString();
            user.Password = HashPassword($"{userRegisterModel.Password}{user.Salt}");

            var data = await _userRepository.CreateUser(user);

            if(data.Success)
            {
                if(data.Data > 0)
                {
                    return new BaseResponse<int>
                    {
                        Data = data.Data,
                        Message = "Zarejestrowano"
                    };
                }
            }

            return new BaseResponse<int>
            {
                Success = false,
                Message = "Problem z systemem, prosimy spróbowac za jakiś czas"
            };
        }

        string HashPassword(string password)
        {
            SHA256 sHA256 = SHA256.Create();

            var passwordBytes = Encoding.Default.GetBytes(password);

            var passwordHash = sHA256.ComputeHash(passwordBytes);

            return Convert.ToHexString(passwordHash);
        }
    }
}
