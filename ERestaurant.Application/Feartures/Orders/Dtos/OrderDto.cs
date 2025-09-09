namespace ERestaurant.Application.Feartures.Orders.Dtos
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerMobile { get; set; } = string.Empty;
        public bool IsActive { get; set; }

        public decimal TotalBeforeTax { get; set; }
        public decimal TotalTax { get; set; }
        public decimal TotalAfterTax { get; set; }

        public List<OrderItemDto> OrderItems { get; set; } = new();
    }
}
