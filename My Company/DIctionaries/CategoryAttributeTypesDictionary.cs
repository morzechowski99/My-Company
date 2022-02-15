//Program powstał na Wydziale Informatyki Politechniki Białostockiej
using My_Company.EnumTypes;
using System.Collections.Generic;

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
