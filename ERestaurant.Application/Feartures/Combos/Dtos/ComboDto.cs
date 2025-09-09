namespace ERestaurant.Application.Feartures.Combos.Dtos
{
    public class ComboDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public required string NameAr { get; set; }
        public required string NameEn { get; set; }
        public decimal Price { get; set; }
        public decimal Tax { get; set; }
        public bool IsActive { get; set; }
        public required string ImageUrl { get; set; }

        public List<ComboMaterialDto> ComboMaterials { get; set; } = new();
    }
}
