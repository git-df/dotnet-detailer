using Data.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories.interfaces
{
    public interface IDbRepository<T> where T : class
    {
        Task<BaseResponse<T>> GetAsync(string sql, object parms);
        Task<BaseResponse<T>> GetFilterAsync(string sql, object parms, Expression<Func<T, bool>> filter);
        Task<BaseResponse<List<T>>> GetAll(string sql, object parms);
        Task<BaseResponse<List<T>>> GetFilterAll(string sql, object parms, Expression<Func<T, bool>> filter);
        Task<BaseResponse<int>> EditData(string sql, object parms);
        Task<BaseResponse<int>> EditDataGetId(string sql, object parms);
    }
}
