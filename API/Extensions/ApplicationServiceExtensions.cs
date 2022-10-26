using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Interfaces;
using API.Interfaces.Logics;
using API.Services;
using API.Services.Logics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<ITokenService, TokenService>();

            services.AddScoped<IFridgeService, FridgeService>();
            services.AddScoped<IFridgeProductService, FridgeProductService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<IFridgeModelService, FridgeModelService>();
            services.AddScoped<IProductService, ProductService>();

            return services;
        }
    }
}