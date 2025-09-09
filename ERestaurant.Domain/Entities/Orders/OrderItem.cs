using ERestaurant.Domain.Common;
using ERestaurant.Domain.Entities.Orders;

public class OrderItem : BaseEntity
{
    public Guid OrderId { get; set; }
    public virtual Order Order { get; set; }

    public Guid? MaterialId { get; set; }
    public Guid? ComboId { get; set; }

    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }
}
