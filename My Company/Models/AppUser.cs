//Program powstał na Wydziale Informatyki Politechniki Białostockiej
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace My_Company.Models
{
    public class AppUser : IdentityUser
    {
        public AppUser()
        {
            Orders = new HashSet<Order>();
            UserRoles = new HashSet<AppUserRole>();
            Addresses = new HashSet<Address>();
        }

        public string Name { get; set; }
        public string Surname { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<AppUserRole> UserRoles { get; set; }
        public virtual ICollection<Address> Addresses { get; set; }
    }
}
