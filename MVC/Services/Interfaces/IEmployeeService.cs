using Data.Responses;
using MVC.Models;

namespace MVC.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<BaseResponse<List<UserEmployeeListModel>>> GetEmployeelist();
        Task<BaseResponse<List<UserEmployeeListModel>>> GetAdminslist();
        Task<BaseResponse<int>> DeleteEmployee(int userid);
        Task<BaseResponse<int>> DeleteAdmin(int userid);
        Task<BaseResponse<int>> AddEmployee(AddEmployeeModel employeeModel);
        Task<BaseResponse<int>> AddAdmin(AddEmployeeModel employeeModel);
    }
}