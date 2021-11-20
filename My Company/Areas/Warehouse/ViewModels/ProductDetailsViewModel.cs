using My_Company.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.Areas.Warehouse.ViewModels
{
    public class ProductDetailsViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Nazwa")]
        public string Name { get; set; }
        [Display(Name = "Kod EAN")]
        public string EANCode { get; set; }
        [Display(Name = "Opis")]
        public string Description { get; set; }
        [Display(Name = "Kategoria")]
        public string Category { get; set; }
        [Display(Name = "Dostawca")]
        public string Supplier { get; set; }
        public int SupplierId { get; set; }
        [Display(Name = "Stawka VAT")]
        public string VATRate { get; set; }
        [Display(Name = "Cena (netto,zł)")]
        public decimal NettoPrice { get; set; }
        [Display(Name = "Atrybuty")]
        public List<AttributeProductViewModel> Attributes { get; set; }
        [Display(Name = "Zdjęcia")]
        public List<string> Photos { get; set; }
        [Display(Name = "Na magazynie")]
        public int MagazineCount { get; set; }
        [Display(Name = "Zapotrzebowanie")]
        public int Demand { get; set; }
        public string Status { get; set; }
    }
}
