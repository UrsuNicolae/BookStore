using System;
using System.Collections.Generic;
using System.Linq;
using BookStore.BusinessLogic.Repository.IRepository;
using BookStore.Data.Data;
using BookStore.Data.DataModels;
using Microsoft.Extensions.Logging;

namespace BookStore.BusinessLogic.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger logger;

        public ProductRepository(ApplicationDbContext db, ILogger<UnitOfWork> logger) : base(db, logger)
        {
            _db = db;
            this.logger = logger;
        }

        public bool Update(Product obj)
        {
            try
            {
                var objFromDb = _db.Products.FirstOrDefault(s => s.Id == obj.Id);
                if (objFromDb != null)
                {
                    if (objFromDb.ImageUrl != null)
                    {
                        objFromDb.ImageUrl = obj.ImageUrl;
                    }
                    objFromDb.ISBN = obj.ISBN;
                    objFromDb.Price = obj.Price;
                    objFromDb.ListPrice = obj.ListPrice;
                    objFromDb.Title = obj.Title;
                    objFromDb.Description = obj.Description;
                    objFromDb.CategoryId = obj.CategoryId;
                    objFromDb.Author = obj.Author;
                    objFromDb.CoverTypeId = obj.CoverTypeId;
                    objFromDb.ImageUrl = obj.ImageUrl;
                    objFromDb.stock = obj.stock;
                }
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
