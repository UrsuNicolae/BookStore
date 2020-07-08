using BookStore.Data.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.Data.DataModelConfiguration
{
    public class CoverTypeConfiguration : IEntityTypeConfiguration<CoverType>
    {
        public void Configure(EntityTypeBuilder<CoverType> builder)
        {
            builder.Property(s => s.Name)
                .HasMaxLength(50)
                .IsRequired()
                .HasAnnotation("DisplayName", "Cover Type");
        }
    }
}
