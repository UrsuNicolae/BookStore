using System;
using BookStore.BusinessLogic.Repository.IRepository;
using BookStore.Data.Data;
using BookStore.Data.DataModels;
using Microsoft.Extensions.Logging;

namespace BookStore.BusinessLogic.Repository
{
    public class ProductsInStockRepository : Repository<ProductsInStock>, IProductsInStockRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger logger;

        public ProductsInStockRepository(ApplicationDbContext db, ILogger<UnitOfWork> logger) : base(db, logger)
        {
            _db = db;
            this.logger = logger;
        }

        public bool Update(ProductsInStock obj)
        {
            try
            {
                _db.Update(obj);
                return true;
            }
            catch (Exception e)
            {
                logger.LogError(e.ToString());
                return false;
            }

        }
    }
}
