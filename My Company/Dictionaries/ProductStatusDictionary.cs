using My_Company.EnumTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.Dictionaries
{
    public class ProductStatusDictionary
    {
        private static Dictionary<ProductStatus, string> productStatusDictionary = new Dictionary<ProductStatus, string>() {
            { ProductStatus.Active, "Aktywny" },
            { ProductStatus.Archived, "Zarchiowizowany" },
            { ProductStatus.TemporarilyUnavailable, "Tymczasowo niedostępny"}
        };

        public static Dictionary<ProductStatus, string> ProductStatusesDictionary { get { return productStatusDictionary; } }
    }
}

