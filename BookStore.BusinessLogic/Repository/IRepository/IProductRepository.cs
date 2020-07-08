using BookStore.Data.DataModels;
using System.Collections.Generic;

namespace BookStore.BusinessLogic.Repository.IRepository
{
    public interface IProductRepository:IRepository<Product>
    {
        bool Update(Product Obj);

    }
}
