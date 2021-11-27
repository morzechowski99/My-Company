using My_Company.Areas.Shop.ViewModels.Cart;
using My_Company.Areas.Shop.ViewModels.Order;
using My_Company.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace My_Company.Interfaces
{
    public interface IOrdersService
    {
        Task<Order> CreateOrder(NewOrderModel orderModel, List<CartCookieItem> cart, string userId);
        Task<Order> GetOrderWithPaymentById(Guid id);
    }
}