using AutoMapper;
using My_Company.Areas.Shop.ViewModels.Cart;
using My_Company.Areas.Shop.ViewModels.Order;
using My_Company.EnumTypes;
using My_Company.Extensions;
using My_Company.Interfaces;
using My_Company.Models;
using My_Company.Services.DeliveryService;
using My_Company.Services.PaymentService;
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

        public OrdersService(IRepositoryWrapper repositoryWrapper, IMapper mapper, IConfig config)
        {
            this.repositoryWrapper = repositoryWrapper;
            this.mapper = mapper;
            this.config = config;
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
                    else order.Address.UserId = userId;
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
    }
}
