using ERestaurant.Application.Feartures.Combos.Dtos;
using ERestaurant.Application.Feartures.Combos.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace E_Restaurant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CombosController : ControllerBase
    {
        #region Properties
        private readonly IComboAppService _comboAppService;
        #endregion

        #region Constructor
        public CombosController(IComboAppService comboAppService)
        {
            _comboAppService = comboAppService;
        }
        #endregion

        #region Methodes

        [HttpPost]
        public async Task<IActionResult> Create(CreateUpdateComboDto dto)
        {
            var combo = await _comboAppService.CreateAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = combo.Id }, combo);
        }
    
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, CreateUpdateComboDto dto)
        {
            var combo = await _comboAppService.UpdateAsync(id, dto);
            return combo ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var combo = await _comboAppService.DeleteAsync(id);
            return combo ? NoContent() : NotFound();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var combo = await _comboAppService.GetAsync(id);
            if (combo == null) return NotFound();

            return Ok(combo);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] ComboFilterDto filter)
        {
            var combos = await _comboAppService.GetAllAsync(filter);
            return Ok(combos);
        }

        [HttpGet("GetComboLookup")]
        public async Task<IActionResult> GetComboLookupAsync()
        {
            var lookup = await _comboAppService.GetComboLookupAsync();
            return Ok(lookup);
        }

        #endregion
    }
}
