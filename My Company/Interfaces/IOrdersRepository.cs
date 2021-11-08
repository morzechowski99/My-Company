using My_Company.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace My_Company.Interfaces
{
    public interface IOrdersRepository : IRepositoryBase<Order>
    {
        Task<Order> GetOrderToCompleteById(Guid id);
        Task<Order> CheckUserHasNotEndedPicking(string userId);
        Task<bool> CheckIfProductIsInOrder(int productId, Guid orderId);
        Task<IEnumerable<Product>> GetProducts(Guid orderId);
        Task<Order> GetOrderWithProductsAndPicking(Guid orderId);
        Task<Order> GetOrderWithProductsInfoById(Guid id);
    }
}
