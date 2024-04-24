using CleanArchMvc.Application.Interfaces;
using CleanArchMvc.Application.Mappings;
using CleanArchMvc.Application.Services;
using CleanArchMvc.Domain.Interfaces;
using CleanArchMvc.Infra.Data.Contexts;
using CleanArchMvc.Infra.Data.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace CleanArchMvc.Infra.IoC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            #region Database

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName));
            });

            #endregion

            #region Repositories

            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();

            #endregion

            #region Services

            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IProductService, ProductService>();

            #endregion

            #region Automapper Profile

            services.AddAutoMapper(typeof(DomainToDTOMappingProfile));

            #endregion

            #region MediatR

            var myHandlers = AppDomain.CurrentDomain.Load("CleanArchMvc.Application");
            services.AddMediatR(myHandlers);

            #endregion

            return services;
        }
    }
}
