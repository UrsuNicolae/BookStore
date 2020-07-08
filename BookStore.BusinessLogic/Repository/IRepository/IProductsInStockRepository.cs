using BookStore.Data.DataModels;
namespace BookStore.BusinessLogic.Repository.IRepository
{
    public interface IProductsInStockRepository:IRepository<ProductsInStock>
    {
        bool Update(ProductsInStock obj);
    }
}
