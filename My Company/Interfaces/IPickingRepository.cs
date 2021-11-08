using My_Company.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace My_Company.Interfaces
{
    public interface IPickingRepository : IRepositoryBase<Picking>
    {
        Task<List<PickingItem>> GetItems(Guid pickingId);
    }
}
