using BookStore.Data.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.Data.DataModelConfiguration
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(s => s.ISBN).IsRequired();
            builder.Property(s => s.Title).IsRequired();
            builder.Property(s => s.Author).IsRequired();
            builder.Property(s => s.ListPrice).IsRequired();
            builder.Property(s => s.Price).IsRequired();
            builder.Property(s => s.CoverTypeId).IsRequired();
            builder.Property(s => s.CategoryId).IsRequired();
        }
    }
}
