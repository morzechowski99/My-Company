using Microsoft.AspNetCore.Mvc;
using My_Company.Areas.Warehouse.ViewModels;
using My_Company.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.Interfaces
{
    public interface ISuppliersRepository : IRepositoryBase<Supplier>
    {
        Task<Supplier> GetById(int id);
        IQueryable<Supplier> GetSuppliersByFilters(SuppliersListFilters filters);
        Task<IEnumerable<string>> GetSuppliersNIPsByPrefix(string prefix);
        Task<IEnumerable<string>> GetSuppliersEmailssByPrefix(string prefix);
        Task<bool> CheckSupplierDeletable(Supplier supplier);
        Task<IEnumerable<Supplier>> GetSuppliersByQuery(string query);
    }
}
