using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using My_Company.Areas.Warehouse.ViewModels;
using My_Company.EnumTypes;
using My_Company.Helpers;
using My_Company.Interfaces;
using My_Company.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace My_Company.Areas.Warehouse.Controllers
{
    [Area("Warehouse")]
    [Authorize(Constants.AuthorizationPolicies.WarehousePolicy)]
    public class OrdersController : Controller
    {
        private readonly IRepositoryWrapper repositoryWrapper;
        private readonly IMapper mapper;

        public OrdersController(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            this.repositoryWrapper = repositoryWrapper;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var orderWithCompletionInProgress = await repositoryWrapper.OrdersRepository.CheckUserHasNotEndedPicking(GetUserId());
            if (orderWithCompletionInProgress != null)
            {
                var list = new List<OrderListItemViewModel>() { new OrderListItemViewModel
                {
                    Id = orderWithCompletionInProgress.Id,
                    OrderDate=orderWithCompletionInProgress.OrderDate,
                    PickingStarted= true
                }};
                return View(list);
            }

            var ordersToComplete = (await repositoryWrapper.OrdersToCompleteView.GetAll())
                .OrderBy(o => o.OrderDate)
                .Take(50);
            var dtos = mapper.Map<List<OrderListItemViewModel>>(ordersToComplete);
            return View(dtos);
        }

        public async Task<IActionResult> Complete(Guid? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var userId = GetUserId();
            var order = await repositoryWrapper.OrdersRepository.GetOrderToCompleteById(id.Value);
            if (order == null)
            {
                TempData["Error"] = "Zamówienie nie istnieje, lub nie można go skompletować";
                return RedirectToAction(nameof(Index));
            }
            else if (order.Picking != null && order.Picking.UserId != GetUserId())
            {
                TempData["Error"] = "Ktoś inny kompletuje już to zamówienie";
                return RedirectToAction(nameof(Index));
            }
            string message;
            if (!OrderHelpers.CheckIfAllProductsAreAvailable(order.ProductOrders, out message))
            {
                TempData["Error"] = message;
                return RedirectToAction(nameof(Index));
            }
            if (order.Picking == null)
            {
                Picking picking = new Picking { Start = DateTime.Now, UserId = userId, OrderId = order.Id };
                order.Picking = picking;
                repositoryWrapper.PickingRepository.Create(picking);
                foreach (var productOrder in order.ProductOrders)
                {
                    var product = await repositoryWrapper.ProductRepository
                        .GetProductWithoutVirtualPropertiesById(productOrder.ProductId);
                    product.MagazineCount -= productOrder.Count;
                    repositoryWrapper.ProductRepository.Update(product);
                }
                await repositoryWrapper.Save();
            }
            var orderView = mapper.Map<OrderPickingViewModel>(order);
            foreach (var item in orderView.Items)
            {
                item.Completed = OrderHelpers.GetCompletedCount(order.Picking.PickingItems, item.ProductOrderId);
                var product = order.ProductOrders
                    .FirstOrDefault(po => po.Id == item.ProductOrderId)
                    .Product;
                var photo = product.Photos.FirstOrDefault(p => p.IsListPhoto);
                var photoUrl = photo == null ? Constants.ImagePlaceholder : photo.Path;
                var productDto = mapper.Map<ProductListItemViewModel>(product);
                productDto.PhotoUrl = photoUrl;
                item.Product = productDto;
            }
            ViewData["rows"] = new SelectList(repositoryWrapper.WarehouseRowRepository.FindAll(), "Id", "RowName");
            ViewBag.Completed = OrderHelpers.CheckOrderCompleted(order);
            return View(orderView);
        }

        private string GetUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }

        [HttpGet]
        public async Task<IActionResult> ValidateProductEan(string ean, Guid? orderId)
        {
            try
            {
                if (ean == null || orderId == null)
                {
                    return BadRequest();
                }

                var product = await repositoryWrapper.ProductRepository.GetProductByEANCode(ean);
                if (product == null)
                {
                    return NotFound("invalid Code");
                }

                if (!await repositoryWrapper.OrdersRepository.Exists(orderId.Value))
                {
                    return NotFound("invalid orderId");
                }

                if (!await repositoryWrapper.OrdersRepository.CheckIfProductIsInOrder(product.Id, orderId.Value))
                {
                    return Ok(new { Error = "product does not exists in order", IsInOrder = false });
                }
                return Ok(new { ProductId = product.Id, IsInOrder = true });

            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error processing request");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetProductsInOrder(Guid? orderId)
        {
            if (orderId == null)
                return BadRequest();

            try
            {
                var products = await repositoryWrapper.OrdersRepository.GetProducts(orderId.Value);
                if (products == null)
                    return NotFound("invalid orderId");
                List<object> list = new();
                foreach (var product in products)
                {
                    list.Add(new { Id = product.Id, Description = $"{product.Name} ({product.EANCode})" });
                }
                return Ok(list);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error processing request");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddProductToPicking(AddProductToPickingViewModel addProductToPickingDto)
        {
            if (addProductToPickingDto == null || !ModelState.IsValid)
                return BadRequest();

            try
            {
                var order = await repositoryWrapper.OrdersRepository.GetOrderWithProductsAndPicking(addProductToPickingDto.OrderId);
                var productOrder = order.ProductOrders.FirstOrDefault(po => po.ProductId == addProductToPickingDto.ProductId);
                if (productOrder == null)
                {
                    return BadRequest("product in order not found");
                }
                var productSector = productOrder.Product.ProductSectors.FirstOrDefault(ps => ps.SectorId == addProductToPickingDto.SectorId);
                if (productSector == null || productSector.Count < addProductToPickingDto.Count)
                {
                    return Ok(new { Error = "too less products in sector" });
                }
                if (!OrderHelpers.ValidateProductCount(productOrder, order.Picking.PickingItems, addProductToPickingDto.Count))
                {
                    return Ok(new { Error = "too many products picked" });
                }
                productSector.Count -= addProductToPickingDto.Count;
                order.Picking.PickingItems.Add(new PickingItem
                {
                    Count = addProductToPickingDto.Count,
                    ProductOrderId = productOrder.Id,
                    SectorId = productSector.SectorId
                });
                ViewBag.Completed = OrderHelpers.CheckOrderCompleted(order);
                repositoryWrapper.OrdersRepository.Update(order);
                await repositoryWrapper.Save();
                return View("CompleteDetailsPartial",order.Id);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error processing request");
            }
        }
        
        [HttpPut]
        public async Task<IActionResult> DeletePickingItem(int? pickingItemId)
        {
            if (pickingItemId ==null )
                return BadRequest();

            try
            {
                var pickingItem = await repositoryWrapper.PickingItemsRepository.GetItemById(pickingItemId.Value);
                if(pickingItem == null)
                    return BadRequest();

                repositoryWrapper.PickingItemsRepository.Delete(pickingItem);

                var productSector = await repositoryWrapper.ProductSectorRepository.GetOne(ps => ps.ProductId == pickingItem.ProductOrder.ProductId && ps.SectorId == pickingItem.SectorId);
                productSector.Count += pickingItem.Count;

                repositoryWrapper.ProductSectorRepository.Update(productSector);
                
                await repositoryWrapper.Save();
                return View("CompleteDetailsPartial",pickingItem.PickingId);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error processing request");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CompletePicking(Guid? orderId)
        {
            if (orderId == null)
                return BadRequest();

            try
            {
                var order = await repositoryWrapper.OrdersRepository.GetOrderWithProductsAndPicking(orderId.Value);
                if (!OrderHelpers.ValidatePicking(order))
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error with products count");
                }
                order.Status = OrderStatus.Completed;
                order.Picking.End = DateTime.Now;
                repositoryWrapper.OrdersRepository.Update(order);
                await repositoryWrapper.Save();
                return Ok();
            }
            catch 
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error processing request");
            }
        }

        [HttpGet]
        [Authorize(Roles =Constants.Roles.MainAdministrator)]
        public IActionResult OrdersList()
        {
            return View();
        }
    }

}
