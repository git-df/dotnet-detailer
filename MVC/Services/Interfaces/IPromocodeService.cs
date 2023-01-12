using Data.Responses;
using MVC.Models;

namespace MVC.Services.Interfaces
{
    public interface IPromocodeService
    {
        Task<BaseResponse<List<PromocodeInListModel>>> GetPromocodelist();
        Task<BaseResponse<int>> Delete(int promocodeid);
        Task<BaseResponse<int>> Add(PromocodeAddModel promocode);
    }
}
