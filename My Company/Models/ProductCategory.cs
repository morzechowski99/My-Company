//Program powstał na Wydziale Informatyki Politechniki Białostockiej
namespace My_Company.Models
{
    public class ProductCategory
    {
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public bool IsProductCategory { get; set; }
        public virtual Product Product { get; set; }
        public virtual Category Category { get; set; }
    }
}
