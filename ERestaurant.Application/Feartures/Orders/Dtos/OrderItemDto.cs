namespace ERestaurant.Application.Feartures.Orders.Dtos
{
    public record OrderItemDto
    {
        public Guid? MaterialId { get; set; }
        public Guid? ComboId { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
    }
}
