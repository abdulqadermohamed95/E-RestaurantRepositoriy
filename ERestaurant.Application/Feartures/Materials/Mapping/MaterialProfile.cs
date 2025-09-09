using AutoMapper;
using ERestaurant.Application.Feartures.Materials.Dtos;
using ERestaurant.Domain.Entities.Materials;

namespace ERestaurant.Application.Feartures.Materials.Mapping
{
    public class MaterialProfile : Profile
    {
        public MaterialProfile()
        {
            CreateMap<Material, MaterialDto>().ReverseMap();
            CreateMap<CreateUpdateMaterialDto, Material>().ReverseMap();
        }
    }
}
