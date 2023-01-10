using Data.Entity;
using Data.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories.interfaces
{
    public interface IEmployeeRepository
    {
        Task<BaseResponse<Employee>> GetEmployeeByUserId(int userid);
        Task<BaseResponse<List<Employee>>> GetAllEmployes();
        Task<BaseResponse<int>> SetAdminFalse(int employeeid);
        Task<BaseResponse<int>> SetAdminTrue(int employeeid);
        Task<BaseResponse<int>> DeleteEmployee(int employeeid);
        Task<BaseResponse<int>> AddEmployee(Employee employee);
    }
}
