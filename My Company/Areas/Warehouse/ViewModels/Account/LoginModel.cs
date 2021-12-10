using System.ComponentModel.DataAnnotations;

namespace My_Company.Areas.Warehouse.ViewModels.Account
{
    public class LoginModel
    {
        [Required]
        [Display(Name = "Nazwa użytkownika")]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string Password { get; set; }
        [Display(Name = "Pamiętaj mnie")]
        public bool RememberMe { get; set; }
    }
}
