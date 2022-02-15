//Program powstał na Wydziale Informatyki Politechniki Białostockiej
using Microsoft.AspNetCore.Identity;
using My_Company.Helpers;
using My_Company.Interfaces;
using My_Company.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace My_Company.InitializeDb
{
    public static class DbInitializer
    {
        public static async Task Initialize(IRepositoryWrapper repository, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager,
            IConfig config)
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
                new VATRate { Rate = 5 },
                new VATRate { Rate = 0 }
            };

            foreach (var rate in vatRates)
            {
                repository.VATRatesRepository.Create(rate);
            }

            await repository.Save();

            var configRepo = repository.ConfigRepository;
            configRepo.Create(new Config { Id = Constants.AVAILABLE_PAYMENT_METHODS });
            configRepo.Create(new Config { Id = Constants.AVAILABLE_PICKING_METHODS });
            configRepo.Create(new Config { Id = Constants.ConfigKeys.CartSubtitle });
            configRepo.Create(new Config { Id = Constants.ConfigKeys.DataToPayment });
            configRepo.Create(new Config { Id = Constants.ConfigKeys.Description });
            configRepo.Create(new Config { Id = Constants.ConfigKeys.DocumentAddress });
            configRepo.Create(new Config { Id = Constants.ConfigKeys.DotPayKeys.Id });
            configRepo.Create(new Config { Id = Constants.ConfigKeys.DotPayKeys.Pin });
            configRepo.Create(new Config { Id = Constants.ConfigKeys.IsShopEnabled });
            configRepo.Create(new Config { Id = Constants.ConfigKeys.Keywords });
            configRepo.Create(new Config { Id = Constants.ConfigKeys.LogoPath });
            configRepo.Create(new Config { Id = Constants.ConfigKeys.MainPageContent });
            configRepo.Create(new Config { Id = Constants.ConfigKeys.OrderConfirmText });
            configRepo.Create(new Config { Id = Constants.ConfigKeys.PersonalPickupAddress });
            configRepo.Create(new Config { Id = Constants.ConfigKeys.Title });

            await repository.Save();
            repository.ClearTracked();

            await config.SetDataToPayment(new Models.Configuration.DataToPayment
            {
                CompanyName = "",
                AccountNumber = "",
                BankName = ""
            }, repository.ConfigRepository);

            await config.SetDocumentAddress(new Services.DocumentGeneratorService.Models.AddressData
            {
                Name = "",
                Address1 = "",
                Address2 = "",
                NIP = "",
                DocumentPlace = ""
            }, repository.ConfigRepository);

            await config.SetIsShopEnabled(false, repository.ConfigRepository);

            await config.SetMainPageContent(new List<Models.Configuration.MainPageItem>(), repository.ConfigRepository);

            await config.SetPaymentsMethods(new List<Models.Configuration.PaymentMethod>(), repository.ConfigRepository);

            await config.SetPersonalPickupAddress(new Models.Configuration.PersonalPickupAddress
            {
                City = "",
                Street = "",
                ZipCode = "",
                PhoneNumber = ""
            }, repository.ConfigRepository);

            await config.SetPickingMethods(new List<Models.Configuration.PickingMethod>(), repository.ConfigRepository);

            await config.SetValue(Constants.ConfigKeys.CartSubtitle, "", repository.ConfigRepository);
            await config.SetValue(Constants.ConfigKeys.Description, "", repository.ConfigRepository);
            await config.SetValue(Constants.ConfigKeys.DotPayKeys.Id, "", repository.ConfigRepository);
            await config.SetValue(Constants.ConfigKeys.DotPayKeys.Pin, "", repository.ConfigRepository);
            await config.SetValue(Constants.ConfigKeys.Keywords, "", repository.ConfigRepository);
            await config.SetValue(Constants.ConfigKeys.LogoPath, "", repository.ConfigRepository);
            await config.SetValue(Constants.ConfigKeys.OrderConfirmText, "", repository.ConfigRepository);
            await config.SetValue(Constants.ConfigKeys.Title, "", repository.ConfigRepository);
        }
    }
}
