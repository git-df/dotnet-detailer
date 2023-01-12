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

		public async Task<BaseResponse<int>> ActiveProduct(int productid)
		{
            var sql = "update public.product set isactive = true where id = @productid";
            return await EditData(sql, new { productid });
        }

		public async Task<BaseResponse<int>> CreateProduct(Product product)
		{
			var sql = "insert into public.product (categoryid, name, description, price, duration, isactive) values(@CategoryId, @Name ,@Description ,@Price ,@Duration, true)";
			return await EditData(sql, product);
		}

		public async Task<BaseResponse<int>> DeActiveProduct(int productid)
		{
            var sql = "update public.product set isactive = false where id = @productid";
            return await EditData(sql, new { productid });
        }

		public async Task<BaseResponse<List<Product>>> GetAllProducts()
		{
			var sql = "select * from public.product";
			return await GetAll(sql, new { } );
		}

		public async Task<BaseResponse<List<Product>>> GetUserProducts(int userid)
		{
            var sql = "select p.id, p.categoryid, p.name, p.description, o.price, p.duration, p.isactive  from public.product p join public.offeruser o on p.id = o.productid where o.userid=@userid";
            return await GetAll(sql, new { userid });
        }
	}
}
