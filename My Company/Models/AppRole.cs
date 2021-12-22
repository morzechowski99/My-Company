using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace My_Company.Models
{
    public class AppRole : IdentityRole
    {
        public AppRole()
        {
            UserRoles = new HashSet<AppUserRole>();
        }

        public virtual ICollection<AppUserRole> UserRoles { get; set; }
    }
}
