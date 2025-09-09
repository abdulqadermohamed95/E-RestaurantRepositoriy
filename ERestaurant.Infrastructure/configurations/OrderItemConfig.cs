using ERestaurant.Domain.Entities.Combos;
using ERestaurant.Domain.Entities.Materials;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERestaurant.Infrastructure.configurations
{
    public class OrderItemConfig : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> entity)
        {
            entity.HasKey(oi => oi.Id);

            entity.Property(oi => oi.UnitPrice)
                  .HasColumnType("decimal(18,2)")
                  .IsRequired();

            entity.Property(oi => oi.TotalPrice)
                  .HasColumnType("decimal(18,2)")
                  .IsRequired();

            entity.Property(oi => oi.Quantity).IsRequired();

            entity.HasOne<Material>()
                  .WithMany()
                  .HasForeignKey(oi => oi.MaterialId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne<Combo>()
                  .WithMany()
                  .HasForeignKey(oi => oi.ComboId)
                  .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
