using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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
