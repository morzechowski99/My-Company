//Program powstał na Wydziale Informatyki Politechniki Białostockiej
using My_Company.EnumTypes;
using System.ComponentModel.DataAnnotations;

namespace My_Company.Areas.Warehouse.ViewModels
{
    public class EditAttributeViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        [Display(Name = "Nazwa")]
        public string Name { get; set; }
        [Display(Name = "Typ")]
        public AttributeType Type { get; set; }
    }
}
