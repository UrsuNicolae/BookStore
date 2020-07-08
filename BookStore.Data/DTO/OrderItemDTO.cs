namespace BookStore.Data.DTO
{
    public class OrderItemDTO
    {
        public int Id { get; set; }
    
        public int OrderId { get; set; }

        public OrderDetailsDTO OrderDetails { get; set; }

        public int ProductId { get; set; }

        public ProductDTO Product { get; set; }
        
        public int Count { get; set; }

        public double Price { get; set; }
    }
}
