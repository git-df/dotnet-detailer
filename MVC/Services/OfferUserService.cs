using AutoMapper;
using Data.Entity;
using Data.Repositories.interfaces;
using Data.Responses;
using MVC.Models;
using MVC.Services.Interfaces;

namespace MVC.Services
{
    public class OfferUserService : IOfferUserService
    {
        private readonly IMapper _mapper;
        private readonly IOfferUserRepository _offerUserRepository;
        private readonly IUserRepository _userRepository;
        private readonly IProductRepository _productRepository;

        public OfferUserService(IMapper mapper, IOfferUserRepository offerUserRepository, IUserRepository userRepository, IProductRepository productRepository)
        {
            _mapper = mapper;
            _offerUserRepository = offerUserRepository;
            _userRepository = userRepository;
            _productRepository = productRepository;
        }

        public async Task<BaseResponse<int>> AddOfferUser(OfferUserAddModel offerUser)
        {
            var user = await _userRepository.GetUserById(offerUser.UserId);
            var product = await _productRepository.GetAllProducts();

            if (user.Success && product.Success && product.Data != null)
            {
                if (user.Data != null)
                {
                    if (product.Data.SingleOrDefault(p => p.Id == offerUser.ProductId) != null)
                    {
                        var data = await _offerUserRepository.CreateOferUser(_mapper.Map<OfferUser>(offerUser));

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
                else
                {
                    return new BaseResponse<int>()
                    {
                        Success = false,
                        Message = "Błędne id użytkownika"
                    };
                }
            }

            return new BaseResponse<int>()
            {
                Success = false,
                Message = "Problem z systemem, prosimy spróbowac za jakiś czas"
            };
        }

        public async Task<BaseResponse<int>> DeleteOffer(int offerUserId)
        {
            var data = await _offerUserRepository.DeleteOferUser(offerUserId);

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

        public async Task<BaseResponse<List<OfferUserListModel>>> GetOfferList()
        {
            var offerList = await _offerUserRepository.GetOfferUserList();
            var products = await _productRepository.GetAllProducts();
            var users = await _userRepository.GetAllUsers();

            if (offerList.Success && products.Success && users.Success && offerList.Data != null && products.Data != null && users.Data != null)
            {
                List<OfferUserListModel> response = new List<OfferUserListModel>();

                foreach (var offer in offerList.Data)
                {
                    response.Add(new OfferUserListModel() 
                    { 
                        Price = offer.Price,
                        Id = offer.Id,
                        Email = users.Data.SingleOrDefault(u => u.Id == offer.UserId).Email,
                        ProductName = products.Data.SingleOrDefault(p => p.Id == offer.ProductId).Name
                    });
                }

                return new BaseResponse<List<OfferUserListModel>>() { Data = response };
            }

            return new BaseResponse<List<OfferUserListModel>>()
            {
                Success = false,
                Message = "Problem z systemem, prosimy spróbowac za jakiś czas"
            };
        }
    }
}
