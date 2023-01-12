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

		public async Task<BaseResponse<int>> ActiveCategory(int categoryid)
		{
            var sql = "update public.category set isactive = true where id = @categoryid";
            return await EditData(sql, new { categoryid });
        }

		public async Task<BaseResponse<int>> CreateCategory(Category category)
		{
			var sql = "insert  into public.category (name, isactive) values(@Name, true)";
			return await EditData(sql, category);
		}

		public async Task<BaseResponse<int>> DeActiveCategory(int categoryid)
		{
            var sql = "update public.category set isactive = false where id = @categoryid";
            return await EditData(sql, new { categoryid });
        }

		public async Task<BaseResponse<List<Category>>> GetAllCategories()
		{
			var sql = "select * from public.category";
			return await GetAll(sql, new { });
		}
	}
}
