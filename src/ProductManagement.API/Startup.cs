using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using ProductManagement.BL.ServiceConfigurations;
using ProductManagement.DAL;
using System;
using System.Security.Claims;
using System.Text;

namespace ProductManagement.API
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration;
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddInternalServices();
            services.AddDbContext<ProductManagementDbContext>(opts => opts.UseSqlServer(Configuration.GetConnectionString("ProductManagementDbCS")));
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.RequireHttpsMetadata = false;
                        options.SaveToken = true;
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = Configuration["Jwt:Issuer"],
                            ValidAudience = Configuration["Jwt:Audience"],                            
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:SecretKey"]))
                        };
                    });
            services.AddAuthorization(config =>
                    {
                        config.AddPolicy("Admin",
                            policy => policy.RequireClaim(ClaimTypes.Role, new string[] { "Admin" }));
                        config.AddPolicy("User",
                            policy => policy.RequireClaim(ClaimTypes.Role, new string[] { "User" }));
                    });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,ILoggerFactory loggerFactory)
        {
            loggerFactory.AddFile("Logs/ProductAPILogs-{Date}.txt", isJson: true); 
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ProductManagementDbContext>();
                db.Database.Migrate();
            }
            if (env.IsDevelopment())
            {
                app.UseExceptionHandlerMiddleware();
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
