﻿using AutoMapper;
using Data.Repositories.interfaces;
using Data.Responses;
using MVC.Models;
using MVC.Services.Interfaces;

namespace MVC.Services
{
    public class ProductService : IProductService
    {
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;

        public ProductService(IMapper mapper, IProductRepository productRepository)
        {
            _mapper = mapper;
            _productRepository = productRepository;
        }

        public async Task<BaseResponse<int>> Active(int productid)
        {
            var data = await _productRepository.ActiveProduct(productid);

            if (data.Success && data.Data != null)
            {
                return new BaseResponse<int>() { Data = data.Data };
            }

            return new BaseResponse<int>()
            {
                Success = false,
                Message = "Problem z systemem, prosimy spróbowac za jakiś czas"
            };
        }

        public async Task<BaseResponse<int>> DeActive(int productid)
        {
            var data = await _productRepository.DeActiveProduct(productid);

            if (data.Success && data.Data != null)
            {
                return new BaseResponse<int>() { Data = data.Data };
            }

            return new BaseResponse<int>()
            {
                Success = false,
                Message = "Problem z systemem, prosimy spróbowac za jakiś czas"
            };
        }

        public async Task<BaseResponse<List<ProductListModel>>> GetProducts()
        {
            var data = await _productRepository.GetAllProducts();

            if (data.Success && data.Data != null)
            {
                return new BaseResponse<List<ProductListModel>>() { Data = _mapper.Map<List<ProductListModel>>(data.Data.OrderBy(p => p.Id)) };
            }

            return new BaseResponse<List<ProductListModel>>
            {
                Success = false,
                Message = "Problem z systemem, prosimy spróbowac za jakiś czas"
            };
        }
    }
}