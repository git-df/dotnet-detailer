using AutoMapper;
using Data.Repositories.interfaces;
using Data.Responses;
using MVC.Models;
using MVC.Services.Interfaces;

namespace MVC.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IMapper _mapper;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUserRepository _userRepository;

        public EmployeeService(IMapper mapper, IEmployeeRepository employeeRepository, IUserRepository userRepository)
        {
            _mapper = mapper;
            _employeeRepository = employeeRepository;
            _userRepository = userRepository;
        }

        public async Task<BaseResponse<List<UserEmployeeListModel>>> GetAdminslist()
        {
            var users = await _userRepository.GetAllUsers();
            var employeelist = await _employeeRepository.GetAllEmployes();
            var response = new List<UserEmployeeListModel>();

            if (users.Success && users.Data != null && employeelist.Success && employeelist.Data != null)
            {
                foreach (var employee in employeelist.Data.Where(e => e.IsAdmin == true))
                {
                    var user = users.Data.SingleOrDefault(u => u.Id == employee.UserId);

                    if (user != null)
                    {
                        response.Add(_mapper.Map<UserEmployeeListModel>(user));
                    }
                }

                return new BaseResponse<List<UserEmployeeListModel>>() { Data = response };
            }

            return new BaseResponse<List<UserEmployeeListModel>>()
            {
                Success = false,
                Message = "Problem z systemem, prosimy spróbowac za jakiś czas"
            };
        }

        public async Task<BaseResponse<List<UserEmployeeListModel>>> GetEmployeelist()
        {
            var users = await _userRepository.GetAllUsers();
            var employeelist = await _employeeRepository.GetAllEmployes();
            var response = new List<UserEmployeeListModel>();

            if (users.Success && users.Data != null && employeelist.Success && employeelist.Data != null)
            {
                foreach (var employee in employeelist.Data)
                {
                    var user = users.Data.SingleOrDefault(u => u.Id == employee.UserId);

                    if (user != null)
                    {
                        response.Add(_mapper.Map<UserEmployeeListModel>(user));
                    }
                }

                return new BaseResponse<List<UserEmployeeListModel>>() { Data = response };
            }

            return new BaseResponse<List<UserEmployeeListModel>>()
            {
                Success = false,
                Message = "Problem z systemem, prosimy spróbowac za jakiś czas"
            };
        }
    }
}
