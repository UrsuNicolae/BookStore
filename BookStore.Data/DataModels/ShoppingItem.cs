using System.ComponentModel.DataAnnotations;

namespace BookStore.Data.DataModels
{
    public class ShoppingItem
    {
        public ShoppingItem()
        {
            Count = 1;
        }

        public int Id { get; set; }

        public string ApplicationUserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; }

        [Range(1,1000, ErrorMessage = "Please enter a number from 1 to 1000")]
        public int Count { get; set; }

        public double Price { get; set; }
    }
}
