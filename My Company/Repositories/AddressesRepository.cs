//Program powstał na Wydziale Informatyki Politechniki Białostockiej
using Microsoft.EntityFrameworkCore;
using My_Company.Data;
using My_Company.Interfaces;
using My_Company.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace My_Company.Repositories
{
    public class AddressesRepository : RepositoryBase<Address>, IAddressesRepository
    {
        public AddressesRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<List<Address>> GetAddressesByUser(string userId)
        {
            return await FindByCondition(a => a.UserId == userId).ToListAsync();
        }
    }
}
