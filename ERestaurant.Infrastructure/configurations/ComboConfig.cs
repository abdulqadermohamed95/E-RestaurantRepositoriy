using ERestaurant.Domain.Entities.Combos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERestaurant.Infrastructure.configurations
{
    public class ComboConfig : IEntityTypeConfiguration<Combo>
    {
        public void Configure(EntityTypeBuilder<Combo> entity)
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.NameAr).IsRequired().HasMaxLength(50);
            entity.Property(c => c.NameEn).IsRequired().HasMaxLength(50);
            entity.Property(c => c.Price).HasColumnType("decimal(18,2)");
            entity.Property(c => c.Tax).HasColumnType("decimal(5,2)");

            entity.HasMany(c => c.ComboMaterials)
                  .WithOne(cm => cm.Combo)
                  .HasForeignKey(cm => cm.ComboId)
                  .OnDelete(DeleteBehavior.Cascade);

            //using to make global filter as not get deleted items
            entity.HasQueryFilter(m => !m.IsDeleted);
        }
    }
}
