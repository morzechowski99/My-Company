using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace My_Company.Models
{
    public class Picking
    {
        public Picking()
        {
            PickingItems = new HashSet<PickingItem>();
        }

        [Key]
        public Guid OrderId { get; set; }
        public string UserId { get; set; }
        public DateTime Start { get; set; }
        public DateTime? End { get; set; }
        public virtual AppUser User { get; set; }
        public virtual Order Order { get; set; }
        public virtual ICollection<PickingItem> PickingItems { get; set; }
    }
}
