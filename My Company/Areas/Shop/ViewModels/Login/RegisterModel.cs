using System.ComponentModel.DataAnnotations;

namespace My_Company.Areas.Shop.ViewModels.Login
{
    public class RegisterModel
    {
        [Required]
        [DataType(DataType.EmailAddress, ErrorMessage = "Nieprawidłowy email")]
        [Display(Name = "Email")]
        public string EmailRegister { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} musi mieć co najmniej {2} i co najwyzej {1} znaków.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string PasswordRegister { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Potwierdź hasło")]
        [Compare("PasswordRegister", ErrorMessage = "Hasła nie są takie same.")]
        public string ConfirmPassword { get; set; }
        [Display(Name = "Imię")]
        [Required]
        public string Name { get; set; }
        [Display(Name = "Nazwisko")]
        [Required]
        public string Surname { get; set; }
    }
}
