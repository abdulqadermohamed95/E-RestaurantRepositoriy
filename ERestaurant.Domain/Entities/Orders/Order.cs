using ERestaurant.Domain.Common;

namespace ERestaurant.Domain.Entities.Orders
{
    public class Order : BaseEntity
    {
        public string OrderNumber { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerMobile { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;

        public decimal TotalBeforeTax { get; private set; }
        public decimal TotalTax { get; private set; }
        public decimal TotalAfterTax { get; private set; }

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        public void RecalculateTotals()
        {
            TotalBeforeTax = OrderItems.Sum(i => i.TotalPrice);
            TotalTax = TotalBeforeTax * 0.14m;
            TotalAfterTax = TotalBeforeTax + TotalTax;
        }
    }
}
