//Program powstał na Wydziale Informatyki Politechniki Białostockiej
using System;

namespace My_Company.ViewModels
{
    public class ListFiltersBase<T> where T : Enum
    {
        public int? PageSize { get; set; }
        public int? Page { get; set; }
        public T SortOrder { get; set; }

    }
}
