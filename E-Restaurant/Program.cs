using E_Restaurant.Middlewares;
using E_Restaurant.Services;
using ERestaurant.Application;
using ERestaurant.Application.Common.Interfaces;
using ERestaurant.Application.Feartures.Combos.Interfaces;
using ERestaurant.Application.Feartures.Combos.Services;
using ERestaurant.Application.Feartures.Materials.Interfaces;
using ERestaurant.Application.Feartures.Materials.Services;
using ERestaurant.Application.Feartures.Orders.Interfaces;
using ERestaurant.Application.Feartures.Orders.Services;
using ERestaurant.Application.Mapping;
using ERestaurant.Infrastructure.Persistence;
using ERestaurant.Infrastructure.Persistence.Repositories;
using ERestaurant.Infrastructure.Seeder;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

#region Database
// Add DbContext
builder.Services.AddDbContext<ERestaurantDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
#endregion

#region Application Layer
// Register all mappings in Application layer
builder.Services.AddApplicationMappings();

// Scan validators from Application assembly
builder.Services.AddFluentValidationAutoValidation()
               .AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssembly(typeof(AssemblyReference).Assembly);
#endregion

#region Repositories & Unit of Work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
#endregion

#region Application Services
builder.Services.AddScoped<IMaterialService, MaterialService>();
builder.Services.AddScoped<IComboAppService, ComboAppService>();
builder.Services.AddScoped<IOrderService, OrderService>();
#endregion

#region HttpContext / User
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<ICurrentCultureService, CurrentCultureService>();
#endregion

#region Controllers & Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
#endregion

#region Localization 
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

var supportedCultures = new[] { "en", "ar" };
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.SetDefaultCulture("en")
           .AddSupportedCultures(supportedCultures)
           .AddSupportedUICultures(supportedCultures);
});
#endregion

var app = builder.Build();

#region Data Seeding
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ERestaurantDbContext>();
    await DataSeeder.SeedAsync(dbContext);
}
#endregion

#region Middlewares
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<TenantMiddleware>();
#endregion

#region HTTP Request Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
#endregion

#region Localization SetUp
app.UseRequestLocalization();
#endregion

app.Run();
