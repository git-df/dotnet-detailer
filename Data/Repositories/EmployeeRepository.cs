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

        public async Task<BaseResponse<Employee>> GetEmployeeByUserId(int userid)
        {
            var sql = "select * from public.employee where userid=@userid";
            return await GetAsync(sql, new { userid });
        }
    }
}
