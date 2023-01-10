using Data.Entity;
using Data.Repositories.interfaces;
using Data.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class EmployeeRepository : DbRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(DapperDbContext dapper) 
            : base(dapper) { }

        public async Task<BaseResponse<int>> AddEmployee(Employee employee)
        {
            var sql = "insert into public.employee (userid, isadmin) values(@UserId, @IsAdmin)";
            return await EditData(sql, employee);
        }

        public async Task<BaseResponse<int>> DeleteEmployee(int employeeid)
        {
            var sql = "delete from public.employee where id = @employeeid";
            return await EditData(sql, new { employeeid });
        }

        public async Task<BaseResponse<List<Employee>>> GetAllEmployes()
        {
            var sql = "select * from public.employee";
            return await GetAll(sql, new { });
        }

        public async Task<BaseResponse<Employee>> GetEmployeeByUserId(int userid)
        {
            var sql = "select * from public.employee where userid=@userid";
            return await GetAsync(sql, new { userid });
        }

        public async Task<BaseResponse<int>> SetAdminFalse(int employeeid)
        {
            var sql = "update public.employee set isadmin = false where id = @employeeid";
            return await EditData(sql, new { employeeid });
        }

        public async Task<BaseResponse<int>> SetAdminTrue(int employeeid)
        {
            var sql = "update public.employee set isadmin = true where id = @employeeid";
            return await EditData(sql, new { employeeid });
        }
    }
}
