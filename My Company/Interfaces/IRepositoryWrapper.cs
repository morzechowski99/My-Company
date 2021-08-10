using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.Interfaces
{
    public interface IRepositoryWrapper
    {
        IWarehouseRepository WarehouseRepository { get; }
        IWarehouseRowRepository WarehouseRowRepository { get; }
        IWarehouseSectorRepository WarehouseSectorRepository { get; }
        Task Save();
        Task<IDbContextTransaction> BeginTransaction();
    }
}
