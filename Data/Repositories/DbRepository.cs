using Data.Repositories.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Data.Responses;

namespace Data.Repositories
{
    public class DbRepository<T> : IDbRepository<T> where T : class
    {
        private readonly DapperDbContext _dapper;

        public DbRepository(DapperDbContext dapper)
        {
            _dapper = dapper;
        }

        public async Task<BaseResponse<int>> EditData(string sql, object parms)
        {
            using (var con = _dapper.CreateConnection())
            {
                try
                {
                    con.Open();
                    var data = await con.ExecuteAsync(sql, parms);
                    return new BaseResponse<int>() { Data = data };
                }
                catch(Exception ex)
                {
                    return new BaseResponse<int>()
                    {
                        Success = false,
                        Message = ex.Message,
                    };
                }
                finally { con.Close(); }
            }
        }

        public async Task<BaseResponse<int>> EditDataGetId(string sql, object parms)
        {
            using (var con = _dapper.CreateConnection())
            {
                try
                {
                    con.Open();
                    var data = await con.QuerySingleAsync<int>(sql, parms);
                    return new BaseResponse<int>() { Data = data };
                }
                catch (Exception ex)
                {
                    return new BaseResponse<int>()
                    {
                        Success = false,
                        Message = ex.Message,
                    };
                }
                finally { con.Close(); }
            }
        }

        public async Task<BaseResponse<List<T>>> GetAll(string sql, object parms)
        {
            using (var con = _dapper.CreateConnection())
            {
                try
                {
                    con.Open();
                    var data = (await con.QueryAsync<T>(sql, parms)).ToList();
                    return new BaseResponse<List<T>>() { Data = data };
                }
                catch (Exception ex)
                {
                    return new BaseResponse<List<T>>()
                    {
                        Success = false,
                        Message = ex.Message,
                    };
                }
                finally { con.Close(); }
            }
        }

        public async Task<BaseResponse<T>> GetAsync(string sql, object parms)
        {
            using (var con = _dapper.CreateConnection())
            {
                try
                {
                    con.Open();
                    var data = (await con.QueryAsync<T>(sql, parms).ConfigureAwait(false)).FirstOrDefault();
                    return new BaseResponse<T>() { Data = data };
                }
                catch (Exception ex)
                {
                    return new BaseResponse<T>()
                    {
                        Success = false,
                        Message = ex.Message,
                    };
                }
                finally { con.Close(); }
            }
        }

        public async Task<BaseResponse<List<T>>> GetFilterAll(string sql, object parms, Expression<Func<T, bool>> filter)
        {
            var result = await GetAll(sql, parms);

            return new BaseResponse<List<T>>()
            {
                Success = result.Success,
                Message = result.Message,
                Data = result.Data.AsQueryable().Where(filter).ToList()
            };
        }

        public async Task<BaseResponse<T>> GetFilterAsync(string sql, object parms, Expression<Func<T, bool>> filter)
        {
            var result = await GetAll(sql, parms);

            return new BaseResponse<T>() 
            { 
                Success = result.Success,
                Message = result.Message,
                Data = result.Data.AsQueryable().SingleOrDefault(filter)
            };
        }
    }
}
