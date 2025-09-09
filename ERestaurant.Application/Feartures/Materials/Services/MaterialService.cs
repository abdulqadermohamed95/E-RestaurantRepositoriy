using AutoMapper;
using ERestaurant.Application.Common.Dtos;
using ERestaurant.Application.Common.Interfaces;
using ERestaurant.Application.Common.Pageination;
using ERestaurant.Application.Feartures.Materials.Dtos;
using ERestaurant.Application.Feartures.Materials.Interfaces;
using ERestaurant.Domain.Entities.Materials;
using Microsoft.EntityFrameworkCore;

namespace ERestaurant.Application.Feartures.Materials.Services
{
    public class MaterialService : IMaterialService
    {
        #region Properties
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly string _culture;
        #endregion

        #region Constructor
        public MaterialService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentCultureService cultureService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _culture = cultureService.GetCurrentLanguage();
        }
        #endregion

        #region Methodes For Bussiness

        /// <summary>
        /// Create New Material 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<MaterialDto> CreateAsync(CreateUpdateMaterialDto dto)
        {
            var material = _mapper.Map<Material>(dto);
            await _unitOfWork.Repository<Material>().AddAsync(material);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<MaterialDto>(material);
        }

        /// <summary>
        /// Update Material
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> UpdateAsync(Guid id, CreateUpdateMaterialDto dto)
        {
            var repo = _unitOfWork.Repository<Material>();
            var material = await repo.GetByIdAsync(id);

            if (material is null) return false;

            _mapper.Map(dto, material);
            await repo.UpdateAsync(material);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Delete Material by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(Guid id)
        {
            var repo = _unitOfWork.Repository<Material>();
            var material = await repo.GetByIdAsync(id);

            if (material is null) return false;

            await repo.DeleteAsync(material);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Get All Materials
        /// </summary>
        /// <param name="search"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<PagedResult<MaterialDto>> GetAllAsync(MaterialFilterDto filter)
        {
            var query = _unitOfWork.Repository<Material>().Query();

            // Searching Using Name
            if (!string.IsNullOrEmpty(filter.Name))
            {
                query = _culture == "ar"
                                 ? query.Where(c => c.NameAr.Contains(filter.Name))
                                 : query.Where(c => c.NameEn.Contains(filter.Name));
            }

            // Searching Using UnitType
            if (filter.Unit.HasValue)
                query = query.Where(m => m.Unit == filter.Unit.Value);

            // Searching Using Status
            if (filter.IsActive.HasValue)
                query = query.Where(m => m.IsActive == filter.IsActive.Value);

            // Apply ordering, paging, mapping
            var totalCount = await query.CountAsync();
            var items = await query
                      .OrderBy(m => m.NameEn) // or dynamic ordering
                      .Skip((filter.PageNumber - 1) * filter.PageSize)
                      .Take(filter.PageSize)
                      .ToListAsync();

            var dtoItems = _mapper.Map<List<MaterialDto>>(items);

            foreach (var dto in dtoItems)
                dto.Name = _culture == "ar" ? dto.NameAr : dto.NameEn;

            return new PagedResult<MaterialDto>(dtoItems, totalCount, filter.PageNumber, filter.PageSize);
        }

        /// <summary>
        /// Get Material By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<MaterialDto?> GetByIdAsync(Guid id)
        {
            var material = await _unitOfWork.Repository<Material>().GetByIdAsync(id);

            if (material is null)
                throw new KeyNotFoundException($"Material with Id {id} not found.");

            var materialDto = _mapper.Map<MaterialDto>(material);

            materialDto.Name = _culture == "ar" ? materialDto.NameAr : materialDto.NameEn;

            return materialDto;
        }

        /// <summary>
        /// Get Lookups for Material
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<LookupDto>> GetMaterialLookupAsync()
        {
            var materials = await _unitOfWork.Repository<Material>()
                                             .Query()
                                             .ToListAsync();

            return materials.Select(m => new LookupDto
            {
                Id = m.Id,
                Name = _culture == "ar" ? m.NameAr : m.NameEn
            });
        }

        #endregion
    }
}
