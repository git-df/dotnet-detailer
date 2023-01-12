using Data.Entity;
using Data.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories.interfaces
{
	public interface IProductRepository
	{
		Task<BaseResponse<List<Product>>> GetAllProducts();
        Task<BaseResponse<List<Product>>> GetUserProducts(int userid);
        Task<BaseResponse<int>> CreateProduct(Product product);
        Task<BaseResponse<int>> ActiveProduct(int productid);
        Task<BaseResponse<int>> DeActiveProduct(int productid);
    }
}
