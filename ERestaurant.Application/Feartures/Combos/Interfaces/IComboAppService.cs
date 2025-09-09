using ERestaurant.Application.Common.Dtos;
using ERestaurant.Application.Common.Pageination;
using ERestaurant.Application.Feartures.Combos.Dtos;

namespace ERestaurant.Application.Feartures.Combos.Interfaces
{
    public interface IComboAppService
    {
        Task<ComboDto> CreateAsync(CreateUpdateComboDto input);
        Task<bool> UpdateAsync (Guid id, CreateUpdateComboDto input);
        Task<bool> DeleteAsync(Guid id);
        Task<ComboDto> GetAsync(Guid id);
        Task<PagedResult<ComboDto>> GetAllAsync(ComboFilterDto filter);
        Task<IEnumerable<LookupDto>> GetComboLookupAsync();
    }
}
