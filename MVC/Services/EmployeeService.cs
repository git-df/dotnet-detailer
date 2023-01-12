using AutoMapper;
using Data.Entity;
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

        public async Task<BaseResponse<int>> AddAdmin(AddEmployeeModel employeeModel)
        {
            var user = await _userRepository.GetUserByEmail(employeeModel.Email);

            if (user.Success && user.Data != null)
            {
                var employee = await _employeeRepository.GetEmployeeByUserId(user.Data.Id);

                if (employee.Success)
                {
                    if (employee.Data != null)
                    {
                        if (!employee.Data.IsAdmin)
                        {
                            var data = await _employeeRepository.SetAdminTrue(employee.Data.Id);

                            if (data.Success)
                            {
                                return new BaseResponse<int>() { Data = data.Data };
                            }
                        }
                        else
                        {
                            return new BaseResponse<int>()
                            {
                                Success = false,
                                Message = "Ten pracownik ma już prawa administratowa"
                            };
                        }
                    }
                    else
                    {
                        return new BaseResponse<int>()
                        {
                            Success = false,
                            Message = "Nie ma pracownika z takim mailem"
                        };
                    }
                }
            }

            return new BaseResponse<int>()
            {
                Success = false,
                Message = "Problem z systemem, prosimy spróbowac za jakiś czas"
            };
        }

        public async Task<BaseResponse<int>> AddEmployee(AddEmployeeModel employeeModel)
        {
            var user = await _userRepository.GetUserByEmail(employeeModel.Email);

            if (user.Success)
            {
                if (user.Data != null)
                {
                    var employee = await _employeeRepository.GetEmployeeByUserId(user.Data.Id);

                    if (employee.Success)
                    {
                        if (employee.Data == null)
                        {
                            var data = await _employeeRepository.AddEmployee(
                            new Employee() { UserId = user.Data.Id, IsAdmin = false });

                            return new BaseResponse<int>() { Data = data.Data };
                        }
                        else
                        {
                            return new BaseResponse<int>()
                            {
                                Success = false,
                                Message = "Jest już pracownik z takim mailem"
                            };
                        }
                    }
                }
                else
                {
                    return new BaseResponse<int>()
                    {
                        Success = false,
                        Message = "Nie ma użytkownika z takim mailem"
                    };
                }
            }

            return new BaseResponse<int>()
            {
                Success = false,
                Message = "Problem z systemem, prosimy spróbowac za jakiś czas"
            };
        }

        public async Task<BaseResponse<int>> DeleteAdmin(int userid)
        {
            var employee = await _employeeRepository.GetEmployeeByUserId(userid);

            if (employee.Success && employee.Data != null)
            {
                var data = await _employeeRepository.SetAdminFalse(employee.Data.Id);

                if (data.Success)
                {
                    return new BaseResponse<int>() { Data = data.Data };
                }
            }

            return new BaseResponse<int>()
            {
                Success = false,
                Message = "Problem z systemem, prosimy spróbowac za jakiś czas"
            };
        }

        public async Task<BaseResponse<int>> DeleteEmployee(int userid)
        {
            var employee = await _employeeRepository.GetEmployeeByUserId(userid);

            if (employee.Success && employee.Data != null)
            {
                var data = await _employeeRepository.DeleteEmployee(employee.Data.Id);

                if (data.Success)
                {
                    return new BaseResponse<int>() { Data = data.Data };
                }
            }

            return new BaseResponse<int>()
            {
                Success = false,
                Message = "Problem z systemem, prosimy spróbowac za jakiś czas"
            };
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
