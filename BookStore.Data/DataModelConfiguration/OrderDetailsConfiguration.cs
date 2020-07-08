using BookStore.Data.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.Data.DataModelConfiguration
{
    public class OrderDetailsConfiguration : IEntityTypeConfiguration<OrderDetails>
    {
        public void Configure(EntityTypeBuilder<OrderDetails> builder)
        {
            builder.Property(s => s.ShippingDate).IsRequired();
            builder.Property(s => s.OrderTotal).IsRequired();
            builder.Property(s => s.PhoneNumber).IsRequired();
            builder.Property(s => s.StreetAdress).IsRequired();
            builder.Property(s => s.City).IsRequired();
            builder.Property(s => s.PostalCode).IsRequired();
            builder.Property(s => s.Name).IsRequired();
        }
    }
}
