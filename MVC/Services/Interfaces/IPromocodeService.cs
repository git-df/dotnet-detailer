using Data.Responses;
using MVC.Models;

namespace MVC.Services.Interfaces
{
    public interface IPromocodeService
    {
        Task<BaseResponse<List<PromocodeInListModel>>> GetPromocodelist();
    }
}
