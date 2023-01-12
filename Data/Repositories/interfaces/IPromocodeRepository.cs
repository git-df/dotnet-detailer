using Data.Entity;
using Data.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories.interfaces
{
    public  interface IPromocodeRepository
    {
        Task<BaseResponse<List<Promocode>>> GetAllPromocodes();
        Task<BaseResponse<List<Promocode>>> GetPromocodesByCode(string code);
        Task<BaseResponse<int>> Delete(int promocodeid);
        Task<BaseResponse<int>> Add(Promocode promocode);
    }
}
