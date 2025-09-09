using AutoMapper;
using ERestaurant.Application.Feartures.Combos.Dtos;
using ERestaurant.Domain.Entities.Combos;

namespace ERestaurant.Application.Feartures.Combos.Mapping
{
    public class ComboProfile : Profile
    {
        public ComboProfile()
        {
            CreateMap<CreateUpdateComboDto, Combo>()
                .ForMember(dest => dest.ComboMaterials, opt => opt.MapFrom(src =>
                    src.Materials.Select(m => new ComboMaterial
                    {
                        MaterialId = m.MaterialId,
                        Quantity = m.Quantity,
                        IsOptional = m.IsOptional,
                    })));

            CreateMap<Combo, ComboDto>().ReverseMap();

            CreateMap<ComboMaterial, ComboMaterialDto>().ReverseMap();
        }
    }
}
