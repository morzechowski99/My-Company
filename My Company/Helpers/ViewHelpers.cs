using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using My_Company.Models;
using My_Company.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.Helpers
{
    public static class ViewHelpers
    {
        public static SelectList GetVatRatesSelectList(IEnumerable<VATRate> rates,int? selectedRate)
        {
            var ratesView = new List<VATRateViewModel>();

            foreach(var rate in rates)
            {
                ratesView.Add(new VATRateViewModel
                {
                    Id = rate.Id,
                    Rate = rate.Rate.ToString() + "%"
                });
            }

            return new SelectList(ratesView, "Id", "Rate",selectedRate.HasValue? selectedRate : null);
        }
        public static SelectList GetSuppliersSelectList(IEnumerable<Supplier> suppliers,int? selectedSupplierId)
        {
            var suppliersView = new List<SupplierSelectViewModel>();

            foreach(var supplier in suppliers)
            {
                suppliersView.Add(new SupplierSelectViewModel
                {
                    Id = supplier.Id,
                    Description = $"{supplier.Name} \n{supplier.Street} {supplier.PostalCode} {supplier.City}"
                });
            }

            return new SelectList(suppliersView, "Id", "Description", selectedSupplierId.HasValue? selectedSupplierId : null);
        }
    }
}
