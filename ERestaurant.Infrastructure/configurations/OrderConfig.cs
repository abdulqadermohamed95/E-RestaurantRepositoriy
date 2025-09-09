using ERestaurant.Application.Common.Interfaces;
using ERestaurant.Domain.Entities.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERestaurant.Infrastructure.configurations
{
    public class OrderConfig : IEntityTypeConfiguration<Order>
    {
      

        public void Configure(EntityTypeBuilder<Order> entity)
        {
            entity.HasKey(o => o.Id);

            entity.Property(o => o.OrderNumber).IsRequired().HasMaxLength(50);
            entity.Property(o => o.CustomerName).IsRequired().HasMaxLength(100);
            entity.Property(o => o.CustomerMobile).IsRequired().HasMaxLength(20);

            entity.Property(o => o.TotalBeforeTax).HasColumnType("decimal(18,2)");
            entity.Property(o => o.TotalTax).HasColumnType("decimal(18,2)");
            entity.Property(o => o.TotalAfterTax).HasColumnType("decimal(18,2)");

            entity.HasIndex(o => o.OrderNumber).IsUnique();

            entity.HasMany(o => o.OrderItems)
                  .WithOne(oi => oi.Order)
                  .HasForeignKey(oi => oi.OrderId)
                  .OnDelete(DeleteBehavior.Cascade);

            //using to make global filter as not get deleted items
            entity.HasQueryFilter(m => !m.IsDeleted);
        }
    }
}
