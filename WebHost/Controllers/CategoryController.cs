
using Domains.DTO;
using Domains.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebHost.DTO.BaseResponse;

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
            return Ok(new ApiResponse<IEnumerable<CategoryResponseDTO>>(categories));
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryResponseDTO>> GetCategory(int id)
        {
            var cateogry = await _categoryService.GetCategoryByIdAsync(id);
            return Ok(new ApiResponse<CategoryResponseDTO>(cateogry));
        }

        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<CategoryResponseDTO>> CreateCategory([FromBody] CategoryPostDTO categoryPostDTO)
        {
            var newCategory = await _categoryService.CreateCategoryAsync(categoryPostDTO);
            /*return CreatedAtAction(nameof(GetCategory), new { id = newCategory.Id }, newCategory);*/
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
