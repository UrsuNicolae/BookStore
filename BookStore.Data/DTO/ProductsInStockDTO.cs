namespace BookStore.Data.DTO
{
    public class ProductsInStockDTO
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public ProductDTO Product { get; set; }

        public int ApplicationUserId { get; set; }

        public ApplicationUserDTO ApplicationUser { get; set; }
    }
}
