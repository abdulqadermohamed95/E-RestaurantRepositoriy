using ERestaurant.Application.Feartures.Orders.Dtos;
using ERestaurant.Application.Feartures.Orders.Interfaces;
using ERestaurant.Domain.Entities.Orders;
using Microsoft.AspNetCore.Mvc;

namespace E_Restaurant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        #region Properties
        private readonly IOrderService _orderService;
        #endregion

        #region Constructor
        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        #endregion

        #region Methodes

        [HttpPost]
        public async Task<IActionResult> Create(CreateUpdateOrderDto dto)
        {
            var Order = await _orderService.CreateAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = Order.Id }, Order);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, CreateUpdateOrderDto dto)
        {
            var Order = await _orderService.UpdateAsync(id, dto);
            return Order ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var Order = await _orderService.DeleteAsync(id);
            return Order ? NoContent() : NotFound();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var Order = await _orderService.GetByIdAsync(id);
            if (Order == null) return NotFound();

            return Ok(Order);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] OrderFilterDto filter)
        {
            var Orders = await _orderService.GetAllAsync(filter);
            return Ok(Orders);
        }

        #endregion
    }
}
