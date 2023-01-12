using Data.Repositories;
using Data.Repositories.interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public static class DataRegistration
    {
        public static IServiceCollection AddData(this IServiceCollection services)
        {
            services.AddSingleton<DapperDbContext>();
            services.AddScoped(typeof(IDbRepository<>), typeof(DbRepository<>));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IPromocodeRepository, PromocodeRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderProductRepository, OrderProductRepository>();
            services.AddScoped<IOfferUserRepository, OfferUserRepository>();
            return services;
        }
    }
}
