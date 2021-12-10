using Microsoft.EntityFrameworkCore;
using My_Company.Areas.Warehouse.EnumTypes;
using My_Company.Areas.Warehouse.ViewModels;
using My_Company.Data;
using My_Company.Interfaces;
using My_Company.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.Repositories
{
    public class DeliveriesRepository : RepositoryBase<Delivery>, IDeliveriesRepository
    {
        public DeliveriesRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<string> CreateKPZNumber()
        {
            var now = DateTime.Now;
            var count = await FindByCondition(d => d.DeliveryDate.Year == now.Year && d.DeliveryDate.Month == now.Month && d.IsCorrecting).CountAsync();
            string number = ("0000" + (count + 1))[^4..];
            return $"KPZ/{number}/{now.Month}/{now.Year}";
        }

        public async Task<string> CreatePZNumber()
        {
            var now = DateTime.Now;
            var count = await FindByCondition(d => d.DeliveryDate.Year == now.Year && d.DeliveryDate.Month == now.Month && !d.IsCorrecting).CountAsync();
            string number = ("0000" + (count + 1))[^4..];
            return $"PZ/{number}/{now.Month}/{now.Year}";
        }

        public IQueryable<Delivery> GetDeliveriesByFilters(DeliveriesListFilters filters)
        {
            IQueryable<Delivery> query = FindAll().Include(d => d.Supplier);

            if (filters.DateFrom.HasValue)
            {
                query = query.Where(d => d.DeliveryDate.Date >= filters.DateFrom);
            }
            if (filters.DateTo.HasValue)
            {
                query = query.Where(d => d.DeliveryDate.Date <= filters.DateTo);
            }
            if (filters.SupplierId.HasValue)
            {
                query = query.Where(d => d.SupplierId == filters.SupplierId);
            }
            if (!string.IsNullOrEmpty(filters.PZNumber))
            {
                var pz = filters.PZNumber.ToLower();
                query = query.Where(d => d.PZNumber.ToLower().Contains(pz) || pz.Contains(d.PZNumber.ToLower()));
            }

            query = filters.SortOrder switch
            {
                var order when order == DeliveriesSortOrderEnum.DateASC => query.OrderBy(d => d.DeliveryDate),
                var order when order == DeliveriesSortOrderEnum.DateDESC => query.OrderByDescending(d => d.DeliveryDate),
                var order when order == DeliveriesSortOrderEnum.SupplierASC => query.OrderBy(d => d.Supplier.Name),
                var order when order == DeliveriesSortOrderEnum.SupplierDESC => query.OrderByDescending(d => d.Supplier.Name),
                _ => query,
            };

            return query;
        }

        public async Task<Delivery> GetDeliveryById(int id)
        {
            return await FindByCondition(d => d.Id == id)
                .Include(d => d.Supplier)
                .Include(d => d.ProductDeliveries)
                .ThenInclude(pd => pd.Product.Photos)
                .Include(d => d.ProductDeliveries)
                .ThenInclude(pd => pd.Sector.Row)
                .FirstOrDefaultAsync();
        }

        public async Task<Delivery> GetDeliveryToDocumentById(int id)
        {
            return await FindByCondition(d => d.Id == id)
                .Include(d => d.Supplier)
                .Include(d => d.ProductDeliveries)
                .ThenInclude(pd => pd.Product)
                .Include(d => d.ProductDeliveries)
                .ThenInclude(pd => pd.Sector.Row)
                .FirstOrDefaultAsync();
        }

        public async Task<Delivery> GetDeliveryCorrectedDeliveryById(int id)
        {
            return await FindByCondition(d => d.CorrectingId == id)
                .Include(d => d.ProductDeliveries)
                .FirstOrDefaultAsync();
        }

        public ICollection<ProductDelivery> RemoveDuplicates(List<ProductDelivery> productDeliveries)
        {
            var copy = new List<ProductDelivery>(productDeliveries);
            int i = 0;
            foreach (var item in productDeliveries)
            {
                for (int j = i + 1; j < productDeliveries.Count; j++)
                {
                    var pd = productDeliveries[j];
                    if (pd.ProductId == item.ProductId && pd.SectorId == item.SectorId)
                    {
                        item.Count += pd.Count;
                        copy.Remove(pd);
                    }
                }
                i++;
            }
            return copy;
        }

        public async Task<Delivery> GetOrginalDeliveryToDocumentCorrectingById(int id)
        {
            return await FindByCondition(d => d.CorrectingId == id)
                .Include(d => d.ProductDeliveries)
                .ThenInclude(pd => pd.Product)
                .Include(d => d.ProductDeliveries)
                .ThenInclude(pd => pd.Sector.Row)
                .FirstOrDefaultAsync();
        }
    }
}
