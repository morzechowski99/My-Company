using Microsoft.EntityFrameworkCore.Storage;
using My_Company.Data;
using My_Company.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace My_Company.Repositories
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private readonly ApplicationDbContext _context;
        private IWarehouseRepository warehouseRepository;
        private IWarehouseRowRepository warehouseRowRepository;
        private IWarehouseSectorRepository warehouseSectorRepository;
        public IWarehouseRepository WarehouseRepository
        {
            get
            {
                if (warehouseRepository == null)
                    warehouseRepository = new WarehouseRepository(_context);

                return warehouseRepository;
            }
        }

        public IWarehouseRowRepository WarehouseRowRepository
        {
            get
            {
                if (warehouseRowRepository == null)
                    warehouseRowRepository = new WarehouseRowRepository(_context);

                return warehouseRowRepository;
            }
        }

        public IWarehouseSectorRepository WarehouseSectorRepository
        {
            get
            {
                if (warehouseSectorRepository == null)
                    warehouseSectorRepository = new WarehouseSectorRepository(_context);

                return warehouseSectorRepository;
            }
        }

        public RepositoryWrapper(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IDbContextTransaction> BeginTransaction()
        {
            return await _context.Database.BeginTransactionAsync();
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
