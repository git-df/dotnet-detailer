using Data.Responses;
using MVC.Models;

namespace MVC.Services.Interfaces
{
    public interface IOfferUserService
    {
        Task<BaseResponse<List<OfferUserListModel>>> GetOfferList();
        Task<BaseResponse<int>> AddOfferUser(OfferUserAddModel offerUser);
        Task<BaseResponse<int>> DeleteOffer(int offerUserId);
    }
}
