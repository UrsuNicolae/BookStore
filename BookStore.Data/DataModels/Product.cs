using System.ComponentModel.DataAnnotations;

namespace BookStore.Data.DataModels
{
    public class Product
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string ISBN { get; set; }

        public string Author { get; set; }

        [Range(1, 10000)]
        public double Price{ get; set; }

        [Range(1, 10000)]
        public double ListPrice { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public string ImageUrl { get; set; }

        public int CoverTypeId { get; set; }

        public CoverType CoverType { get; set; }

        [Range(0, 10000)]
        public int stock { get; set; }
    }
}
