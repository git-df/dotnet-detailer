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
            CreateMap<Product, ProductInPriceListModel>().ReverseMap();
            CreateMap<Category, CategoryWithProductsPriceListModel>().ReverseMap();
            CreateMap<Employee, EmployeeInUserInfoModel>().ReverseMap();
        }
    }
}
