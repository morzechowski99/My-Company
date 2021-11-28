using Hangfire;
using Hangfire.Dashboard;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using My_Company.Data;
using My_Company.Filters;
using My_Company.Helpers;
using My_Company.Interfaces;
using My_Company.Jobs.EmialSenderJob;
using My_Company.Models;
using My_Company.Models.AppSettings;
using My_Company.Repositories;
using My_Company.Services;
using My_Company.Validation;
using System;

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
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection"));
                options.EnableSensitiveDataLogging();
            });
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDefaultIdentity<AppUser>(options =>
            {
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
                opt.AddPolicy(Constants.AuthorizationPolicies.ShopAccountPolicy,
                    o => o.RequireRole(new string[] { Constants.Roles.MainAdministrator, Constants.Roles.ShopUser }));
            });

            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<IUsersService, UsersService>();

            services.AddControllersWithViews(o =>
            {
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
            services.AddScoped<DotPayIpFilter>();

            services.AddSingleton<IConfig>(new Services.Config());
            services.AddSingleton<IEmailQueue>(new EmailQueue());

            services.AddTransient<IParcelLockersService, ParcelLockersService>();
            services.AddTransient<IEmailSenderJob, EmailSenderJob>();
            services.AddTransient<IOrdersService, OrdersService>();
            services.AddTransient<IEmailService, EmailService>();

            services.AddSingleton<Microsoft.AspNetCore.Http.IHttpContextAccessor, Microsoft.AspNetCore.Http.HttpContextAccessor>();

            services.Configure<DotPayOptions>(Configuration.GetSection("DotPay"));

            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                 .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                 .UseSqlServerStorage(Configuration.GetConnectionString("DefaultConnection")
                 , new SqlServerStorageOptions
                 {
                     CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                     SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                     QueuePollInterval = TimeSpan.Zero,
                     UseRecommendedIsolationLevel = true,
                     DisableGlobalLocks = true
                 }));

            services.AddHangfireServer();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,IRecurringJobManager recurringJobManager, IEmailSenderJob emailSenderJob)
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

            recurringJobManager.AddOrUpdate("sendEmail",
                () => emailSenderJob.SendEmails(),
                Configuration.GetValue<string>("emailCron"));

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();


            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[] { new HangFireFilter() },
                IsReadOnlyFunc = (DashboardContext context) => true
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHangfireDashboard();

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
