using ERestaurant.Domain.Common;
using ERestaurant.Domain.Entities.Materials.Enums;

namespace ERestaurant.Domain.Entities.Materials
{
    public class Material : BaseEntity
    {
        public string NameAr { get; set; } = string.Empty;
        public string NameEn { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public UnitType Unit { get; set; }
        public decimal Tax { get; set; }
        public bool IsActive { get; set; } = true;
        public string? ImageUrl { get; set; }
    }
}
