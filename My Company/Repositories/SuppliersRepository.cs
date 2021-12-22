using Microsoft.EntityFrameworkCore;
using My_Company.Areas.Warehouse.EnumTypes;
using My_Company.Areas.Warehouse.ViewModels;
using My_Company.Data;
using My_Company.Interfaces;
using My_Company.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.Repositories
{
    public class SuppliersRepository : RepositoryBase<Supplier>, ISuppliersRepository
    {
        public SuppliersRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Supplier> GetById(int id)
        {
            return await FindByCondition(s => s.Id == id).FirstOrDefaultAsync();
        }

        public IQueryable<Supplier> GetSuppliersByFilters(SuppliersListFilters filters)
        {
            var suppliers = FindAll();

            suppliers = Filter(filters, suppliers);

            suppliers = Sort(filters, suppliers);

            return suppliers;
        }

        private IQueryable<Supplier> Sort(SuppliersListFilters filters, IQueryable<Supplier> suppliers)
        {
            switch (filters.SortOrder)
            {
                case SuppliersListSortOrderEnum.NameASC:
                suppliers = suppliers.OrderBy(s => s.Name);
                break;
                case SuppliersListSortOrderEnum.NameDESC:
                suppliers = suppliers.OrderByDescending(s => s.Name);
                break;
                case SuppliersListSortOrderEnum.NIPDESC:
                suppliers = suppliers.OrderByDescending(s => s.NIP);
                break;
                case SuppliersListSortOrderEnum.NIPASC:
                suppliers = suppliers.OrderBy(s => s.NIP);
                break;
            }

            return suppliers;
        }

        private IQueryable<Supplier> Filter(SuppliersListFilters filters, IQueryable<Supplier> suppliers)
        {
            if (!string.IsNullOrEmpty(filters.Email))
                suppliers = suppliers.Where(s => s.Email.StartsWith(filters.Email));
            if (!string.IsNullOrEmpty(filters.Nip))
                suppliers = suppliers.Where(s => s.NIP.StartsWith(filters.Nip));
            if (!string.IsNullOrEmpty(filters.SearchString))
            {
                var search = filters.SearchString.ToLower();
                suppliers = suppliers.Where(s => s.Name.ToLower().Contains(search) ||
                     search.Contains(s.Name.ToLower()) ||
                     s.NIP.Contains(filters.SearchString) ||
                     filters.SearchString.Contains(s.NIP) ||
                     s.Email.Contains(filters.SearchString) ||
                     filters.SearchString.Contains(s.Email) ||
                     s.WebSite.Contains(filters.SearchString) ||
                     filters.SearchString.Contains(s.WebSite));
            }

            return suppliers;
        }

        public async Task<IEnumerable<string>> GetSuppliersEmailssByPrefix(string prefix)
        {
            return await FindByCondition(s => s.Email.StartsWith(prefix)).Select(s => s.Email).ToListAsync();
        }

        public async Task<IEnumerable<string>> GetSuppliersNIPsByPrefix(string prefix)
        {
            return await FindByCondition(s => s.NIP.StartsWith(prefix)).Select(s => s.NIP).ToListAsync();
        }

        public async Task<bool> CheckSupplierDeletable(Supplier supplier)
        {
            return !await FindByCondition(s => s.Id == supplier.Id).AnyAsync(s => s.Products.Any());
        }

        public async Task<IEnumerable<Supplier>> GetSuppliersByQuery(string query)
        {
            query = query.ToLower();
            return await FindByCondition(s => s.City.ToLower().Contains(query) || query.Contains(s.City.ToLower()) ||
                s.Name.ToLower().Contains(query) || query.Contains(s.Name.ToLower()) ||
                s.NIP.Contains(query) || query.Contains(s.NIP) ||
                s.PostalCode.Contains(query) || query.Contains(s.PostalCode) ||
                s.Street.ToLower().Contains(query) || query.Contains(s.Street.ToLower()))
                .ToListAsync();
        }
    }
}
