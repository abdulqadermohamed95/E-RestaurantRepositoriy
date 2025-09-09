using AutoMapper;
using ERestaurant.Application.Common.Dtos;
using ERestaurant.Application.Common.Interfaces;
using ERestaurant.Application.Common.Pageination;
using ERestaurant.Application.Feartures.Combos.Dtos;
using ERestaurant.Application.Feartures.Combos.Interfaces;
using ERestaurant.Domain.Entities.Combos;
using ERestaurant.Domain.Entities.Materials;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace ERestaurant.Application.Feartures.Combos.Services
{
    public class ComboAppService : IComboAppService
    {
        #region Properties
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly string _culture;
        #endregion

        #region Constructor
        public ComboAppService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentCultureService cultureService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _culture = cultureService.GetCurrentLanguage();
        }

        #endregion

        #region Methodes For Bussiness

        /// <summary>
        /// Create New Combo With Insert Into ComboMaterial 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ComboDto> CreateAsync(CreateUpdateComboDto input)
        {
            var combo = _mapper.Map<Combo>(input);

            combo.ComboMaterials = input.Materials.Select(m => new ComboMaterial
            {
                MaterialId = m.MaterialId,
                Quantity = m.Quantity,
                IsOptional = m.IsOptional
            }).ToList();

            await _unitOfWork.Repository<Combo>().AddAsync(combo);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ComboDto>(combo);
        }

        /// <summary>
        /// Update  Combo With Update Into ComboMaterial 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(Guid id, CreateUpdateComboDto input)
        {
            var comboRepo = _unitOfWork.Repository<Combo>();

            var combo = await comboRepo.Query()
                                       .Include(c => c.ComboMaterials)
                                       .FirstOrDefaultAsync(c => c.Id == id);

            if (combo is null) return false;

            _mapper.Map(input, combo);

            combo.ComboMaterials.Clear();

            foreach (var m in input.Materials)
            {
                combo.ComboMaterials.Add(new ComboMaterial
                {
                    ComboId = id,
                    MaterialId = m.MaterialId,
                    Quantity = m.Quantity,
                    IsOptional = m.IsOptional
                });
            }

            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Delete  Combo 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public async Task<bool> DeleteAsync(Guid id)
        {
            var comboRepo = _unitOfWork.Repository<Combo>();
            var comboMaterialRepo = _unitOfWork.Repository<ComboMaterial>();

            var combo = await comboRepo.Query()
                                       .Include(c => c.ComboMaterials)
                                       .FirstOrDefaultAsync(c => c.Id == id);

            if (combo is null) return false;

            await comboRepo.DeleteAsync(combo);
            //Delete related item in combomaterial as hard delete because not need history of table 
            foreach (var cm in combo.ComboMaterials.ToList())
            {
                await comboMaterialRepo.DeleteAsync(cm);
            }

            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Gert Combo 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public async Task<ComboDto> GetAsync(Guid id)
        {
            var combo = await _unitOfWork.Repository<Combo>()
                                         .Query()
                                         .Include(c => c.ComboMaterials)
                                             .ThenInclude(cm => cm.Material)
                                         .SingleOrDefaultAsync(c => c.Id == id);

            if (combo is null)
                throw new KeyNotFoundException($"Combo with Id {id} not found.");

            var dto = _mapper.Map<ComboDto>(combo);

            dto.Name = _culture == "ar" ? dto.NameAr : dto.NameEn;

            return dto;
        }

        /// <summary>
        /// Get All Combo  
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<PagedResult<ComboDto>> GetAllAsync(ComboFilterDto filter)
        {
            IQueryable<Combo> query = _unitOfWork.Repository<Combo>()
                                                 .Query()
                                                 .Include(c => c.ComboMaterials)
                                                     .ThenInclude(cm => cm.Material);

            if (!string.IsNullOrEmpty(filter.Name))
            {
                query = _culture == "ar"
                                 ? query.Where(c => c.NameAr.Contains(filter.Name))
                                 : query.Where(c => c.NameEn.Contains(filter.Name));
            }

            if (filter.IsActive.HasValue)
                query = query.Where(c => c.IsActive == filter.IsActive.Value);

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(c => c.NameEn)
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            var dtoItems = _mapper.Map<List<ComboDto>>(items);

            foreach (var dto in dtoItems)
                dto.Name = _culture == "ar" ? dto.NameAr : dto.NameEn;

            return new PagedResult<ComboDto>
            {
                Items = dtoItems,
                TotalCount = totalCount,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            };
        }

        /// <summary>
        /// Get Combo Lookups
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<LookupDto>> GetComboLookupAsync()
        {
            var combos = await _unitOfWork.Repository<Combo>()
                                          .Query()
                                          .ToListAsync();

            return combos.Select(m => new LookupDto
            {
                Id = m.Id,
                Name = _culture == "ar" ? m.NameAr : m.NameEn
            });
        }

        #endregion
    }
}
