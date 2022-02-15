//Program powstał na Wydziale Informatyki Politechniki Białostockiej
using System.ComponentModel.DataAnnotations;

namespace My_Company.Areas.Warehouse.ViewModels
{
    public class EditEmployeeViewModel
    {
        public string Id { get; set; }
        [Display(Name = "Imię")]
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        [Display(Name = "Nazwisko")]
        [Required]
        [MaxLength(50)]
        public string Surname { get; set; }
        [EmailAddress]
        [Required]
        [MaxLength(60)]
        [Display(Name = "Adres Email")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Rola")]
        public string Role { get; set; }
    }
}
