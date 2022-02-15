//Program powstał na Wydziale Informatyki Politechniki Białostockiej
using My_Company.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace My_Company.Interfaces
{
    public interface IUserRepository : IRepositoryBase<AppUser>
    {
        Task<int> GetUsersWithSameNameAndSurnameCount(string name, string surname);
        Task<IEnumerable<AppUser>> GetAll();
    }
}
