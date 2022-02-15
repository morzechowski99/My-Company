//Program powstał na Wydziale Informatyki Politechniki Białostockiej
using Microsoft.AspNetCore.Mvc.Rendering;
using My_Company.Areas.Warehouse.EnumTypes;
using My_Company.Models;
using My_Company.ViewModels;
using System.Collections.Generic;

namespace My_Company.Helpers
{
    public static class ViewHelpers
    {
        public static SelectList GetVatRatesSelectList(IEnumerable<VATRate> rates, int? selectedRate)
        {
            var ratesView = new List<VATRateViewModel>();

            foreach (var rate in rates)
            {
                ratesView.Add(new VATRateViewModel
                {
                    Id = rate.Id,
                    Rate = rate.Rate.ToString() + "%"
                });
            }

            return new SelectList(ratesView, "Id", "Rate", selectedRate.HasValue ? selectedRate : null);
        }
        public static SelectList GetSuppliersSelectList(IEnumerable<Supplier> suppliers, int? selectedSupplierId)
        {
            var suppliersView = new List<SupplierSelectViewModel>();

            foreach (var supplier in suppliers)
            {
                suppliersView.Add(new SupplierSelectViewModel
                {
                    Id = supplier.Id,
                    Description = $"{supplier.Name} \n{supplier.Street} {supplier.PostalCode} {supplier.City}"
                });
            }

            return new SelectList(suppliersView, "Id", "Description", selectedSupplierId.HasValue ? selectedSupplierId : null);
        }

        public static StockState GetProductStockStatus(Product p)
        {
            if (p.StockQuantity > p.Demand * 1.15)
                return StockState.Good;
            else if (p.StockQuantity > p.Demand)
                return StockState.RunningOut;
            else
                return StockState.Critical;
        }
    }
}
