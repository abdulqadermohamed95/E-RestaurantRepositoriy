using ERestaurant.Infrastructure.Persistence;

namespace ERestaurant.Infrastructure.Seeder
{
    public static class DataSeeder
    {
        public static async Task SeedAsync(ERestaurantDbContext context)
        {
            await MaterialSeeder.SeedAsync(context);
            await ComboSeeder.SeedAsync(context);
            await ComboMaterialSeeder.SeedAsync(context);
            await OrderSeeder.SeedAsync(context);
        }
    }
}
