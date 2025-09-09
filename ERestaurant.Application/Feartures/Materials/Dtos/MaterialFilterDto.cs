using ERestaurant.Domain.Entities.Materials.Enums;

namespace ERestaurant.Application.Feartures.Materials.Dtos
{
    public class MaterialFilterDto
    {
        public string? Name { get; set; }    
        public UnitType? Unit { get; set; }
        public bool? IsActive { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
