using Data.Responses;
using MVC.Models;

namespace MVC.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<BaseResponse<List<UserEmployeeListModel>>> GetEmployeelist();
        Task<BaseResponse<List<UserEmployeeListModel>>> GetAdminslist();
    }
}