using ERestaurant.Domain.Entities.Orders;
using ERestaurant.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ERestaurant.Infrastructure.Seeder
{
    public static class OrderSeeder
    {
        public static async Task SeedAsync(ERestaurantDbContext context)
        {
            await context.Database.MigrateAsync();

            if (await context.Orders.AnyAsync())
                return;

            var material = await context.Materials.FirstOrDefaultAsync();
            var combo = await context.Combos.FirstOrDefaultAsync();

            if (material is not null || combo is not null)
            {
                var order = new Order
                {
                    Id = Guid.NewGuid(),
                    CustomerName = "Seeded Customer",
                    CustomerMobile = "01014528786",
                    OrderDate = DateTime.UtcNow,
                    TenantId = Guid.Parse("11111111-1111-1111-1111-111111111111"), 
                    OrderNumber = $"ORDER-{DateTime.UtcNow:MMddyyyyhhmmss}",
                    OrderItems = new List<OrderItem>()
                };

                if (material is not null)
                {
                    order.OrderItems.Add(new OrderItem
                    {
                        Id = Guid.NewGuid(),
                        MaterialId = material.Id,
                        UnitPrice = material.Price,
                        Quantity = 2,
                        TotalPrice = material.Price * 2
                    });
                }

                if (combo is not null)
                {
                    order.OrderItems.Add(new OrderItem
                    {
                        Id = Guid.NewGuid(),
                        ComboId = combo.Id,
                        UnitPrice = combo.Price,
                        Quantity = 1,
                        TotalPrice = combo.Price
                    });
                }

                order.RecalculateTotals();

                await context.Orders.AddAsync(order);
                await context.SaveChangesAsync();
            }
        }
    }
}


