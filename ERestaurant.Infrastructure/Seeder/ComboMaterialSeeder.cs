using ERestaurant.Domain.Entities.Combos;
using ERestaurant.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

public static class ComboMaterialSeeder
{
    public static async Task SeedAsync(ERestaurantDbContext context)
    {
        await context.Database.MigrateAsync();

        if (await context.ComboMaterials.AnyAsync())
            return;

        var combo = await context.Combos.FirstOrDefaultAsync();
        var material = await context.Materials.FirstOrDefaultAsync();

        if (combo is not null && material is not null)
        {
            var comboMaterials = new List<ComboMaterial>
            {
                new ComboMaterial
                {
                    ComboId = combo.Id,
                    MaterialId = material.Id,
                    Quantity = 1
                }
            };

            await context.ComboMaterials.AddRangeAsync(comboMaterials);
            await context.SaveChangesAsync();
        }
    }
}
