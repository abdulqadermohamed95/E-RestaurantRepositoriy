using ERestaurant.Domain.Entities.Materials.Enums;

namespace ERestaurant.Application.Feartures.Combos.Dtos
{
    public class ComboMaterialDto
    {
        public Guid MaterialId { get; set; }
        public int Quantity { get; set; }
        public bool IsOptional { get; set; }
    }
}
