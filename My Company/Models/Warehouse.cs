﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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
        public string Name { get; set; }
        [Required]
        public string Street { get; set; }
        public string PostalCode { get; set; }
        [Required]
        public string City { get; set; }
        public string Voivodeship { get; set; }
        public virtual ICollection<WarehouseRow> Rows { get; set; }
    }
}
