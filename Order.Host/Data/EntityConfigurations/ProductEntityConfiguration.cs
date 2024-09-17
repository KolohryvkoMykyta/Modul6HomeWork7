using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Order.Host.Data.Entities;

namespace Order.Host.Data.EntityConfigurations
{
    public class ProductEntityConfiguration : IEntityTypeConfiguration<ProductEntity>
    {
        public void Configure(EntityTypeBuilder<ProductEntity> builder)
        {
            builder.ToTable("Products");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                .IsRequired();

            builder.HasOne(p => p.Order)
                .WithMany(o => o.Products)
                .HasForeignKey(p => p.OrderId);
        }
    }
}
