using System.Collections.Generic;

namespace My_Company.Models
{
    public class Category
    {
        public Category()
        {
            ProductCategories = new HashSet<ProductCategory>();
            ChildCategories = new HashSet<Category>();
            Attributes = new HashSet<Attribute>();
        }


        public int Id { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public int? ParentCategoryId { get; set; }
        public virtual Category ParentCategory { get; set; }
        public virtual ICollection<ProductCategory> ProductCategories { get; set; }
        public virtual ICollection<Category> ChildCategories { get; set; }
        public virtual ICollection<Attribute> Attributes { get; set; }

    }
}
