//Program powstał na Wydziale Informatyki Politechniki Białostockiej
using My_Company.Models;
using System.Threading.Tasks;

namespace My_Company.Interfaces
{
    public interface IPickingItemsRepository : IRepositoryBase<PickingItem>
    {
        Task<PickingItem> GetItemById(int id);
    }
}
