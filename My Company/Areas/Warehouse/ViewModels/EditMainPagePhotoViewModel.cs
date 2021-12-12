using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace My_Company.Areas.Warehouse.ViewModels
{
    public class EditMainPagePhotoViewModel
    {
        [Required]
        public int Order { get; set; }
        [Required]
        public IFormFile Photo { get; set; }
    }
}
