using AutoMapper;
using ERestaurant.Application.Feartures.Combos.Dtos;
using ERestaurant.Application.Feartures.Orders.Dtos;
using ERestaurant.Application.Feartures.Orders.Mapping;
using ERestaurant.Application.Feartures.Orders.Services;
using ERestaurant.Domain.Entities.Combos;
using ERestaurant.Domain.Entities.Materials;
using ERestaurant.Domain.Entities.Materials.Enums;
using ERestaurant.Domain.Entities.Orders;
using ERestaurant.Infrastructure.Persistence;
using ERestaurant.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using static ERestaurant.Tests.Materials.MaterialServiceTests;

namespace ERestaurant.Tests.Orders
{
    public class OrderServiceTests : IDisposable
    {
        private readonly ERestaurantDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly OrderService _service;

        public OrderServiceTests()
        {
            var options = new DbContextOptionsBuilder<ERestaurantDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _dbContext = new ERestaurantDbContext(options, new FakeCurrentUserService());

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CreateUpdateOrderDto, Order>()
                     .ForMember(dest => dest.Id, opt => opt.Ignore())
                     .ForMember(dest => dest.OrderNumber, opt => opt.Ignore())
                     .ForMember(dest => dest.OrderDate, opt => opt.Ignore())
                     .ForMember(dest => dest.TotalBeforeTax, opt => opt.Ignore())
                     .ForMember(dest => dest.TotalTax, opt => opt.Ignore())
                     .ForMember(dest => dest.TotalAfterTax, opt => opt.Ignore())
                     .ForMember(dest => dest.OrderItems, opt => opt.Ignore());

                cfg.CreateMap<OrderItemDto, OrderItem>()
                    .ForMember(dest => dest.Id, opt => opt.Ignore())
                    .ForMember(dest => dest.TotalPrice, opt => opt.Ignore());

                cfg.CreateMap<Order, OrderDto>()
                    .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems));

                cfg.CreateMap<OrderItem, OrderItemDto>();
            });


            _mapper = config.CreateMapper();

            var unitOfWork = new UnitOfWork(_dbContext);
            _service = new OrderService(unitOfWork, _mapper);
        }

        [Fact]
        public async Task CreateAsync_Should_Add_New_Order_With_Items()
        {
            var material = new Material
            {
                Id = Guid.NewGuid(),
                NameEn = "Apple",
                NameAr = "تفاح",
                Price = 5,
                Tax = 1,
                Unit = UnitType.Piece,
                IsActive = true
            };
            _dbContext.Materials.Add(material);

            var combo = new Combo
            {
                Id = Guid.NewGuid(),
                NameEn = "Fruit Combo",
                NameAr = "كومبو فواكه",
                Price = 20,
                Tax = 2,
                IsActive = true,
                ImageUrl = "img.png"
            };
            _dbContext.Combos.Add(combo);

            await _dbContext.SaveChangesAsync();

            var dto = new CreateUpdateOrderDto
            {
                CustomerName = "Youssef",
                CustomerMobile = "01273167757",
                OrderItems = new List<OrderItemDto>
                { new OrderItemDto
                    {
                    MaterialId = material.Id,
                    ComboId = combo.Id,
                    UnitPrice = 5,
                    Quantity = 2
                    }
                }
            };

            var result = await _service.CreateAsync(dto);

            // Assert
            Assert.NotNull(result);
            Assert.NotEqual(Guid.Empty, result.Id);

            var order = await _dbContext.Orders.Include(o => o.OrderItems).FirstOrDefaultAsync();
            Assert.Single(order!.OrderItems);
            Assert.Equal(10, order.TotalBeforeTax);
        }

        [Fact]
        public async Task UpdateAsync_Should_SoftDelete_OldItems_And_Add_New_Ones()
        {
            // Arrange
            var material1 = CreateMaterial("Tomato", "طماطم", 2, 0.2m);
            var material2 = CreateMaterial("Cheese", "جبنة", 3, 0.3m);

            _dbContext.Materials.AddRange(material1, material2);

            var order = CreateOrder("Ali", new List<OrderItem>
            {
                CreateOrderItem(material1.Id, 2, 1)
            });

            _dbContext.Orders.Add(order);
            await _dbContext.SaveChangesAsync();

            var updateDto = new CreateUpdateOrderDto
            {
                CustomerName = "Ali Updated",
                OrderItems = new List<OrderItemDto>
                {
                 new OrderItemDto { MaterialId = material2.Id, UnitPrice = 3, Quantity = 2 }
                }
            };

            // Act
            var result = await _service.UpdateAsync(order.Id, updateDto);

            // Assert
            Assert.True(result);

            var updated = await _dbContext.Orders
                        .Include(o => o.OrderItems)
                        .FirstOrDefaultAsync(o => o.Id == order.Id);

            Assert.NotNull(updated);
            Assert.Equal("Ali Updated", updated!.CustomerName);

            var activeItems = updated.OrderItems.Where(i => !i.IsDeleted).ToList();
            Assert.Single(activeItems);
            Assert.Equal(material2.Id, activeItems.First().MaterialId);
        }

        [Fact]
        public async Task DeleteAsync_Should_SoftDelete_Order_And_Items()
        {
            var order = new Order
            {
                Id = Guid.NewGuid(),
                CustomerName = "Test",
                CustomerMobile = "01273167757",
                OrderNumber = $"ORDER-{DateTime.UtcNow:yyyyMMdd}",
                OrderDate = DateTime.UtcNow,
                OrderItems = new List<OrderItem>
                {
                    new OrderItem { MaterialId = Guid.NewGuid(), UnitPrice = 5, Quantity = 1, TotalPrice = 5 }
                }
            };

            _dbContext.Orders.Add(order);
            await _dbContext.SaveChangesAsync();

            var result = await _service.DeleteAsync(order.Id);

            Assert.True(result);
            var deletedOrder = await _dbContext.Orders.IgnoreQueryFilters().FirstOrDefaultAsync(o => o.Id == order.Id);
            Assert.True(deletedOrder!.IsDeleted);

            var deletedItems = await _dbContext.OrderItems.IgnoreQueryFilters().Where(i => i.OrderId == order.Id).ToListAsync();
            Assert.All(deletedItems, i => Assert.True(i.IsDeleted));
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_Order_With_Items()
        {
            var order = new Order
            {
                Id = Guid.NewGuid(),
                CustomerName = "Test Customer",
                CustomerMobile = "01273167757",
                OrderDate = DateTime.UtcNow,
                OrderNumber = $"ORDER-{DateTime.UtcNow:yyyyMMdd}",
                OrderItems = new List<OrderItem>
                {
                    new OrderItem { MaterialId = Guid.NewGuid(), UnitPrice = 10, Quantity = 2, TotalPrice = 20 }
                }
            };

            _dbContext.Orders.Add(order);
            await _dbContext.SaveChangesAsync();

            var result = await _service.GetByIdAsync(order.Id);

            Assert.NotNull(result);
            Assert.Equal("Test Customer", result!.CustomerName);
            Assert.Single(result.OrderItems);
        }

        [Fact]
        public async Task GetAllAsync_With_Filter()
        {
            _dbContext.Orders.AddRange(
                new Order
                {
                    CustomerName = "Ali",
                    CustomerMobile = "0101111111",
                    OrderNumber = $"ORDER-{DateTime.UtcNow:yyyyMMdd}",
                    OrderDate = DateTime.UtcNow,
                    OrderItems = new List<OrderItem>
                {
                    new OrderItem { MaterialId = Guid.NewGuid(), UnitPrice = 10, Quantity = 2, TotalPrice = 20 }
                }
                },
                new Order
                {
                    CustomerName = "Omar",
                    CustomerMobile = "0102222222",
                    OrderNumber = $"ORDER-{DateTime.UtcNow:yyyyMMdd}",
                    OrderDate = DateTime.UtcNow,
                    OrderItems = new List<OrderItem>
                    {
                    new OrderItem { MaterialId = Guid.NewGuid(), UnitPrice = 10, Quantity = 2, TotalPrice = 20 }
                    }
                }
            );
            await _dbContext.SaveChangesAsync();

            var filter = new OrderFilterDto { CustomerName = "Ali", PageNumber = 1, PageSize = 10 };
            var result = await _service.GetAllAsync(filter);

            Assert.Single(result.Items);
            Assert.Equal("Ali", result.Items.First().CustomerName);
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        private static Material CreateMaterial(string nameEn, string nameAr, decimal price, decimal tax) =>
           new Material
           {
               Id = Guid.NewGuid(),
               NameEn = nameEn,
               NameAr = nameAr,
               Price = price,
               Tax = tax,
               Unit = UnitType.Piece,
               IsActive = true
           };

        private static Order CreateOrder(string customerName, List<OrderItem> items) =>
            new Order
            {
                Id = Guid.NewGuid(),
                CustomerName = customerName,
                CustomerMobile = "01273167757",
                OrderNumber = $"ORDER-{DateTime.UtcNow:yyyyMMdd}",
                OrderDate = DateTime.UtcNow,
                OrderItems = items
            };

        private static OrderItem CreateOrderItem(Guid materialId, decimal unitPrice, int quantity) =>
            new OrderItem
            {
                Id = Guid.NewGuid(),
                MaterialId = materialId,
                UnitPrice = unitPrice,
                Quantity = quantity,
                TotalPrice = unitPrice * quantity
            };
    }
}
