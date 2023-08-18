using Catalog.Host.Data.Entities;

namespace Catalog.Host.Data.EntityConfigurations
{
    public class CatalogRadiusEntityTypeConfiguration
        : IEntityTypeConfiguration<CatalogRadius>
    {
        public void Configure(EntityTypeBuilder<CatalogRadius> builder)
        {
            builder.ToTable("CatalogRadius");

            builder.HasKey(ci => ci.Id);

            builder.Property(ci => ci.Id)
                .UseHiLo("catalog_radius_hilo")
                .IsRequired();

            builder.Property(cb => cb.Radius)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
