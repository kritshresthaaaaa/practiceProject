
using Infrastructure.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebHost.Services.IServices;

namespace InventoryMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryResponseDTO>>> GetCategories()
        {

            var categories = await _categoryService.GetCategoriesAsync();
            return Ok(categories);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryResponseDTO>> GetCategory(int id)
        {
            var cateogry = await _categoryService.GetCategoryByIdAsync(id);
            if (cateogry == null)
            {
                return NotFound();
            }
            return Ok(cateogry);
        }

        [HttpPost]
        public async Task<ActionResult<CategoryResponseDTO>> CreateCategory([FromBody] CategoryPostDTO categoryPostDTO)
        {
            var newCategory = await _categoryService.CreateCategoryAsync(categoryPostDTO);
            return CreatedAtAction(nameof(GetCategory), new { id = newCategory.Id }, newCategory);
        }
        [HttpDelete]
        public async Task<ActionResult> SoftDeleteCategory(int id)
        {
            await _categoryService.SoftDeleteCategoryAsync(id);
            return NoContent();
        }
    }
}
