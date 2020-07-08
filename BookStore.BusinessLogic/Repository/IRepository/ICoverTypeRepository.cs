using BookStore.Data.DataModels;
namespace BookStore.BusinessLogic.Repository.IRepository
{
    public interface ICoverTypeRepository:IRepository<CoverType>
    {
        bool Update(CoverType Obj);
    }
}
