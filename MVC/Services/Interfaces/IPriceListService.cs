﻿using Data.Responses;
using MVC.Models;

namespace MVC.Services.Interfaces
{
	public interface IPriceListService
	{
		Task<BaseResponse<List<CategoryWithProductsPriceListModel>>> GetPricelist();
	}
}
