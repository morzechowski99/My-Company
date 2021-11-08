using My_Company.Data;
using My_Company.Interfaces;
using My_Company.Models.DBViews;

namespace My_Company.DBViews
{
    public class OrdersToCompleteView : DBViewBase<OrdersToComplete>, IOrdersToCompleteView
    {
        public OrdersToCompleteView(ApplicationDbContext context) : base(context)
        {
        }
    }
}
