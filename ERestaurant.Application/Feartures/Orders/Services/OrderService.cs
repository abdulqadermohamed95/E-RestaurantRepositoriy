using AutoMapper;
using ERestaurant.Application.Common.Interfaces;
using ERestaurant.Application.Common.Pageination;
using ERestaurant.Application.Feartures.Orders.Dtos;
using ERestaurant.Application.Feartures.Orders.Interfaces;
using ERestaurant.Domain.Entities.Orders;
using Microsoft.EntityFrameworkCore;

namespace ERestaurant.Application.Feartures.Orders.Services
{
    public class OrderService : IOrderService
    {
        #region Properties
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        #endregion

        #region Constructor
        public OrderService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        #endregion

        #region Methodes For Bussiness

        /// <summary>
        /// Create New Order
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<OrderDto> CreateAsync(CreateUpdateOrderDto input)
        {
            var repo = _unitOfWork.Repository<Order>();
            var order = _mapper.Map<Order>(input);

            order.Id = Guid.NewGuid();
            order.OrderNumber = $"ORDER-{DateTime.UtcNow:MMddyyyyhhmmss}";
            order.OrderDate = DateTime.UtcNow;

            order.OrderItems = input.OrderItems.Select(i => new OrderItem
            {
                Id = Guid.NewGuid(),
                MaterialId = i.MaterialId,
                ComboId = i.ComboId,
                UnitPrice = i.UnitPrice,
                Quantity = i.Quantity,
                TotalPrice = i.UnitPrice * i.Quantity
            }).ToList();

            order.RecalculateTotals();

            await repo.AddAsync(order);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<OrderDto>(order);
        }

        /// <summary>
        /// Update Order 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> UpdateAsync(Guid id, CreateUpdateOrderDto input)
        {
            var orderRepo = _unitOfWork.Repository<Order>();
            var orderItemRepo = _unitOfWork.Repository<OrderItem>();

            var order = await orderRepo.Query()
                                       .Include(o => o.OrderItems)
                                       .SingleOrDefaultAsync(o => o.Id == id);

            if (order is null) return false;

            _mapper.Map(input, order);

            foreach (var item in order.OrderItems)
                item.IsDeleted = true;

            foreach (var i in input.OrderItems)
            {
                var newItem = new OrderItem
                {
                    OrderId = order.Id,
                    MaterialId = i.MaterialId,
                    ComboId = i.ComboId,
                    UnitPrice = i.UnitPrice,
                    Quantity = i.Quantity,
                    TotalPrice = i.UnitPrice * i.Quantity,
                    IsDeleted = false,
                };

                await orderItemRepo.AddAsync(newItem);
            }

            order.RecalculateTotals();

            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Delete Order According Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> DeleteAsync(Guid id)
        {
            var repo = _unitOfWork.Repository<Order>();
            var orderItemRepo = _unitOfWork.Repository<OrderItem>();

            var order = await repo.Query()
                                       .Include(o => o.OrderItems)
                                       .SingleOrDefaultAsync(o => o.Id == id);

            if (order is null) return false;

            foreach (var item in order.OrderItems)
                item.IsDeleted = true;

            await repo.DeleteAsync(order);

            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Get All Order List
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<PagedResult<OrderDto>> GetAllAsync(OrderFilterDto filter)
        {
            var repo = _unitOfWork.Repository<Order>();
            var query = repo.Query().Include(o => o.OrderItems).AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.CustomerName))
                query = query.Where(o => o.CustomerName.Contains(filter.CustomerName.Trim()));

            if (filter.FromDate.HasValue)
                query = query.Where(o => o.OrderDate >= filter.FromDate.Value);

            if (filter.ToDate.HasValue)
                query = query.Where(o => o.OrderDate <= filter.ToDate.Value);

            if (filter.IsActive.HasValue)
                query = query.Where(o => o.IsActive == filter.IsActive.Value);

            var total = await query.CountAsync();

              var items = await query
                      .OrderByDescending(o => o.OrderDate)
                      .Skip((filter.PageNumber - 1) * filter.PageSize)
                      .Take(filter.PageSize)
                      .ToListAsync();

            return new PagedResult<OrderDto>(
                       items.Select(_mapper.Map<OrderDto>).ToList(),
                       total,
                       filter.PageNumber,
                       filter.PageSize
            );
        }

        /// <summary>
        /// Get Order By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<OrderDto> GetByIdAsync(Guid id)
        {
            var repo = _unitOfWork.Repository<Order>();
            var order = await repo.Query()
                                  .Include(o => o.OrderItems)
                                  .SingleOrDefaultAsync(o => o.Id == id);

            if (order is null)
                throw new KeyNotFoundException($"Order with Id {id} not found.");

            return _mapper.Map<OrderDto>(order);
        }

        #endregion
    }
}
