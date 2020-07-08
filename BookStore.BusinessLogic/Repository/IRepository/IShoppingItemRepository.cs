using BookStore.Data.DataModels;
namespace BookStore.BusinessLogic.Repository.IRepository
{
    public interface IShoppingItemRepository:IRepository<ShoppingItem>
    {
        bool Update(ShoppingItem Obj);
    }
}
