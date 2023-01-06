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
	}
}
