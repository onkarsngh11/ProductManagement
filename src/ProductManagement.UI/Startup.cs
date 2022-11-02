using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ProductManagement.Interfaces;
using ProductManagement.UI.Controllers;
using ProductManagement.UI.Helpers;
using ProductManagement.UI.Middlewares;
using System;

namespace ProductManagement.UI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddMvc(options => options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute()));
            services.AddScoped<CartController, CartController>();
            services.AddSingleton<IHttpClientHelper, HttpClientHelper>();
            services.AddHttpContextAccessor(); 
            services.AddHttpClient("PMApis",c=> c.BaseAddress = new Uri(Configuration.GetSection("ApiUrls")["BaseAddress"].ToString()));
            services.AddAuthentication(config =>
                    {
                        config.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                        config.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                        config.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    })
                    .AddCookie(options =>
                    {
                        options.LoginPath = "/Auth/Login";
                        options.ReturnUrlParameter = "/Home/Index";
                        options.LogoutPath = "/Auth/Logout";
                        options.AccessDeniedPath = "/Home/Error";
                        options.Cookie.HttpOnly = true;
                        options.Cookie.Name = "PMCookie";
                        options.ExpireTimeSpan = TimeSpan.FromHours(.2);
                    });
            services.AddAuthorization(options =>
                    {
                        options.AddPolicy("Admin",
                            policy => policy.RequireClaim(System.Security.Claims.ClaimTypes.Role, new string[] { "Admin" }));
                        options.AddPolicy("User",
                            policy => policy.RequireClaim(System.Security.Claims.ClaimTypes.Role, new string[] { "User" }));
                        options.AddPolicy("RegisteredOnly",
                            policy => policy.RequireClaim(System.Security.Claims.ClaimTypes.Role, new string[] { "User","Admin" }));
                    });
        }

        

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddFile("Logs/ProductUILogs-{Date}.txt", isJson: true);
            if (env.IsDevelopment())
            {
                app.UseExceptionHandlerMiddleware();
                app.UseDeveloperExceptionPage();
                app.UseExceptionHandler("/Home/Error");
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseCookiePolicy();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}