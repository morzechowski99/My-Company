//Program powstał na Wydziale Informatyki Politechniki Białostockiej
using System;
using System.ComponentModel.DataAnnotations;

namespace My_Company.Models
{
    public class Packing
    {
        [Key]
        public Guid OrderId { get; set; }
        public DateTime PackingStart { get; set; }
        public DateTime? PackingEnd { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }
    }
}
