using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using ProductManagement.Entities;
using System.IO;

namespace ProductManagement.DAL
{
    public class ProductManagementDbContext : DbContext
    {
        public ProductManagementDbContext(DbContextOptions<ProductManagementDbContext> options) : base(options)
        {
        }

        public DbSet<Products> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<Cart> Cart { get; set; }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
        //        .AddJsonFile(@Directory.GetCurrentDirectory() + "/appsettings.json").Build();
        //    DbContextOptionsBuilder<ProductManagementDbContext> builder = new DbContextOptionsBuilder<ProductManagementDbContext>();
        //    string connectionString = configuration.GetConnectionString("ProductManagementDbCS");
        //    optionsBuilder.UseSqlServer(connectionString);
        //}
    }
}
