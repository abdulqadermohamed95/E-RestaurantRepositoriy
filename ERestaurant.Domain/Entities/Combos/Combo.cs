using ERestaurant.Domain.Common;

namespace ERestaurant.Domain.Entities.Combos
{
    public class Combo : BaseEntity
    {
        public required string NameAr { get; set; }
        public required string NameEn { get; set; }
        public decimal Price { get; set; }
        public decimal Tax { get; set; }
        public bool IsActive { get; set; }
        public required string ImageUrl { get; set; }

        //Make Relation M-M Between Material And Combo
        public ICollection<ComboMaterial> ComboMaterials { get; set; } = new List<ComboMaterial>();
    }
}
