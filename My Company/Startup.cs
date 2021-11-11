using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using My_Company.Data;
using My_Company.Helpers;
using My_Company.Interfaces;
using My_Company.Models;
using My_Company.Repositories;
using My_Company.Services;
using My_Company.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company
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
            services.AddDbContext<ApplicationDbContext>(options => {
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection"));
                options.EnableSensitiveDataLogging();
            });
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDefaultIdentity<AppUser>(options => {
                options.SignIn.RequireConfirmedAccount = true;
                options.User.RequireUniqueEmail = true;
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 -._@+/";
                })
                .AddRoles<AppRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddAuthorization(opt =>
            {
                opt.AddPolicy(Constants.AuthorizationPolicies.WarehousePolicy,
                    o => o.RequireRole(new string[] { Constants.Roles.MainAdministrator, Constants.Roles.WarehouseEmployee }));
            });

            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<IUsersService, UsersService>();

            services.AddControllersWithViews(o => {
                o.ModelMetadataDetailsProviders.Add(
            new CustomValidationMetadataProvider(
                "My_Company.Validation.Validation",
                typeof(Validation.Validation)));
            })
                .AddNewtonsoftJson(opt => 
            opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddAutoMapper(typeof(Startup));
            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
            services.AddScoped<IFilesService, LocalFilesService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapAreaControllerRoute(
                    name: "Warehouse",
                    areaName: "Warehouse",
                    pattern: "Warehouse/{controller=Home}/{action=Index}/{id?}"
                );

                endpoints.MapAreaControllerRoute(
                    name: "Shop",
                    areaName: "Shop",
                    pattern: "/{controller=Home}/{action=Index}/{id?}"
                );

                endpoints.MapRazorPages();
            });
        }
    }
}
