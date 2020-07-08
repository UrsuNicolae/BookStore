using BookStore.Data.DataModels;
namespace BookStore.BusinessLogic.Repository.IRepository
{
    public interface IOrderDetailsRepository:IRepository<OrderDetails>
    {
        bool Update(OrderDetails Obj);
    }
}
