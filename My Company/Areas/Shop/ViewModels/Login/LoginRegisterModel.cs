using System.ComponentModel.DataAnnotations;

namespace My_Company.Areas.Shop.ViewModels.Login
{
    public class LoginRegisterModel
    {

        //login
        [Required]
        [DataType(DataType.EmailAddress, ErrorMessage = "Nieprawidłowy email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string Password { get; set; }

        [Display(Name = "Nie wylogowywuj mnie")]
        public bool RememberMe { get; set; }
        //register

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
