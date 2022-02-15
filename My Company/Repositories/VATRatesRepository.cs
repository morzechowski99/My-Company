//Program powstał na Wydziale Informatyki Politechniki Białostockiej
using My_Company.Data;
using My_Company.Interfaces;
using My_Company.Models;

namespace My_Company.Repositories
{
    public class VATRatesRepository : RepositoryBase<VATRate>, IVATRatesRepository
    {
        public VATRatesRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
