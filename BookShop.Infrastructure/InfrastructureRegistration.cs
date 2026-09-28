using BookShop.Application.Interfaces;
using BookShop.Application.IServices;
using BookShop.Infrastructure.Data;
using BookShop.Infrastructure.Repositories;
using BookShop.Infrastructure.Services.Auth;
using BookShop.Infrastructure.Services.CloudinaryService;
using BookShop.Infrastructure.Services.StripePayment;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookShop.Infrastructure
{
    public static class InfrastructureRegistration
    {
        public static IServiceCollection InfrastructureConfiguration(this IServiceCollection services,
                                                                     IConfiguration configuration)
        {
            // DbContext
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IAuthorRepository, AuthorRepository>();
            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IRatingRepository, RatingRepository>();

            services.AddScoped<ICloudinaryUploadService, CloudinaryUploadService>();
            services.Configure<CloudinarySettings>(configuration.GetSection("CloudinarySettings"));
            services.Configure<StripeSettings>(configuration.GetSection("StripeSettings"));

            services.AddScoped<IAuthService, AuthService>();

            return services;
        }
    }
}
