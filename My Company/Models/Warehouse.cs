//Program powstał na Wydziale Informatyki Politechniki Białostockiej
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace My_Company.Models
{
    public class Warehouse
    {
        public Warehouse()
        {
            Rows = new HashSet<WarehouseRow>();
        }

        public int Id { get; set; }
        [Required]
        [Display(Name = "Nazwa")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Ulica")]
        public string Street { get; set; }
        [Display(Name = "Kod pocztowy")]
        public string PostalCode { get; set; }
        [Required]
        [Display(Name = "Miasto")]
        public string City { get; set; }
        [Display(Name = "Województwo")]
        public string Voivodeship { get; set; }
        public virtual ICollection<WarehouseRow> Rows { get; set; }
    }
}
