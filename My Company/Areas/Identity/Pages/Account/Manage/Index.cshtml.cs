//Program powstał na Wydziale Informatyki Politechniki Białostockiej
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using My_Company.Models;

namespace My_Company.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        public IActionResult OnGet()
        {
            return RedirectToPage("ChangePassword");
        }
    }
}
