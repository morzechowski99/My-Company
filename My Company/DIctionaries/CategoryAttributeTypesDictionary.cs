using My_Company.EnumTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.Dictionaries
{
    public static class CategoryAttributeTypesDictionary
    {
        private static Dictionary<AttributeType, string> attributeDictionary = new Dictionary<AttributeType, string>() {
            { AttributeType.Bool, "Prawda/Fałsz" },
            { AttributeType.Date, "Data" },
            { AttributeType.Dictionary, "Atrybut słownikowy" },
            { AttributeType.Numeric, "Numeryczny" },
            { AttributeType.Text, "Tekstowy" },
        };

        public static Dictionary<AttributeType, string> AttributeDictionary { get { return attributeDictionary; } }
    }
}
