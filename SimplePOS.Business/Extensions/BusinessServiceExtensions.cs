using Microsoft.Extensions.DependencyInjection;
using SimplePOS.Business.Interfaces;
using SimplePOS.Business.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplePOS.Business.Extensions
{
    public static class BusinessServiceExtensions
    {
        public static IServiceCollection AddBusinessServices(this IServiceCollection services)
        {
            // Aquí puedes registrar tus servicios de negocio
            services.AddScoped<IClientService, ClientService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ISaleService, SaleService>();
            services.AddScoped<IPaginationService, PaginationService>();
            return services;
        }
    }
}
