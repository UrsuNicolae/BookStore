using BookStore.BusinessLogic.Repository.IRepository;
using BookStore.Data.Data;
using Microsoft.Extensions.Logging;

namespace BookStore.BusinessLogic.Repository
{

    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;

        private readonly ILogger<UnitOfWork> _logger;

        public UnitOfWork(ApplicationDbContext db, ILogger<UnitOfWork> logger)
        {
            _db = db;
            _logger = logger;
            Category = new CategoryRepository(_db, _logger);
            Company = new CompanyRepository(_db, _logger);
            CoverType = new CoverTypeRepository(_db, _logger);
            Product = new ProductRepository(_db, _logger);
            ApplicationUser = new ApplicationUserRepository(_db, _logger);
            ApplicationRole = new ApplicationRoleRepository(_db, _logger);
            OrderDetails = new OrderDetailsRepository(_db, _logger);
            OrderItem = new OrderItemRepository(_db, _logger);
            ShoppingItem = new ShoppingItemRepository(_db, _logger);
            ProductsInStock = new ProductsInStockRepository(_db, _logger);

        }

        public ICategoryRepository Category { get; private set; }

        public ICoverTypeRepository CoverType { get; private set; }

        public IProductRepository Product { get; private set; }

        public ICompanyRepository Company { get; private set; }

        public IProductsInStockRepository ProductsInStock { get; set; }

        public IApplicationUserRepository ApplicationUser { get; private set; }

        public IApplicationRoleRepository ApplicationRole { get; private set; }

        public IOrderDetailsRepository OrderDetails { get; private set; }

        public IOrderItemRepository OrderItem { get; private set; }

        public IShoppingItemRepository ShoppingItem { get; private set; }

        public void Dispose()
        {
            _db.Dispose();
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
