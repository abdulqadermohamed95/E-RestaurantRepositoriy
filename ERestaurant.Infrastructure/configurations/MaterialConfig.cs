using ERestaurant.Domain.Entities.Materials;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERestaurant.Infrastructure.configurations
{
    public class MaterialConfig : IEntityTypeConfiguration<Material>
    {
        public void Configure(EntityTypeBuilder<Material> entity)
        {
            entity.HasKey(m => m.Id);

            entity.Property(m => m.NameEn)
                  .IsRequired()
                  .HasMaxLength(50);

            entity.Property(m => m.NameAr)
                  .HasMaxLength(50);

            entity.Property(m => m.Price)
                  .IsRequired()
                  .HasColumnType("decimal(18,2)");

            entity.Property(m => m.Unit)
                  .IsRequired()
                  .HasMaxLength(50);

            entity.Property(m => m.Tax)
                  .IsRequired()
                  .HasColumnType("decimal(5,2)");

            entity.Property(m => m.ImageUrl)
                  .HasMaxLength(500);

            // Global filter for soft delete
            entity.HasQueryFilter(m => !m.IsDeleted);
        }
    }
}
