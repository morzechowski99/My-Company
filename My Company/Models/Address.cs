//Program powstał na Wydziale Informatyki Politechniki Białostockiej
using System.Collections.Generic;

namespace My_Company.Models
{
    public class Address
    {
        public Address()
        {
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string PhoneNumber { get; set; }
        public virtual AppUser User { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public override string ToString()
        {
            return $"{Street}, {ZipCode} {City}";
        }
    }
}
