using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.Models
{
    public class AttributeDictionaryValues
    {
        public int Id { get; set; }
        public int AttributeId { get; set; }
        public string Value { get; set; }
        public virtual Attribute Attribute { get; set; }
    }
}
