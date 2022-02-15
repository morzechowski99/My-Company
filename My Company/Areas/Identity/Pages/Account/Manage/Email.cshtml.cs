//Program powstał na Wydziale Informatyki Politechniki Białostockiej
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using My_Company.Models;

namespace My_Company.Areas.Identity.Pages.Account.Manage
{
    public partial class EmailModel : PageModel
    {
        public ActionResult OnGetAsync()
        {
            return StatusCode(404);
        }

    }
}
