using AutoMapper;
using Data.Entity;
using Data.Repositories.interfaces;
using Data.Responses;
using MVC.Models;
using MVC.Services.Interfaces;
using System.Collections.Generic;

namespace MVC.Services
{
    public class PromocodeService : IPromocodeService
    {
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;
        private readonly IPromocodeRepository _promocodeRepository;

        public PromocodeService(IMapper mapper, IPromocodeRepository promocodeRepository, IProductRepository productRepository)
        {
            _mapper = mapper;
            _promocodeRepository = promocodeRepository;
            _productRepository = productRepository;
        }

        public async Task<BaseResponse<int>> Add(PromocodeAddModel promocode)
        {
            var products = await _productRepository.GetAllProducts();

            if (products.Success && products.Data != null)
            {
                if (products.Data.SingleOrDefault(p => p.Id == promocode.ProductId) != null)
                {
                    var data = await _promocodeRepository.Add(_mapper.Map<Promocode>(promocode));

                    if (data.Success && data.Data != 0)
                    {
                        return new BaseResponse<int>() { Data = data.Data };
                    }
                }
                else
                {
                    return new BaseResponse<int>()
                    {
                        Success = false,
                        Message = "Błędne id produktu"
                    };
                }
            }

            return new BaseResponse<int>()
            {
                Success = false,
                Message = "Problem z systemem, prosimy spróbowac za jakiś czas"
            };
        }

        public async Task<BaseResponse<int>> Delete(int promocodeid)
        {
            var data = await _promocodeRepository.Delete(promocodeid);

            if (data.Success && data.Data != 0)
            {
                return new BaseResponse<int>() { Data = data.Data };
            }

            return new BaseResponse<int>()
            {
                Success = false,
                Message = "Problem z systemem, prosimy spróbowac za jakiś czas"
            };
        }

        public async Task<BaseResponse<List<PromocodeInListModel>>> GetPromocodelist()
        {
            var productList = await _productRepository.GetAllProducts();
            var promocodeList = await _promocodeRepository.GetAllPromocodes();

            if (promocodeList.Success && promocodeList.Data != null && productList.Success && productList.Data != null)
            {
                List<PromocodeInListModel> list = new List<PromocodeInListModel>();

                foreach (var promocode in promocodeList.Data)
                {
                    var product = productList.Data.SingleOrDefault(p => p.Id == promocode.ProductId && p.IsActive == true);

                    if (product != null)
                    {
                        var el = _mapper.Map<PromocodeInListModel>(promocode);
                        el.Product = _mapper.Map<ProductInPromocodeListModel>(product);
                        list.Add(el);
                    }
                }

                return new BaseResponse<List<PromocodeInListModel>>() { Data = list };
            }


            return new BaseResponse<List<PromocodeInListModel>>()
            {
                Success = false,
                Message = "Problem z systemem, prosimy spróbowac za jakiś czas"
            };
        }
    }
}
