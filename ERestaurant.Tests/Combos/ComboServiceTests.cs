                                                          using AutoMapper;
using ERestaurant.Application.Common.Interfaces;
using ERestaurant.Application.Feartures.Combos.Dtos;
using ERestaurant.Application.Feartures.Combos.Services;
using ERestaurant.Domain.Entities.Combos;
using ERestaurant.Domain.Entities.Materials;
using ERestaurant.Domain.Entities.Materials.Enums;
using ERestaurant.Infrastructure.Persistence;
using ERestaurant.Infrastructure.Persistence.Repositories;
using ERestaurant.Tests.Common;
using Microsoft.EntityFrameworkCore;
using static ERestaurant.Tests.Materials.MaterialServiceTests;

namespace ERestaurant.Tests.Combos
{
    public class ComboServiceTests : IDisposable
    {
        private readonly ERestaurantDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ComboAppService _service;
        private readonly ICurrentCultureService _cultureService;


        public ComboServiceTests()
        {
            var options = new DbContextOptionsBuilder<ERestaurantDbContext>()
                        .UseInMemoryDatabase(Guid.NewGuid().ToString())
                        .Options;

            _dbContext = new ERestaurantDbContext(options, new FakeCurrentUserService());

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CreateUpdateComboDto, Combo>()
                   .ForMember(dest => dest.ComboMaterials, opt => opt.MapFrom(src =>
                       src.Materials.Select(m => new ComboMaterial
                       {
                           MaterialId = m.MaterialId,
                           Quantity = m.Quantity,
                           IsOptional = m.IsOptional
                       })));

                cfg.CreateMap<Combo, ComboDto>();
                cfg.CreateMap<ComboMaterial, ComboMaterialDto>();
            });

            _mapper = config.CreateMapper();

            var unitOfWork = new UnitOfWork(_dbContext);

            var fakeCultureService = new FakeCultureService(); 
            _service = new ComboAppService(unitOfWork, _mapper, fakeCultureService);
        }

        [Fact]
        public async Task CreateAsync_Should_Add_New_Combo()
        {
            var material = new Material { Id = Guid.NewGuid(), NameEn = "Apple", NameAr = "تفاح", Price = 5, Tax = 1, Unit = UnitType.Piece, IsActive = true };
            _dbContext.Materials.Add(material);
            await _dbContext.SaveChangesAsync();

            var dto = new CreateUpdateComboDto
            {
                NameAr = " فواكة كومبو",
                NameEn = "Fruit Combo",
                Price = 20,
                Tax = 2,
                IsActive = true,
                ImageUrl = "img.png",
                Materials = new List<ComboMaterialDto>
                {
                    new ComboMaterialDto { MaterialId = material.Id, Quantity = 2, IsOptional = false }
                }
            };

            var result = await _service.CreateAsync(dto);

            Assert.NotNull(result);
            Assert.Equal("Fruit Combo", result.NameEn);
            Assert.NotEqual(Guid.Empty, result.Id);
        } 

        [Fact]
        public async Task GetByIdAsync_Should_Return_Combo()
        {
            var combo = new Combo { NameEn = "TestCombo", NameAr = "كومبو", Price = 15, Tax = 1.5m, IsActive = true, ImageUrl = "test.png" };
            _dbContext.Combos.Add(combo);
            await _dbContext.SaveChangesAsync();

            var result = await _service.GetAsync(combo.Id);

            Assert.NotNull(result);
            Assert.Equal("TestCombo", result!.NameEn);
        }

        [Fact]
        public async Task GetAllAsync_Should_Filter_By_Name()
        {
            _dbContext.Combos.AddRange(
                new Combo { NameEn = "Pizza Combo", NameAr = "كومبو بيتزا", Price = 50, Tax = 5, IsActive = true, ImageUrl = "pizza.png" },
                new Combo { NameEn = "Burger Combo", NameAr = "كومبو برجر", Price = 40, Tax = 4, IsActive = true, ImageUrl = "burger.png" }
            );
            await _dbContext.SaveChangesAsync();

            var filter = new ComboFilterDto { Name = "Pizza", PageNumber = 1, PageSize = 10 };
            var result = await _service.GetAllAsync(filter);

            Assert.Single(result.Items);
            Assert.Equal("Pizza Combo", result.Items.First().NameEn);
        }

        [Fact]
        public async Task UpdateAsync_Should_Modify_Combo_And_Materials()
        {
            var material1 = new Material { Id = Guid.NewGuid(), NameEn = "Tomato", NameAr = "طماطم", Price = 2, Tax = 0.2m, Unit = UnitType.Piece, IsActive = true };
            var material2 = new Material { Id = Guid.NewGuid(), NameEn = "Cheese", NameAr = "جبنة", Price = 3, Tax = 0.3m, Unit = UnitType.Piece, IsActive = true };
            _dbContext.Materials.AddRange(material1, material2);

            var combo = new Combo
            {
                Id = Guid.NewGuid(),
                NameEn = "Salad Combo",
                NameAr = "كومبو سلطة",
                Price = 10,
                Tax = 1,
                IsActive = true,
                ImageUrl = "salad.png",
                ComboMaterials = new List<ComboMaterial>
                {
                    new ComboMaterial { MaterialId = material1.Id, Quantity = 1, IsOptional = false }
                }
            };

            _dbContext.Combos.Add(combo);
            await _dbContext.SaveChangesAsync();

            var updateDto = new CreateUpdateComboDto
            {
                NameAr = "كومبو سلطة معدل",
                NameEn = "Updated Salad Combo",
                Price = 12,
                Tax = 1.5m,
                IsActive = false,
                ImageUrl = "updated.png",
                Materials = new List<ComboMaterialDto>
                {
                    new ComboMaterialDto { MaterialId = material2.Id, Quantity = 2, IsOptional = true }
                }
            };

            var result = await _service.UpdateAsync(combo.Id, updateDto);

            Assert.True(result);
            var updated = await _dbContext.Combos.Include(c => c.ComboMaterials).FirstOrDefaultAsync(c => c.Id == combo.Id);
            Assert.NotNull(updated);
            Assert.Equal("Updated Salad Combo", updated!.NameEn);
            Assert.Single(updated.ComboMaterials);
            Assert.Equal(material2.Id, updated.ComboMaterials.First().MaterialId);
        }

        [Fact]
        public async Task DeleteAsync_Should_SoftDeleteCombo_And_RemoveComboMaterials()
        {
            var material = new Material { Id = Guid.NewGuid(), NameEn = "Lettuce", NameAr = "خس", Price = 1, Tax = 0.1m, Unit = UnitType.Piece, IsActive = true };
            _dbContext.Materials.Add(material);

            var combo = new Combo
            {
                Id = Guid.NewGuid(),
                NameEn = "Veggie Combo",
                NameAr = "كومبو خضار",
                Price = 8,
                Tax = 0.8m,
                IsActive = true,
                ImageUrl = "veggie.png",
                ComboMaterials = new List<ComboMaterial>
                {
                    new ComboMaterial { MaterialId = material.Id, Quantity = 1, IsOptional = false }
                }
            };

            _dbContext.Combos.Add(combo);
            await _dbContext.SaveChangesAsync();

            var result = await _service.DeleteAsync(combo.Id);

            Assert.True(result);

            var deletedCombo = await _dbContext.Combos.IgnoreQueryFilters().FirstOrDefaultAsync(c => c.Id == combo.Id);
            Assert.NotNull(deletedCombo);
            Assert.True(deletedCombo!.IsDeleted);

            var deletedMaterials = await _dbContext.ComboMaterials.Where(cm => cm.ComboId == combo.Id).ToListAsync();
            Assert.Empty(deletedMaterials);
        }
        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
