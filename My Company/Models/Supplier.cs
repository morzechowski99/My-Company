//Program powstał na Wydziale Informatyki Politechniki Białostockiej
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace My_Company.Models
{
    public class Supplier
    {
        public Supplier()
        {
            Products = new HashSet<Product>();
        }

        public int Id { get; set; }
        [Required]
        [MaxLength(20)]
        public string Name { get; set; }
        [MaxLength(15)]
        public string NIP { get; set; }
        [Required]
        [MaxLength(100)]
        public string Street { get; set; }
        [Required]
        [MaxLength(6)]
        public string PostalCode { get; set; }
        [Required]
        [MaxLength(30)]
        public string City { get; set; }
        [MaxLength(15)]
        [Required]
        public string PhoneNumber1 { get; set; }
        [MaxLength(15)]
        public string PhoneNumber2 { get; set; }
        [MaxLength(50)]
        public string WebSite { get; set; }
        [MaxLength(50)]
        public string Email { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
