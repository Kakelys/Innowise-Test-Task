using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Data.Repositories;
using API.Interfaces;
using API.Interfaces.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace API.Extensions
{
    public static class RepositoryServiceExtensions
    {
        public static IServiceCollection AddRepositoryServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<RepositoryContext>(options =>
            {
                options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            });

            services.AddScoped<IFridgeRepository, FridgeRepository>();
            services.AddScoped<IFridgeProductRepository, FridgeProductRepository>();
            services.AddScoped<IFridgeModelRepository, FridgeModelRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductPictureRepository, ProductPictureRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            
            services.AddScoped<IRepositoryManager, RepositoryManager>();

            return services;
        }
    }
}