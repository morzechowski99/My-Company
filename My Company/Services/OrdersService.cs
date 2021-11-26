using AutoMapper;
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

        public OrdersService(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            this.repositoryWrapper = repositoryWrapper;
            this.mapper = mapper;
        }

        public async Task<bool> CreateOrder(NewOrderModel orderModel, List<CartCookieItem> cart, string userId)
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
                repositoryWrapper.OrdersRepository.Create(order);
                await repositoryWrapper.Save();
                return true;
            }
            catch
            {
                return false;
            }

        }

        private OrderDelivery GetDelivery(NewOrderModel order)
        {
            IDeliveryService deliveryService = order.DeliveryType.GetService();

            return deliveryService.GetDelivery(order);
        }
    }
}
