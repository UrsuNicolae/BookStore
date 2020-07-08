namespace BookStore.Data.DataModels
{
    public class OrderItem
    {
        public int Id { get; set; }
    
        public int OrderId { get; set; }

        public OrderDetails OrderDetails { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; }
        
        public int Count { get; set; }

        public double Price { get; set; }
    }
}
