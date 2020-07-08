using System.ComponentModel.DataAnnotations;

namespace BookStore.Data.DTO
{
    public class ShoppingItemDTO
    {
        public ShoppingItemDTO()
        {
            Count = 1;
        }

        public int Id { get; set; }

        public string ApplicationUserId { get; set; }

        public ApplicationUserDTO ApplicationUser { get; set; }

        public int ProductId { get; set; }

        public ProductDTO Product { get; set; }

        [Range(1,1000, ErrorMessage = "Please enter a number from 1 to 1000")]
        public int Count { get; set; }

        public double Price { get; set; }
    }
}
