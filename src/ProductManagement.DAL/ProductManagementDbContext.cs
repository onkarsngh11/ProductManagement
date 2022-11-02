using Microsoft.EntityFrameworkCore;
using ProductManagement.Entities;

namespace ProductManagement.DAL
{
    public class ProductManagementDbContext : DbContext
    {
        public ProductManagementDbContext(DbContextOptions<ProductManagementDbContext> options) : base(options)
        {

        }

        public ProductManagementDbContext()
        {
                
        }

        public DbSet<Products> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<Cart> Cart { get; set; }
    }
}
