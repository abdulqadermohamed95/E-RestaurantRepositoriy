using AutoMapper;
using ERestaurant.Application.Feartures.Combos.Services;
using ERestaurant.Application.Feartures.Materials.Dtos;
using ERestaurant.Application.Feartures.Materials.Services;
using ERestaurant.Domain.Entities.Materials;
using ERestaurant.Domain.Entities.Materials.Enums;
using ERestaurant.Infrastructure.Persistence;
using ERestaurant.Infrastructure.Persistence.Repositories;
using ERestaurant.Tests.Common;
using Microsoft.EntityFrameworkCore;


namespace ERestaurant.Tests.Materials
{
    public partial class MaterialServiceTests : IDisposable
    {
        private readonly ERestaurantDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly MaterialService _service;

        public MaterialServiceTests()
        {
            var options = new DbContextOptionsBuilder<ERestaurantDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) 
                .Options;

            _dbContext = new ERestaurantDbContext(options, new FakeCurrentUserService());

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CreateUpdateMaterialDto, Material>();
                cfg.CreateMap<Material, MaterialDto>();
            });
            _mapper = config.CreateMapper();

            var unitOfWork = new UnitOfWork(_dbContext);

            var fakeCultureService = new FakeCultureService();
            _service = new MaterialService(unitOfWork, _mapper, fakeCultureService);
        }

        [Fact]
        public async Task CreateAsync_Should_Add_New_Material()
        {
            var dto = new CreateUpdateMaterialDto
            {
                NameAr = "لحم",
                NameEn = "Meat",
                Price = 10,
                Tax = 2,
                Unit = UnitType.Piece,
                IsActive = true
            };

            var result = await _service.CreateAsync(dto);

            Assert.NotNull(result);
            Assert.Equal("Meat", result.NameEn);
            Assert.NotEqual(Guid.Empty, result.Id); 
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_Material()
        {
            var material = new Material { NameEn = "Banana", NameAr = "موز", Price = 5, Tax = 1, Unit = UnitType.Piece, IsActive = true };
            _dbContext.Materials.Add(material);
            await _dbContext.SaveChangesAsync();

            var result = await _service.GetByIdAsync(material.Id);

            Assert.NotNull(result);
            Assert.Equal("Banana", result!.NameEn);
        }

        [Fact]
        public async Task GetAllAsync_Should_Filter_By_Name()
        {
            _dbContext.Materials.AddRange(
                new Material { NameEn = "Orange", NameAr = "برتقال", Price = 7, Tax = 1.5m, Unit = UnitType.Piece, IsActive = true },
                new Material { NameEn = "Banana", NameAr = "موز", Price = 5, Tax = 1, Unit = UnitType.Piece, IsActive = true }
            );
            await _dbContext.SaveChangesAsync();

            var filter = new MaterialFilterDto { Name = "Banana", PageNumber = 1, PageSize = 10 };
            var result = await _service.GetAllAsync(filter);

            Assert.Single(result.Items);
            Assert.Equal("Banana", result.Items.First().NameEn);
        }

        [Fact]
        public async Task UpdateAsync_Should_Modify_Material()
        {
            var material = new Material { NameEn = "Peach", NameAr = "خوخ", Price = 12, Tax = 2, Unit = UnitType.Piece, IsActive = true };
            _dbContext.Materials.Add(material);
            await _dbContext.SaveChangesAsync();

            var updateDto = new CreateUpdateMaterialDto
            {
                NameAr = "خوخ معدل",
                NameEn = "Peach Updated",
                Price = 15,
                Tax = 3,
                Unit = UnitType.Piece,
                IsActive = false
            };

            await _service.UpdateAsync(material.Id, updateDto);

            var updated = await _dbContext.Materials.FindAsync(material.Id);
            Assert.NotNull(updated);
            Assert.Equal("Peach Updated", updated!.NameEn);
            Assert.Equal(15, updated.Price);
            Assert.False(updated.IsActive);
        }

        [Fact]
        public async Task DeleteAsync_Should_Mark_Material_As_Deleted()
        {
            var material = new Material
            {
                NameEn = "Grapes",
                NameAr = "عنب",
                Price = 8,
                Tax = 1.2m,
                Unit = UnitType.Piece,
                IsActive = true
            };
            _dbContext.Materials.Add(material);
            await _dbContext.SaveChangesAsync();

            await _service.DeleteAsync(material.Id);

            var deleted = await _dbContext.Materials.IgnoreQueryFilters()
                                                    .FirstOrDefaultAsync(m => m.Id == material.Id);

            Assert.NotNull(deleted);
            Assert.True(deleted.IsDeleted); 
        }
        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
