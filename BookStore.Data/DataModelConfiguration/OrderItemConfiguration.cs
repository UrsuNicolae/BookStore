using BookStore.Data.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.Data.DataModelConfiguration
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.Property(s => s.OrderId).IsRequired();
            builder.HasOne(s => s.OrderDetails)
                .WithMany()
                .HasForeignKey(s => s.OrderId);
        }
    }
}
