using ERestaurant.Application.Common.Pageination;
using ERestaurant.Application.Feartures.Orders.Dtos;

namespace ERestaurant.Application.Feartures.Orders.Interfaces
{
    public interface IOrderService
    {
        Task<OrderDto> CreateAsync(CreateUpdateOrderDto dto);
        Task<bool> UpdateAsync(Guid id, CreateUpdateOrderDto dto);
        Task<bool> DeleteAsync(Guid id);
        Task<OrderDto> GetByIdAsync(Guid id);
        Task<PagedResult<OrderDto>> GetAllAsync(OrderFilterDto filter);
    }
}
