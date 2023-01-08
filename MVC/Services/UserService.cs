using AutoMapper;
using Data.Entity;
using Data.Repositories.interfaces;
using Data.Responses;
using MVC.Models;
using MVC.Services.Interfaces;
using System.Security.Claims;

namespace MVC.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public UserService(IMapper mapper, IUserRepository userRepository)
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task<BaseResponse<int>> EditUser(UserEditModel userEdit)
        {
            var data = await _userRepository.UpdateNameUser(
                 _mapper.Map<User>(userEdit));
            
            if (data.Success && data.Data != 0)
            {
                return data;
            }
            else
            {
                return new BaseResponse<int>()
                {
                    Success = false,
                    Message = "Nie udało się zmienić danych"
                };
            }
        }
    }
}
