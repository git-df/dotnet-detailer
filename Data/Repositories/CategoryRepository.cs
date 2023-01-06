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
	internal class CategoryRepository : DbRepository<Category>, ICategoryRepository
	{
		public CategoryRepository(DapperDbContext dapper) 
			: base(dapper) { }

		public async Task<BaseResponse<List<Category>>> GetAllCategories()
		{
			var sql = "select * from public.category";
			return await GetAll(sql, new { });
		}
	}
}
