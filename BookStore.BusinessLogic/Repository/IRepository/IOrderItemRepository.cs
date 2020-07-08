using BookStore.Data.DataModels;
namespace BookStore.BusinessLogic.Repository.IRepository
{
    public interface IOrderItemRepository:IRepository<OrderItem>
    {
        bool Update(OrderItem obj);
    }
}
