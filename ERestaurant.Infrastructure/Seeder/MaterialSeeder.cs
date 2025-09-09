using ERestaurant.Domain.Entities.Materials;
using ERestaurant.Domain.Entities.Materials.Enums;
using ERestaurant.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ERestaurant.Infrastructure.Seeder
{
    public static class MaterialSeeder
    {
        public static async Task SeedAsync(ERestaurantDbContext context)
        {
            await context.Database.MigrateAsync();

            if (await context.Materials.AnyAsync())
                return;

            var materials = new List<Material>
                {
                    new Material
                    {
                        NameEn = "Burger Meat",
                        NameAr = "برجر لحم",
                        Price = 150.75m,
                        Unit = UnitType.Piece,
                        Tax = 5.00m,
                        IsActive = true,
                        IsDeleted = false,
                        ImageUrl = null
                    },
                };

            await context.Materials.AddRangeAsync(materials);
            await context.SaveChangesAsync();
        }
    }
}
