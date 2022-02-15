//Program powstał na Wydziale Informatyki Politechniki Białostockiej
using System.ComponentModel.DataAnnotations;

namespace My_Company.Areas.Warehouse.ViewModels
{
    public class EmployeeListItem
    {
        public string Id { get; set; }
        [Display(Name = "Imię i nazwisko")]
        public string NameAndSurname { get; set; }
        [Display(Name = "E-mail")]
        public string Email { get; set; }
        [Display(Name = "Login")]
        public string UserName { get; set; }
        [Display(Name = "Zablokowany?")]
        public bool LockoutEnabled { get; set; }

    }
}
