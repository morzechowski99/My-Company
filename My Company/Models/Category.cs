using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.Models
{
    public class Category
    {
        public Category()
        {
            Products = new HashSet<Product>();
            ChildCategories = new HashSet<Category>();
        }


        public int Id { get; set; }
        public string CategoryName { get; set; }
        public string Descripttion { get; set; }
        public int ParentCategoryId { get; set; }
        public virtual Category ParentCategory { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<Category> ChildCategories { get; set; }

    }
}
