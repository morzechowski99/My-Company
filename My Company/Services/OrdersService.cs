using AutoMapper;
using Microsoft.EntityFrameworkCore;
using My_Company.Areas.Shop.ViewModels.Cart;
using My_Company.Areas.Shop.ViewModels.Order;
using My_Company.EnumTypes;
using My_Company.Extensions;
using My_Company.Interfaces;
using My_Company.Models;
using My_Company.Services.DeliveryService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.Services
{
    public class OrdersService : IOrdersService
    {
        private readonly IRepositoryWrapper repositoryWrapper;
        private readonly IMapper mapper;
        private readonly IConfig config;
        private readonly IParcelLockersService parcelLockersService;

        public OrdersService(IRepositoryWrapper repositoryWrapper, IMapper mapper, IConfig config,IParcelLockersService parcelLockersService)
        {
            this.repositoryWrapper = repositoryWrapper;
            this.mapper = mapper;
            this.config = config;
            this.parcelLockersService = parcelLockersService;
        }

        public async Task<Order> CreateOrder(NewOrderModel orderModel, List<CartCookieItem> cart, string userId)
        {
            try
            {
                Order order = mapper.Map<Order>(orderModel);
                order.UserId = userId;

                #region Address
                if (order.UserId != null)
                {
                    order.Email = null;
                    if (order.AddressId != 0)
                    {
                        order.Address.Id = order.AddressId;
                        order.Address.UserId = order.UserId;
                        repositoryWrapper.AddressesRepository.Update(order.Address);
                        order.Address = null;
                    }
                    else
                    {
                        order.Address.UserId = userId;
                    }
                }
                #endregion
                order.OrderDate = DateTime.Now;
                #region Products
                var products = await repositoryWrapper.ProductRepository.GetProductsByIds(cart.Select(c => c.Id).ToList());
                foreach (var product in products)
                {
                    var quantity = cart.First(ci => ci.Id == product.Id).Quantity;
                    order.ProductOrders.Add(new()
                    {
                        ProductId = product.Id,
                        ProductPrice = product.NettoPrice,
                        ProductVatRate = product.VATRate.Rate,
                        Count = quantity
                    });
                    product.Demand += quantity;
                    product.VATRate = null;
                    repositoryWrapper.ProductRepository.Update(product);
                }
                #endregion
                order.Delivery = GetDelivery(orderModel);
                order.Payment = new Payment();
                order.DeliveryPrice = await GetDeliveryPrice(order);
                order.PaymentPrice = await GetPaymentPrice(order);
                repositoryWrapper.OrdersRepository.Create(order);
                await repositoryWrapper.Save();
                return order;
            }
            catch
            {
                return null;
            }

        }

        private async Task<int> GetDeliveryPrice(Order order)
        {
            var availableDeliveries = await config.GetAvailavlePickingMethods(repositoryWrapper.ConfigRepository);

            return availableDeliveries.FirstOrDefault(x => x.Type == order.DeliveryType).Price;
        }

        private async Task<int> GetPaymentPrice(Order order)
        {
            var availablePayments = await config.GetAvailavlePaymentsMethods(repositoryWrapper.ConfigRepository);

            return availablePayments.FirstOrDefault(x => x.Method == order.PaymentMethod).Price;
        }

        public async Task<Order> GetOrderWithPaymentAndUserById(Guid id)
        {
            return await repositoryWrapper.OrdersRepository.GetOrderWithPaymentAndUser(id);
        }

        private OrderDelivery GetDelivery(NewOrderModel order)
        {
            IDeliveryService deliveryService = order.DeliveryType.GetService();

            return deliveryService.GetDelivery(order);
        }

        public async Task<OrderDefailsViewModel> GetOrderByIdAndUser(Guid orderId, string userId)
        {
            var order = await repositoryWrapper.OrdersRepository
                .FindByCondition(o => o.Id == orderId && o.UserId == userId)
                .Include(o => o.ProductOrders)
                .ThenInclude(o => o.Product)
                .ThenInclude(p => p.Photos.Where(ph => ph.IsListPhoto))
                .Include(o => o.Delivery)
                .Include(o => o.Payment)
                .Include(o => o.Address)
                .FirstOrDefaultAsync();

            if (order == null)
                return null;

            var orderModel = mapper.Map<OrderDefailsViewModel>(order);
            orderModel.Products.ForEach(p => p.Price = p.OneItemPrice * p.Quantity);
            if(order.DeliveryType == DeliveryType.PaczkomatyInPost)
            {
                orderModel.Delivery.ParcelLockerInfo = await parcelLockersService.GetParcelLockerInfo((order.Delivery as InPostDelivery).PackLockerName);
            }

            return orderModel;
        }
    }
}
