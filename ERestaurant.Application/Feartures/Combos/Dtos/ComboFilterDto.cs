namespace ERestaurant.Application.Feartures.Combos.Dtos
{
    public class ComboFilterDto
    {
        public string? Name { get; set; }
        public bool? IsActive { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
