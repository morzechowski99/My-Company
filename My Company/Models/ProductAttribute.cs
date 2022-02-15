//Program powstał na Wydziale Informatyki Politechniki Białostockiej
namespace My_Company.Models
{
    public class ProductAttribute
    {
        public int Id { get; set; }
        public int AttributeId { get; set; }
        public int ProductId { get; set; }
        public string Value { get; set; }
        public virtual Product Product { get; set; }
        public virtual Attribute Attribute { get; set; }
    }
}
