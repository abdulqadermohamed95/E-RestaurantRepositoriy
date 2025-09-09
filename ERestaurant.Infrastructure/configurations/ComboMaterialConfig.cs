using ERestaurant.Domain.Entities.Combos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERestaurant.Infrastructure.configurations
{
    public class ComboMaterialConfig : IEntityTypeConfiguration<ComboMaterial>
    {
        public void Configure(EntityTypeBuilder<ComboMaterial> entity)
        {
            entity.HasKey(cm => cm.Id);

            entity.HasOne(cm => cm.Combo)
                  .WithMany(c => c.ComboMaterials)
                  .HasForeignKey(cm => cm.ComboId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(cm => cm.Material)
                  .WithMany()
                  .HasForeignKey(cm => cm.MaterialId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.Property(cm => cm.Quantity).IsRequired();
            entity.Property(cm => cm.IsOptional).HasDefaultValue(false);
        }
    }
}
