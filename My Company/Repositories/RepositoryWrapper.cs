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
        private ICategoriesRepository categoriesRepository;
        private IVATRatesRepository _VATRatesRepository;
        private ISuppliersRepository suppliersRepository;
        private IProductRepository productRepository;
        private IUserRepository userRepository;
        private ICategoryAttributesRepository categoryAttributesRepository;

        public RepositoryWrapper(ApplicationDbContext context)
        {
            _context = context;
        }
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

        public ICategoriesRepository CategoriesRepository
        {
            get
            {
                if (categoriesRepository == null)
                    categoriesRepository = new CategoriesRepository(_context);

                return categoriesRepository;
            }
        }

        public IVATRatesRepository VATRatesRepository
        {
            get
            {
                if (_VATRatesRepository == null)
                    _VATRatesRepository = new VATRatesRepository(_context);

                return _VATRatesRepository;
            }
        }

        public ISuppliersRepository SuppliersRepository
        {
            get
            {
                if (suppliersRepository == null)
                    suppliersRepository = new SuppliersRepository(_context);

                return suppliersRepository;
            }
        }

        public IProductRepository ProductRepository
        {
            get
            {
                if (productRepository == null)
                    productRepository = new ProductRepository(_context);

                return productRepository;
            }
        }

        public IUserRepository UserRepository
        {
            get
            {
                if (userRepository == null)
                    userRepository = new UserRepository(_context);

                return userRepository;
            }
        } 
        
        public ICategoryAttributesRepository CategoryAttributesRepository
        {
            get
            {
                if (categoryAttributesRepository == null)
                    categoryAttributesRepository = new CategoryAttributesRepository(_context);

                return categoryAttributesRepository;
            }
        }

        public async Task<IDbContextTransaction> BeginTransaction()
        {
            return await _context.Database.BeginTransactionAsync();
        }

        public bool EnsureCreated()
        {
            return _context.Database.EnsureCreated();
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
