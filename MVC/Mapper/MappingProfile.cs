using AutoMapper;
using Data.Entity;
using MVC.Models;

namespace MVC.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserLoginModel>().ReverseMap();
            CreateMap<User, UserInfoModel>().ReverseMap();
            CreateMap<User, UserRegisterModel>().ReverseMap();
            CreateMap<User, UserEditModel>().ReverseMap();
            CreateMap<User, UserPasswordChangeModel>().ReverseMap();
            CreateMap<UserInfoModel, UserEditModel>().ReverseMap();
            CreateMap<Product, ProductInPriceListModel>().ReverseMap();
            CreateMap<Product, ProductInPromocodeListModel>().ReverseMap();
            CreateMap<Product, ProductInOrderModel>().ReverseMap();
            CreateMap<Category, CategoryWithProductsPriceListModel>().ReverseMap();
            CreateMap<Employee, EmployeeInUserInfoModel>().ReverseMap();
            CreateMap<Promocode, PromocodeInListModel>().ReverseMap();
            CreateMap<OrderProductModel, ProductInOrderModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ProductId)).ReverseMap();
        }
    }
}
