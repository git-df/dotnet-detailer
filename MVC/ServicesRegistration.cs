using MVC.Services.Interfaces;
using MVC.Services;
using System.Reflection;

namespace MVC
{
    public static class ServicesRegistration
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddScoped<IAccessService, AccessService>();
            services.AddScoped<IPriceListService, PriceListService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPromocodeService, PromocodeService>();
            services.AddScoped<IOrderService, OrderService>();
            return services;
        }
    }
}
