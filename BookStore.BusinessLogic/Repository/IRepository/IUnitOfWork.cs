using System;

namespace BookStore.BusinessLogic.Repository.IRepository
{
    public interface IUnitOfWork:IDisposable
    {
        ICategoryRepository Category { get; }
        ICoverTypeRepository CoverType { get; }
        IProductRepository Product { get; }
        ICompanyRepository Company { get; }
        IApplicationUserRepository ApplicationUser { get; }
        IApplicationRoleRepository ApplicationRole { get; }
        IOrderDetailsRepository OrderDetails { get; }
        IOrderItemRepository OrderItem { get; }
        IShoppingItemRepository ShoppingItem { get; }
        IProductsInStockRepository ProductsInStock { get; }
        void Save();
    }
}
