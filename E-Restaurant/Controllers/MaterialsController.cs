using ERestaurant.Application.Feartures.Materials.Dtos;
using ERestaurant.Application.Feartures.Materials.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace E_Restaurant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialsController : ControllerBase
    {
        #region Properties
        private readonly IMaterialService _materialService;
        #endregion

        #region Constructor
        public MaterialsController(IMaterialService materialService)
        {
            _materialService = materialService;
        }
        #endregion

        #region Methodes

        [HttpPost]
        public async Task<IActionResult> Create(CreateUpdateMaterialDto dto)
        {
            var Material = await _materialService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = Material.Id }, Material);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, CreateUpdateMaterialDto dto)
        {
            var Material = await _materialService.UpdateAsync(id, dto);
            return Material ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var Material = await _materialService.DeleteAsync(id);
            return Material ? NoContent() : NotFound();
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var material = await _materialService.GetByIdAsync(id);
            return Ok(material);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] MaterialFilterDto filter)
        {
            var Materials = await _materialService.GetAllAsync(filter);
            return Ok(Materials);
        }

        [HttpGet("GetMaterialLookup")]
        public async Task<IActionResult> GetMaterialLookupAsync()
        {
            var lookup = await _materialService.GetMaterialLookupAsync();
            return Ok(lookup);
        }
    }
    #endregion
}
