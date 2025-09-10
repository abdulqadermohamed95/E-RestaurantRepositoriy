using ERestaurant.Domain.Entities.Materials.Enums;

namespace ERestaurant.Application.Feartures.Materials.Dtos
{
    public record CreateUpdateMaterialDto
    {
        public string NameAr { get; set; } = string.Empty;
        public string NameEn { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public UnitType Unit { get; set; } 
        public decimal Tax { get; set; }
        public bool IsActive { get; set; }
        public string? ImageUrl { get; set; }
    }
}
