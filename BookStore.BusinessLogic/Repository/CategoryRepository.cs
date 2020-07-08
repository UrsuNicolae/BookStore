using System;
using BookStore.BusinessLogic.Repository.IRepository;
using Microsoft.Extensions.Logging;

using BookStore.Data.Data;
using BookStore.Data.DataModels;
using System.Linq;
using BookStore.Data.DTO;

namespace BookStore.BusinessLogic.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger logger;

        public CategoryRepository(ApplicationDbContext db, ILogger<UnitOfWork> logger) : base(db, logger)
        {
            _db = db;
            this.logger = logger;
        }

        public bool Update(Category obj)
        {
            try
            {
                var objFromDb = _db.Categories.FirstOrDefault(s => s.Id == obj.Id);
                if (objFromDb != null)
                {
                    objFromDb.Name = obj.Name;
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
