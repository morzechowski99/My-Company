//Program powstał na Wydziale Informatyki Politechniki Białostockiej
using System.ComponentModel.DataAnnotations;

namespace My_Company.Models
{
    public class Photo
    {
        [Key]
        public string Path { get; set; }
        public int ProductId { get; set; }
        public bool IsListPhoto { get; set; }
        public bool IsMainPhoto { get; set; }
        public virtual Product Product { get; set; }
    }
}
