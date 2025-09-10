namespace ERestaurant.Application.Feartures.Orders.Dtos
{
    public record CreateUpdateOrderDto
    {
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerMobile { get; set; } = string.Empty;
        public bool IsActive { get; set; }

        public List<OrderItemDto> OrderItems { get; set; }
    }
}
