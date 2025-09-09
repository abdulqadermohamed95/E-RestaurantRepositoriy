using ERestaurant.Domain.Common;
using ERestaurant.Domain.Entities.Materials;

namespace ERestaurant.Domain.Entities.Combos
{
    public class ComboMaterial 
    {
        public Guid Id { get; set; }
        public Guid ComboId { get; set; }
        public Combo Combo { get; set; } 

        public Guid MaterialId { get; set; }
        public Material Material { get; set; } 

        public decimal Quantity { get; set; }
        public bool IsOptional { get; set; }
    }
}
