using My_Company.EnumTypes;
using System.Collections.Generic;

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

