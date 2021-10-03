using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.ViewModels
{
    public class ListFiltersBase<T> where T : Enum
    {
        public int? PageSize { get; set; }
        public int? Page { get; set; }
        public T SortOrder { get; set; }
        
    }
}
