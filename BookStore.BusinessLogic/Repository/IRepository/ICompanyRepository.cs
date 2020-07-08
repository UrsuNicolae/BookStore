using BookStore.Data.DataModels;
namespace BookStore.BusinessLogic.Repository.IRepository
{
    public interface ICompanyRepository:IRepository<Company>
    {
        bool Update(Company Obj);
    }
}
