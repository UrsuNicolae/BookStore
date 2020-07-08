using BookStore.Data.DataModels;
using BookStore.Data.DTO;

namespace BookStore.BusinessLogic.Repository.IRepository
{
    public interface ICategoryRepository:IRepository<Category>
    {
        bool Update(Category obj);
    }
}
