using Microsoft.AspNetCore.Mvc;
using My_Company.EnumTypes;
using My_Company.ViewModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace My_Company.Areas.Warehouse.ViewModels
{
    public class CreateEditProductViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Nazwa")]
        [Required]
        [MaxLength(25)]
        public string Name { get; set; }
        [Display(Name = "Kod EAN")]
        [RegularExpression(@"^\d{13}$", ErrorMessage = "Pole może zawierać tylko cyfry")]
        [MinLength(13)]
        [MaxLength(13)]
        [Remote(action: "CheckEAN", controller: "Products", areaName: "Warehouse", AdditionalFields = "Id")]
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
        [RegularExpression(@"^0\,(0)([1-9])$|^0\,(([1-9])(\d)?)$|^([1-9])((\,\d{1,2})?)$|^((?!0)(\d){1,5})((\,\d{1,2})?)$|^(1(\d{5})(,\d{1,2})?)$|^(200000(,[0]{1,2})?)$")]
        public string NettoPrice { get; set; }
        [Display(Name = "Status")]
        public ProductStatus Status { get; set; }
        public List<AttributeProductViewModel> Attributes { get; set; }
        public List<PhotoViewModel> Photos { get; set; }
    }
}
