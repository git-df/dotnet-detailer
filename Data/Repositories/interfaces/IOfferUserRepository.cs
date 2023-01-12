using Data.Entity;
using Data.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories.interfaces
{
    public interface IOfferUserRepository
    {
        Task<BaseResponse<List<OfferUser>>> GetOfferUserList();
        Task<BaseResponse<int>> DeleteOferUser(int offerid);
        Task<BaseResponse<int>> CreateOferUser(OfferUser offerUser);
    }
}
