using Microsoft.EntityFrameworkCore.Storage;
using My_Company.Data;
using My_Company.DBViews;
using My_Company.Interfaces;
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
        private IProductAttributeRepository productAttributeRepository;
        private IPhotosRepository photosRepository;
        private IDeliveriesRepository deliveriesRepository;
        private IProductSectorRepository productSectorRepository;
        private IOrdersToCompleteView ordersToCompleteView;
        private IOrdersRepository ordersRepository;
        private IPickingRepository pickingRepository;
        private IPickingItemsRepository pickingItemsRepository;
        private IConfigRepository configRepository;

        public RepositoryWrapper(ApplicationDbContext context)
        {
            _context = context;
        }
        public IWarehouseRepository WarehouseRepository
        {
            get
            {
                if (warehouseRepository == null)
                {
                    warehouseRepository = new WarehouseRepository(_context);
                }

                return warehouseRepository;
            }
        }

        public IWarehouseRowRepository WarehouseRowRepository
        {
            get
            {
                if (warehouseRowRepository == null)
                {
                    warehouseRowRepository = new WarehouseRowRepository(_context);
                }

                return warehouseRowRepository;
            }
        }

        public IWarehouseSectorRepository WarehouseSectorRepository
        {
            get
            {
                if (warehouseSectorRepository == null)
                {
                    warehouseSectorRepository = new WarehouseSectorRepository(_context);
                }

                return warehouseSectorRepository;
            }
        }

        public ICategoriesRepository CategoriesRepository
        {
            get
            {
                if (categoriesRepository == null)
                {
                    categoriesRepository = new CategoriesRepository(_context);
                }

                return categoriesRepository;
            }
        }

        public IVATRatesRepository VATRatesRepository
        {
            get
            {
                if (_VATRatesRepository == null)
                {
                    _VATRatesRepository = new VATRatesRepository(_context);
                }

                return _VATRatesRepository;
            }
        }

        public ISuppliersRepository SuppliersRepository
        {
            get
            {
                if (suppliersRepository == null)
                {
                    suppliersRepository = new SuppliersRepository(_context);
                }

                return suppliersRepository;
            }
        }

        public IProductRepository ProductRepository
        {
            get
            {
                if (productRepository == null)
                {
                    productRepository = new ProductRepository(_context);
                }

                return productRepository;
            }
        }

        public IUserRepository UserRepository
        {
            get
            {
                if (userRepository == null)
                {
                    userRepository = new UserRepository(_context);
                }

                return userRepository;
            }
        }

        public ICategoryAttributesRepository CategoryAttributesRepository
        {
            get
            {
                if (categoryAttributesRepository == null)
                {
                    categoryAttributesRepository = new CategoryAttributesRepository(_context);
                }

                return categoryAttributesRepository;
            }
        }

        public IProductAttributeRepository ProductAttributeRepository
        {
            get
            {
                if (productAttributeRepository == null)
                {
                    productAttributeRepository = new ProductAttributesRepository(_context);
                }

                return productAttributeRepository;
            }
        }

        public IPhotosRepository PhotosRepository
        {
            get
            {
                if (photosRepository == null)
                {
                    photosRepository = new PhotosRepository(_context);
                }

                return photosRepository;
            }
        }

        public IDeliveriesRepository DeliveriesRepository
        {
            get
            {
                if (deliveriesRepository == null)
                {
                    deliveriesRepository = new DeliveriesRepository(_context);
                }

                return deliveriesRepository;
            }
        }

        public IProductSectorRepository ProductSectorRepository
        {
            get
            {
                if (productSectorRepository == null)
                {
                    productSectorRepository = new ProductSectorRepository(_context);
                }

                return productSectorRepository;
            }
        }

        public IOrdersToCompleteView OrdersToCompleteView
        {
            get
            {
                if (ordersToCompleteView == null)
                {
                    ordersToCompleteView = new OrdersToCompleteView(_context);
                }

                return ordersToCompleteView;
            }
        }

        public IOrdersRepository OrdersRepository
        {
            get
            {
                if (ordersRepository == null)
                {
                    ordersRepository = new OrdersRepository(_context);
                }

                return ordersRepository;
            }
        }

        public IPickingRepository PickingRepository
        {
            get
            {
                if (pickingRepository == null)
                {
                    pickingRepository = new PickingRepository(_context);
                }

                return pickingRepository;
            }
        }

        public IPickingItemsRepository PickingItemsRepository
        {
            get
            {
                if(pickingItemsRepository == null)
                {
                    pickingItemsRepository = new PickingItemsRepository(_context);
                }

                return pickingItemsRepository;
            }
        }
        
        public IConfigRepository ConfigRepository
        {
            get
            {
                if(configRepository == null)
                {
                    configRepository = new ConfigRepository(_context);
                }

                return configRepository;
            }
        }

        public async Task<IDbContextTransaction> BeginTransaction()
        {
            return await _context.Database.BeginTransactionAsync();
        }

        public void ClearTracked()
        {
            _context.ChangeTracker.Clear();
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
