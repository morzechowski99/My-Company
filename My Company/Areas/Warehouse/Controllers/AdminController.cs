using AutoMapper;
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
        private readonly IMapper mapper;

        public AdminController(IConfig config, IRepositoryWrapper repositoryWrapper, IFilesService filesService, IMapper mapper)
        {
            this.config = config;
            this.repositoryWrapper = repositoryWrapper;
            this.filesService = filesService;
            this.mapper = mapper;
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
            {
                await config.SetIsShopEnabled(!shopEnabled, repositoryWrapper.ConfigRepository);
            }

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
                await config.SetPersonalPickupAddress(pickingMethodsDto.Addres, repositoryWrapper.ConfigRepository);
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

        [HttpPost]
        public async Task<IActionResult> AddMainPageItem(MainPageItem newItem, IFormFile photo)
        {
            if (newItem == null || photo == null || !ModelState.IsValid)
                return BadRequest();

            var configRepository = repositoryWrapper.ConfigRepository;
            var mainpageItems = await config.GetMainPageContent(configRepository);
            if (mainpageItems == null)
                mainpageItems = new List<MainPageItem>();

            newItem.PhotoUrl = await filesService.UploadFile(photo);
            newItem.Order = mainpageItems.Count + 1;
            newItem.CategoryId = newItem.CategoryId == -1 ? null : newItem.CategoryId;
            mainpageItems.Add(newItem);
            await config.SetMainPageContent(mainpageItems, configRepository);

            return ViewComponent("MainPageForm");

        }

        [HttpDelete]
        public async Task<IActionResult> DeleteMainPageItem(int? order)
        {
            if (order == null)
                return BadRequest();

            var configRepository = repositoryWrapper.ConfigRepository;
            var mainpageItems = await config.GetMainPageContent(configRepository);
            if (mainpageItems == null)
                return NotFound();

            var deletedItem = mainpageItems.FirstOrDefault(i => i.Order == order);
            if (deletedItem == null)
                return BadRequest();
            if (deletedItem.PhotoUrl != Constants.ImagePlaceholder)
                filesService.DeletePhoto(deletedItem.PhotoUrl);
            mainpageItems = mainpageItems.Where(i => i.Order != order).OrderBy(i => i.Order).ToList();
            for (int i = 0; i < mainpageItems.Count; i++)
            {
                mainpageItems[i].Order = i + 1;
            }
            await config.SetMainPageContent(mainpageItems, configRepository);

            return ViewComponent("MainPageForm");

        }

        [HttpPut]
        public async Task<IActionResult> MoveMainPageItem(int? order, MoveDirection? direction)
        {

            if (order == null || direction == null)
                return BadRequest();

            var configRepository = repositoryWrapper.ConfigRepository;
            var mainpageItems = await config.GetMainPageContent(configRepository);
            if (mainpageItems == null)
                return NotFound();

            var movingItem = mainpageItems.FirstOrDefault(i => i.Order == order);
            var secondItem = mainpageItems.FirstOrDefault(i => i.Order == order + (int)direction.Value);
            if (movingItem == null || secondItem == null)
                return BadRequest();
            var tempOrder = movingItem.Order;
            movingItem.Order = secondItem.Order;
            secondItem.Order = tempOrder;
            await config.SetMainPageContent(mainpageItems, configRepository);

            return ViewComponent("MainPageForm");

        }

        public enum MoveDirection
        {
            Up = -1,
            Down = 1
        }

        [HttpPut]
        public async Task<IActionResult> EditMainPageItemPhoto(EditMainPagePhotoViewModel photoViewModel)
        {

            if (photoViewModel == null || !ModelState.IsValid)
                return BadRequest();

            var configRepository = repositoryWrapper.ConfigRepository;
            var mainpageItems = await config.GetMainPageContent(configRepository);
            if (mainpageItems == null)
                return NotFound();

            var editingItem = mainpageItems.FirstOrDefault(i => i.Order == photoViewModel.Order);
            if (editingItem == null)
                return NotFound();
            try
            {
                filesService.DeletePhoto(editingItem.PhotoUrl);
            }
            catch
            {

            }
            var path = await filesService.UploadFile(photoViewModel.Photo);
            editingItem.PhotoUrl = path;
            await config.SetMainPageContent(mainpageItems, configRepository);

            return ViewComponent("MainPageForm");
        }

        [HttpPut]
        public async Task<IActionResult> EditMainPageItem(EditMainPageItemViewModel editMainPage)
        {

            if (editMainPage == null || !ModelState.IsValid)
                return BadRequest();

            var configRepository = repositoryWrapper.ConfigRepository;
            var mainpageItems = await config.GetMainPageContent(configRepository);
            if (mainpageItems == null)
                return NotFound();

            var editingItem = mainpageItems.FirstOrDefault(i => i.Order == editMainPage.Order);
            if (editingItem == null)
                return NotFound();

            mainpageItems.Remove(editingItem);
            editingItem = mapper.Map(editMainPage, editingItem);
            mainpageItems.Add(editingItem);
            await config.SetMainPageContent(mainpageItems, configRepository);

            return ViewComponent("MainPageForm");
        }
    }
}
