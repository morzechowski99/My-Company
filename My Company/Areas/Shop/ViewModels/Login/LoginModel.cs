using System.ComponentModel.DataAnnotations;

namespace My_Company.Areas.Shop.ViewModels.Login
{
    public class LoginModel
    {
        [Required]
        [DataType(DataType.EmailAddress, ErrorMessage = "Nieprawidłowy email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string Password { get; set; }

        [Display(Name = "Nie wylogowywuj mnie")]
        public bool RememberMe { get; set; }
    }
}
