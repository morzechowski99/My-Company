using Microsoft.AspNetCore.Identity;
using My_Company.Data;
using My_Company.Helpers;
using My_Company.Interfaces;
using My_Company.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.InitializeDb
{
    public static class DbInitializer
    {
        public static async Task Initialize(IRepositoryWrapper repository, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            if (!repository.EnsureCreated()) return;

            var adminRole = new AppRole();
            adminRole.Name = Constants.Roles.MainAdministrator;

            var employee = new AppRole();
            employee.Name = Constants.Roles.WarehouseEmployee;

            var shopUser = new AppRole();
            shopUser.Name = Constants.Roles.ShopUser;

            await roleManager.CreateAsync(adminRole);
            await roleManager.CreateAsync(employee);
            await roleManager.CreateAsync(shopUser);

            var AdminUser = new AppUser()
            {
                Email = "admin@admin.pl",
                UserName = "admin@admin.pl",
                Name = "Admin",
                Surname = "Admin",
                EmailConfirmed = true
            };

            string pass = "Admin1234%";

            IdentityResult checkUser = await userManager.CreateAsync(AdminUser, pass);

            if (checkUser.Succeeded)
            {
                await userManager.AddToRoleAsync(AdminUser, Constants.Roles.MainAdministrator);
            }

            var vatRates = new List<VATRate>() {
                new VATRate { Rate = 7 },
                new VATRate { Rate = 23 },
                new VATRate { Rate = 0 }
            };

            foreach (var rate in vatRates)
            {
                repository.VATRatesRepository.Create(rate);
            }

            await repository.Save();
        }
    }
}
