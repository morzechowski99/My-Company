using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using My_Company.Areas.Warehouse.ViewModels;
using My_Company.Areas.Warehouse.ViewModels.PaymentMethods;
using My_Company.Areas.Warehouse.ViewModels.PickingMethods;
using My_Company.EnumTypes;
using My_Company.Helpers;
using My_Company.Interfaces;
using My_Company.Models.Configuration;
using My_Company.Services.DocumentGeneratorService.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.Areas.Warehouse.Controllers
{
    [Area("Warehouse")]
    [Authorize(Roles = Constants.Roles.MainAdministrator)]
    public class AdminController : Controller
    {
        private readonly IConfig config;
        private readonly IRepositoryWrapper repositoryWrapper;
        private readonly IFilesService filesService;

        public AdminController(IConfig config, IRepositoryWrapper repositoryWrapper, IFilesService filesService)
        {
            this.config = config;
            this.repositoryWrapper = repositoryWrapper;
            this.filesService = filesService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangeBaseInfo(List<ConfigValue> configValues)
        {
            if (configValues == null)
                return BadRequest();

            try
            {
                foreach (var item in configValues)
                {
                    await config.SetValue(item.Key, item.Value, repositoryWrapper.ConfigRepository);
                }
                return Ok();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        [HttpPost]
        public async Task<IActionResult> ChangeContentValues(List<ConfigValue> configValues)
        {
            if (configValues == null)
                return BadRequest();

            try
            {
                foreach (var item in configValues)
                {
                    await config.SetValue(item.Key, item.Value, repositoryWrapper.ConfigRepository);
                }
                return Ok();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<IActionResult> ChangeShopActive()
        {
            var shopEnabled = await config.IsShopEnabled(repositoryWrapper.ConfigRepository);
            if (!shopEnabled && (
                (await config.GetAvailavlePaymentsMethods(repositoryWrapper.ConfigRepository)).Count == 0
                || (await config.GetAvailavlePickingMethods(repositoryWrapper.ConfigRepository)).Count == 0))
            {
                TempData["warning"] = "Nie można włączyć sklepu, ponieważ nie ma zdefiniowanych metod płatności lub wysyłki";
            }
            else
                await config.SetIsShopEnabled(!shopEnabled, repositoryWrapper.ConfigRepository);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> ChangeLogo(IFormFile logo)
        {
            try
            {
                var old = await config.GetValue(Constants.ConfigKeys.LogoPath, repositoryWrapper.ConfigRepository);
                filesService.DeletePhoto(old);
            }
            catch { }
            var path = await filesService.ChangeShopLogo(logo);
            await config.SetValue(Constants.ConfigKeys.LogoPath, path, repositoryWrapper.ConfigRepository);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> ChangePaymentsMethods(PaymentMethodsFormViewModel paymentMethodsDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (paymentMethodsDto.Methods.Any(m => m.Enabled == true && m.Method == PaymentMethodEnum.TraditionalTransfer))
                await config.SetDataToPayment(paymentMethodsDto.DataToPayment, repositoryWrapper.ConfigRepository);
            else
                await config.SetDataToPayment(new DataToPayment(), repositoryWrapper.ConfigRepository);
            if (paymentMethodsDto.Methods.Any(m => m.Enabled == true && m.Method == PaymentMethodEnum.DotPay))
            {
                await config.SetValue(Constants.ConfigKeys.DotPayKeys.Id, paymentMethodsDto.DotPayId.ToString(), repositoryWrapper.ConfigRepository);
                await config.SetValue(Constants.ConfigKeys.DotPayKeys.Pin, paymentMethodsDto.DotPayPin, repositoryWrapper.ConfigRepository);
            }
            else
            {
                await config.SetValue(Constants.ConfigKeys.DotPayKeys.Id, "", repositoryWrapper.ConfigRepository);
                await config.SetValue(Constants.ConfigKeys.DotPayKeys.Pin, "", repositoryWrapper.ConfigRepository);
            }
            var newPaymentsMethods = paymentMethodsDto.Methods.Where(m => m.Enabled).Select(m => new PaymentMethod { Method = m.Method, Price = (int)(decimal.Parse(m.Price) * 100) }).ToList();
            if (newPaymentsMethods.Count == 0)
            {
                await config.SetIsShopEnabled(false, repositoryWrapper.ConfigRepository);
                TempData["warning"] = "Sklep został tymczasowo wyłączony, ponieważ nie zdefiniowano żadnej metody płatności";
            }
            await config.SetPaymentsMethods(newPaymentsMethods, repositoryWrapper.ConfigRepository);

            return RedirectToAction(nameof(Index));

        }
        
        [HttpPost]
        public async Task<IActionResult> ChangePickingMethods(PickingMethodsFormViewModel pickingMethodsDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (pickingMethodsDto.Methods.Any(m => m.Enabled == true && m.Type == DeliveryType.PersonalPickup))
                await config.SetPersonalPickupAddress(pickingMethodsDto.Addres,repositoryWrapper.ConfigRepository);
            else
                await config.SetPersonalPickupAddress(new PersonalPickupAddress(), repositoryWrapper.ConfigRepository);
            var newPickingMethods = pickingMethodsDto.Methods.Where(m => m.Enabled).Select(m => new PickingMethod { Type = m.Type, Price = (int)(decimal.Parse(m.Price) * 100) }).ToList();
            if (newPickingMethods.Count == 0)
            {
                await config.SetIsShopEnabled(false, repositoryWrapper.ConfigRepository);
                TempData["warning"] = "Sklep został tymczasowo wyłączony, ponieważ nie zdefiniowano żadnej metody płatności";
            }
            await config.SetPickingMethods(newPickingMethods, repositoryWrapper.ConfigRepository);

            return RedirectToAction(nameof(Index));

        }
        
        [HttpPost]
        public async Task<IActionResult> ChangeDocumentAddressData(AddressData newAddress)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            await config.SetDocumentAddress(newAddress, repositoryWrapper.ConfigRepository);

            return Ok();

        }

    }



}
