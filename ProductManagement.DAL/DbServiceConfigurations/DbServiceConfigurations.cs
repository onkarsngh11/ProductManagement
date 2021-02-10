using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProductManagement.Interfaces;

namespace ProductManagement.DAL
{
    public static class DbServiceConfigurations
    {
        public static IServiceCollection AddDbServiceConfigurations(this IServiceCollection configuration)
        {
            configuration.AddScoped<IProductDbOps, PMDbOps>();
            configuration.AddScoped<ICartDbOps, CartDbOps>();
            configuration.AddScoped<IOrderDbOps, OrdersDbOps>();
            configuration.AddScoped<IAuthDbOps, AuthDbOps>();
            ProductManagementDbContext ProductDbContext = new ProductManagementDbContext();
            ProductDbContext.Database.Migrate();
            return configuration;
        }
    }
}
