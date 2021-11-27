﻿using My_Company.EnumTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.Models
{
    public class Payment
    {
        [Key]
        public Guid OrderId { get; set; }
        public PaymentStatus Status { get; set; }
        public virtual Order Order { get; set; }
    }
}
