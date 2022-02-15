//Program powstał na Wydziale Informatyki Politechniki Białostockiej
using My_Company.Areas.Warehouse.EnumTypes;
using My_Company.Areas.Warehouse.ViewModels;
using My_Company.EnumTypes;
using My_Company.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
        IQueryable<Order> GetOrdersByFilters(OrdersListFilters filters);
        Task<List<Guid>> GetNumbersByQuery(string query);
        Task<Order> GetOrderById(Guid? id);
        Task<Order> GetOrderWithPaymentAndUser(Guid id);
        Task<PaymentMethodEnum?> GetOrderPaymentTypeByOrderId(Guid orderId);
        Task<List<Order>> GetOrdersByUserId(string userId);
        Task<Order> CheckUserHasNotEndedPacking(string userId);
        Task<List<Order>> GetOrdersToPacking();
        Task<Order> GetOrderToPackingById(Guid value);
        Task<Order> GetOrderToDocumentsById(Guid? id);
        Task<string> GetInvoiceNumber();
        Task<string> GetWZNumber();
        Task<List<ChartItem>> GetDataToChart(ChartEnums.ChartRange range);
    }
}
