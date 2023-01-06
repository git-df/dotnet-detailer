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
	public class ProductRepository : DbRepository<Product>, IProductRepository
	{
		public ProductRepository(DapperDbContext dapper) 
			: base(dapper) { }

		public async Task<BaseResponse<List<Product>>> GetAllProducts()
		{
			var sql = "select * from public.product";
			return await GetAll(sql, new { } );
		}
	}
}
