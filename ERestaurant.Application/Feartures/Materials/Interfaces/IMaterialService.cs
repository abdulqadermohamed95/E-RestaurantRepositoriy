using ERestaurant.Application.Common.Dtos;
using ERestaurant.Application.Common.Pageination;
using ERestaurant.Application.Feartures.Materials.Dtos;

namespace ERestaurant.Application.Feartures.Materials.Interfaces
{
    public interface IMaterialService
    {
        Task<MaterialDto?> GetByIdAsync(Guid id);
        Task<PagedResult<MaterialDto>> GetAllAsync(MaterialFilterDto filter);
        Task<MaterialDto> CreateAsync(CreateUpdateMaterialDto dto);
        Task<bool> UpdateAsync(Guid id, CreateUpdateMaterialDto dto);
        Task<bool> DeleteAsync(Guid id);
        Task<IEnumerable<LookupDto>> GetMaterialLookupAsync();
    }
}
