using Microsoft.EntityFrameworkCore;
using My_Company.Areas.Warehouse.EnumTypes;
using My_Company.Areas.Warehouse.ViewModels;
using My_Company.Data;
using My_Company.EnumTypes;
using My_Company.Interfaces;
using My_Company.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.Repositories
{
    public class OrdersRepository : RepositoryBase<Order>, IOrdersRepository
    {
        public OrdersRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<bool> CheckIfProductIsInOrder(int productId, Guid orderId)
        {
            return await FindByCondition(o => o.ProductOrders.Any(po => po.ProductId == productId) && o.Id == orderId)
                .AnyAsync();
        }

        public async Task<Order> CheckUserHasNotEndedPicking(string userId)
        {
            return await FindByCondition(o => o.Status < OrderStatus.Completed
            && o.Picking != null
            && o.Picking.UserId == userId
            && o.Picking.End == null)
            .FirstOrDefaultAsync();
        }

        public async Task<Order> GetOrderToCompleteById(Guid id)
        {
            return await FindByCondition(o => o.Id == id
                && o.Paid == true
                && o.Status <= OrderStatus.Paid)
                .Include(o => o.ProductOrders)
                .ThenInclude(po => po.Product.ProductSectors)
                .ThenInclude(ps => ps.Sector.Row)
                .Include(o => o.ProductOrders)
                .ThenInclude(po => po.Product.Photos)
                .Include(o => o.Picking)
                .ThenInclude(p => p.PickingItems)
                .ThenInclude(pi => pi.ProductOrder.Product)
                .Include(o => o.Picking)
                .ThenInclude(p => p.PickingItems)
                .ThenInclude(pi => pi.Sector.Row)
                .FirstOrDefaultAsync();
        }

        public async Task<Order> GetOrderWithProductsAndPicking(Guid orderId)
        {
            return await FindByCondition(o => o.Id == orderId)
                .Include(o => o.ProductOrders)
                .ThenInclude(po => po.Product.ProductSectors)
                .Include(o => o.Picking)
                .ThenInclude(p => p.PickingItems)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetProducts(Guid orderId)
        {
            var order = (await FindByCondition(o => o.Id == orderId)
                .Include(o => o.ProductOrders)
                .ThenInclude(po => po.Product)
                .FirstOrDefaultAsync());

            if (order != null)
            {
                return order.ProductOrders.Select(po => po.Product);
            }

            return null;
        }

        public async Task<Order> GetOrderWithProductsInfoById(Guid id)
        {
            return await FindByCondition(o => o.Id == id)
                .Include(o => o.ProductOrders)
                .ThenInclude(po => po.Product.ProductSectors)
                .ThenInclude(ps => ps.Sector.Row)
                .Include(o => o.ProductOrders)
                .ThenInclude(po => po.Product.Photos)
                .Include(o => o.Picking)
                .ThenInclude(p => p.PickingItems)
                .ThenInclude(pi => pi.ProductOrder.Product)
                .Include(o => o.Picking)
                .ThenInclude(p => p.PickingItems)
                .ThenInclude(pi => pi.Sector.Row)
                .FirstOrDefaultAsync();
        }

        public IQueryable<Order> GetOrdersByFilters(OrdersListFilters filters)
        {
            IQueryable<Order> query = FindAll();

            if (filters.OrderId != null)
                query = query.Where(o => o.Id == filters.OrderId);
            if (filters.DateFrom != null)
                query = query.Where(o => o.OrderDate >= filters.DateFrom);
            if (filters.DateTo != null)
                query = query.Where(o => o.OrderDate <= filters.DateTo);
            if (filters.Statuses != null && filters.Statuses.Any())
                query = query.Where(o => filters.Statuses.Contains(o.Status));

            query = filters.SortOrder switch
            {
                var sort when sort == OrdersSortOrderEnum.Default => query,
                var sort when sort == OrdersSortOrderEnum.Newest => query.OrderByDescending(o => o.OrderDate),
                var sort when sort == OrdersSortOrderEnum.Oldest => query.OrderBy(o => o.OrderDate),
                var sort when sort == OrdersSortOrderEnum.StatusDESC => query.OrderBy(o => o.Status),
                var sort when sort == OrdersSortOrderEnum.StatusASC => query.OrderByDescending(o => o.Status),
                _ => query
            };
            return query;
        }

        public async Task<List<Guid>> GetNumbersByQuery(string query)
        {
            return await FindByCondition(o => o.Id.ToString()
                .Contains(query))
                .Select(o => o.Id)
                .ToListAsync();
        }

        public async Task<Order> GetOrderById(Guid? id)
        {
            return await FindByCondition(o => o.Id == id)
                .Include(o => o.ProductOrders)
                .ThenInclude(o => o.Product)
                .Include(o => o.User)
                .Include(o => o.Picking.User)
                .Include(o => o.Address)
                .Include(o => o.Delivery)
                .FirstOrDefaultAsync();
        }

        public async Task<Order> GetOrderToDocumentsById(Guid? id)
        {
            return await FindByCondition(o => o.Id == id)
                .Include(o => o.ProductOrders)
                .ThenInclude(o => o.Product)
                .Include(o => o.Address)
                .Include(o => o.Packing)
                .FirstOrDefaultAsync();
        }

        public async Task<Order> GetOrderWithPaymentAndUser(Guid id)
        {
            return await FindByCondition(o => o.Id == id)
                  .Include(o => o.ProductOrders)
                  .Include(o => o.Payment)
                  .Include(o => o.User)
                  .FirstOrDefaultAsync();
        }

        public async Task<PaymentMethodEnum?> GetOrderPaymentTypeByOrderId(Guid orderId)
        {
            return await FindByCondition(o => o.Id == orderId)
                .Select(o => o.PaymentMethod)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Order>> GetOrdersByUserId(string userId)
        {
            if (userId == null)
                return null;

            return await FindByCondition(o => o.UserId == userId).Include(o => o.ProductOrders).ToListAsync();
        }

        public async Task<Order> CheckUserHasNotEndedPacking(string userId)
        {
            return await FindByCondition(o => o.Status == OrderStatus.Completed
           && o.Packing != null
           && o.Packing.UserId == userId
           && o.Packing.PackingEnd == null)
           .FirstOrDefaultAsync();
        }

        public async Task<List<Order>> GetOrdersToPacking()
        {
            return await FindByCondition(o => o.Status == OrderStatus.Completed && o.Packing == null)
                .OrderBy(o => o.OrderDate)
                .Take(50)
                .ToListAsync();
        }

        public async Task<Order> GetOrderToPackingById(Guid id)
        {
            return await FindByCondition(o => o.Id == id
               && o.Status == OrderStatus.Completed)
               .Include(o => o.ProductOrders)
               .ThenInclude(po => po.Product)
               .ThenInclude(pr => pr.Photos.Where(ph => ph.IsListPhoto))
               .Include(o => o.Packing)
               .FirstOrDefaultAsync();
        }

        public async Task<string> GetInvoiceNumber()
        {
            var now = DateTime.Now;
            var count = await FindByCondition(o => o.OrderDate.Year == now.Year && o.OrderDate.Month == now.Month).CountAsync();
            string number = ("0000" + (count + 1))[^4..];
            return $"FV {number}/{now.Month}/{now.Year}";
        }

        public async Task<string> GetWZNumber()
        {
            var now = DateTime.Now;
            var count = await FindByCondition(o => o.Packing.PackingEnd.Value.Year == now.Year && o.Packing.PackingEnd.Value.Month == now.Month && o.WZNumber != null).CountAsync();
            string number = ("0000" + (count + 1))[^4..];
            return $"WZ {number}/{now.Month}/{now.Year}";
        }

        public async Task<List<ChartItem>> GetDataToChart(ChartEnums.ChartRange range)
        {
            var now = DateTime.Now.Date;
            List<ChartItem> chartItems = new();
            switch (range)
            {
                case ChartEnums.ChartRange.Week:
                    {
                        DateTime firstDay = now.AddDays(-4);
                        IEnumerable<IGrouping<DayOfWeek, Order>> items = (await FindByCondition(o => o.OrderDate >= firstDay).ToListAsync()).GroupBy(o => o.OrderDate.DayOfWeek);
                        for (int i = 0; i < 5; i++)
                        {
                            var dayOfWeek = (int)(firstDay.DayOfWeek + i) % 7;
                            var item = items.FirstOrDefault(i => (int)i.Key == dayOfWeek);
                            chartItems.Add(new ChartItem { Index = dayOfWeek, Value = item == null ? 0 : item.Count() });
                        }
                        break;
                    }
                case ChartEnums.ChartRange.Month:
                    {
                        DateTime firstMonth = now.AddMonths(-4);
                        IEnumerable<IGrouping<int, Order>> items = (await FindByCondition(o => o.OrderDate >= firstMonth).ToListAsync()).GroupBy(o => o.OrderDate.Month);
                        for (int i = 0; i < 5; i++)
                        {
                            var month = (((int)(firstMonth.Month + i) - 1) % 12) + 1;
                            var item = items.FirstOrDefault(i => (int)i.Key == month);
                            chartItems.Add(new ChartItem { Index = month, Value = item == null ? 0 : item.Count() });
                        }
                        break;
                    }
                case ChartEnums.ChartRange.Year:
                    {
                        DateTime firstYear = now.AddYears(-4);
                        IEnumerable<IGrouping<int, Order>> items = (await FindByCondition(o => o.OrderDate >= firstYear).ToListAsync()).GroupBy(o => o.OrderDate.Year);
                        for (int i = 0; i < 5; i++)
                        {
                            var year = (int)firstYear.Year + i;
                            var item = items.FirstOrDefault(i => (int)i.Key == year);
                            chartItems.Add(new ChartItem { Index = year, Value = item == null ? 0 : item.Count() });
                        }
                        break;
                    }
            }

            return chartItems;
        }
    }
}
