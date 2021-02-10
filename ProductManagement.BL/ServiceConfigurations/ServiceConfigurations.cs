using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using ProductManagement.BL;
using ProductManagement.DAL;
using ProductManagement.Interfaces;

namespace ProductManagement.BL.ServiceConfigurations
{
    public static class ServiceConfigurations
    {
        public static IServiceCollection AddInternalServices(this IServiceCollection configuration)
        {
            configuration.AddScoped<IProductOps, ProductsBL>();
            configuration.AddScoped<ICartOps, CartBL>();
            configuration.AddScoped<IOrderOps, OrdersBL>();
            configuration.AddScoped<IAuthOps, AuthBL>();
            configuration.AddDbServiceConfigurations();
            return configuration;
        }
    }
}
