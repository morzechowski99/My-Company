//Program powstał na Wydziale Informatyki Politechniki Białostockiej
using System.Collections.Generic;

namespace My_Company.Extensions
{
    public static class ICollectionExtensions
    {
        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> collectionToAdd)
        {
            foreach (var item in collectionToAdd)
                collection.Add(item);
        }
    }
}
