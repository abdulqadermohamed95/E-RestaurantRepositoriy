using ERestaurant.Domain.Entities.Combos;
using ERestaurant.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
public static class ComboSeeder
{
    public static async Task SeedAsync(ERestaurantDbContext context)
    {
        await context.Database.MigrateAsync();

        if (await context.Combos.AnyAsync())
            return;

        var combos = new List<Combo>
            {
                new Combo
                {
                    NameEn = "Burger Combo",
                    NameAr = "وجبة برجر",
                    Price = 250.00m,
                    Tax = 10.00m,
                    IsActive = true,
                    IsDeleted = false,
                    ImageUrl = string.Empty,
                }
            };

        await context.Combos.AddRangeAsync(combos);
        await context.SaveChangesAsync();
    }
}
