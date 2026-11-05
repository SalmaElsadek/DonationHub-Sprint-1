using DonationHub.Application.DTOs;
using DonationHub.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DonationHub.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        // GET: api/categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetAll()
        {
            var result = await _categoryService.GetAllAsync();
            return Ok(result);
        }

        // GET: api/categories/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CategoryDTO>>GetById(Guid id)
        {
            var result= await _categoryService.GetByIdAsync(id);
            if(result==null) return NotFound(new { message = "Category.NotFound" });
            return Ok(result);
        }

        // POST: api/categories
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<CategoryDTO>> Create([FromBody] CategoryDTO categoryDTO)
        {
            try
            {
                var result = await _categoryService.CreateAsync(categoryDTO);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // PUT: api/categories/{id}
        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CategoryDTO categoryDto)
        {
            try
            {
                await _categoryService.UpdateAsync(id, categoryDto);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Category.NotFound" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // DELETE: api/categories/{id} 
        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _categoryService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Category.NotFound" });
            }
            catch (InvalidOperationException ex) when (ex.Message == "Category.HasCampaigns")
            {
                return Conflict(new { message = "Category.HasCampaigns" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
