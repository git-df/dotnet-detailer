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
        private readonly IEmployeeRepository _employeeRepository;

        public AccessService(IMapper mapper, IUserRepository userRepository, IEmployeeRepository employeeRepository)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _employeeRepository = employeeRepository;
        }

        public async Task<BaseResponse<UserInfoModel>> GetUserInfo(int userid)
        {
            var user = await _userRepository.GetUserById(userid);
            var employee = await _employeeRepository.GetEmployeeByUserId(userid);

            if(user.Success && employee.Success)
            {
                if(user.Data != null)
                {
                    var data = new BaseResponse<UserInfoModel>
                    {
                        Data = _mapper.Map<UserInfoModel>(user.Data)
                    };

                    if (employee.Data != null) 
                        data.Data.Employee = _mapper.Map<EmployeeInUserInfoModel>(employee.Data);

                    return data;
                }
                else
                {
                    return new BaseResponse<UserInfoModel>
                    {
                        Success = false,
                        Message = "Nie ma takiego użytkownika"
                    };
                }
            }

            return new BaseResponse<UserInfoModel>
            {
                Success = false,
                Message = "Problem z systemem, prosimy spróbowac za jakiś czas"
            };
        }

        public async Task<BaseResponse<int>> PasswordChange(UserPasswordChangeModel userPasswordChange)
        {
            var user = await _userRepository.GetUserById(userPasswordChange.Id);

            if(user.Success && user.Data != null)
            {
                if (user.Data.Password == HashPassword($"{userPasswordChange.OldPassword}{user.Data.Salt}"))
                {
                    var userNewPassword = _mapper.Map<User>(user.Data);
                    userNewPassword.Salt = SaltGenerator();
                    userNewPassword.Password = HashPassword($"{userPasswordChange.NewPassword}{userNewPassword.Salt}");
                    var data = await _userRepository.PasswordChange(userNewPassword);

                    if (data.Success)
                    {
                        return new BaseResponse<int>
                        {
                            Data = data.Data
                        };
                    }
                }
                else
                {
                    return new BaseResponse<int>
                    {
                        Success = false,
                        Message = "Błędne hasło"
                    };
                }
            }

            return new BaseResponse<int>
            {
                Success = false,
                Message = "Problem z systemem, prosimy spróbowac za jakiś czas"
            };
        }

        public async Task<BaseResponse<UserInfoModel>> SignIn(UserLoginModel userLoginModel)
        {
            var data = await _userRepository.GetUserByEmail(userLoginModel.Email);
            
            if (data.Success)
            {
                if (data.Data != null)
                {

                    if (data.Data.Password == HashPassword($"{userLoginModel.Password}{data.Data.Salt}"))
                    {
                        var userInfo = await GetUserInfo(data.Data.Id);

                        if (userInfo.Success)
                        {
                            return new BaseResponse<UserInfoModel>
                            {
                                Data = userInfo.Data,
                                Message = "Zalogowano"
                            };
                        }
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

            return new BaseResponse<UserInfoModel>
            {
                Success = false,
                Message = "Problem z systemem, prosimy spróbowac za jakiś czas"
            };
        }

        public async Task<BaseResponse<int>> SignUp(UserRegisterModel userRegisterModel)
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
