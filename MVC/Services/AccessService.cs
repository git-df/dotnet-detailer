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
            var dataSalt = await _userRepository.GetUserByEmail(userLoginModel.Email);
            
            if (dataSalt.Success)
            {
                if(dataSalt.Data != null)
                {
                    var dataUser = await _userRepository.GetUserByEmailAndPassword(userLoginModel.Email, HashPassword($"{userLoginModel.Password}{dataSalt.Data.Salt}"));

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
            var userExist = await _userRepository.GetUserByEmail(userRegisterModel.Email);

            if (userExist.Success)
            {
                if (userExist.Data == null)
                {
                    var user = _mapper.Map<User>(userRegisterModel);

                    user.Salt = SaltGenerator();
                    user.Password = HashPassword($"{userRegisterModel.Password}{user.Salt}");

                    var data = await _userRepository.CreateUser(user);

                    if (data.Success)
                    {
                        if (data.Data > 0)
                        {
                            return new BaseResponse<int>
                            {
                                Data = data.Data,
                                Message = "Zarejestrowano"
                            };
                        }
                    }
                }
                else
                {
                    return new BaseResponse<int>
                    {
                        Success = false,
                        Message = "Konto z takim Emailem już istnieje"
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

        string SaltGenerator()
        {
            var rng = RandomNumberGenerator.Create();
            byte[] salt = new byte[32];
            rng.GetNonZeroBytes(salt);
            return Convert.ToHexString(salt);
        }
    }
}
