using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using My_Company.EnumTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.Areas.Warehouse.ViewModels
{
    public class CreateEditProductViewModel
    {
        public int Id { get; set; }
        [Display(Name ="Nazwa")]
        [Required]
        [MaxLength(25)]
        public string Name { get; set; }
        [Display(Name = "Kod EAN")]
        [RegularExpression(@"^\d{13}$",ErrorMessage ="Pole może zawierać tylko cyfry")]
        [MinLength(13)]
        [MaxLength(13)]
        [Remote(action: "CheckEAN", controller: "Products", areaName: "Warehouse",AdditionalFields ="Id")]
        [Required]
        public string EANCode { get; set; }
        [DataType(DataType.MultilineText)]
        [Display(Name = "Opis")]
        [MaxLength(15000)]
        public string Description { get; set; }
        [Required]
        [Display(Name = "Kategoria")]
        public List<int> Categories { get; set; }
        [Display(Name = "Dostawca")]
        [Required]
        public int SupplierId { get; set; }
        [Display(Name = "Stawka VAT")]
        [Required]
        public int VATRateId { get; set; }
        [Display(Name = "Cena (netto,zł)")]
        [Required]
        [DataType(DataType.Currency)]
        [RegularExpression(@"^(\d*\,\d{1,2}|\d+)$")]
        public string NettoPrice { get; set; }
        [Display(Name="Status")]
        public ProductStatus Status { get; set; }
        public List<AttributeProductViewModel> Attributes { get; set; }
        public List<PhotoViewModel> Photos { get; set; }
    }
}
