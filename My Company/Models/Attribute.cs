using My_Company.EnumTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.Models
{
    public class Attribute
    {
        public Attribute()
        {
            AttributeDictionaryValues = new HashSet<AttributeDictionaryValues>();
            ProductAttributes = new HashSet<ProductAttribute>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public AttributeType Type { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<AttributeDictionaryValues> AttributeDictionaryValues { get; set; }
        public virtual ICollection<ProductAttribute> ProductAttributes { get; set; }

    }
}
