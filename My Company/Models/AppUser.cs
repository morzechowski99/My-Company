using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.Models
{
    public class AppUser: IdentityUser
    {
        public AppUser()
        {
            Orders = new HashSet<Order>();
        }

        public string Name { get; set; }
        public string Surname { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
