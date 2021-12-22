using System.ComponentModel.DataAnnotations;

namespace My_Company.Areas.Warehouse.ViewModels
{
    public class ProductBasicInfoEditViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(25)]
        public string Name { get; set; }
        [Display(Name = "Kod EAN")]
        [RegularExpression(@"^\d{13}$", ErrorMessage = "Pole może zawierać tylko cyfry")]
        [MinLength(13)]
        [MaxLength(13)]
        [Required]
        public string EANCode { get; set; }
        [Required]
        public int SupplierId { get; set; }
        [Required]
        public int VATRateId { get; set; }
        [Required]
        [RegularExpression(@"^(\d*\,\d{1,2}|\d+)$")]
        public string NettoPrice { get; set; }
    }
}
