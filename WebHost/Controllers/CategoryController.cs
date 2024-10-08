﻿
using Application.Constants;
using Application.Services;
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
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<CategoryController> _logger;


        public CategoryController(ICategoryService categoryService, ICurrentUserService currentUserService, ILogger<CategoryController> logger)
        {
            _categoryService = categoryService;
            _currentUserService = currentUserService;
            _logger = logger;
        }

        [HttpGet]
        /*        [Authorize(Roles = Roles.User)]
        */
        public async Task<ActionResult<IEnumerable<CategoryResponseDTO>>> GetCategories()
        {
            var userId = _currentUserService.UserId;
            var username = _currentUserService.Username;
            var roles = _currentUserService.Roles;
            _logger.LogInformation("User {Username} with ID {UserId} and Roles {Roles} is accessing categories.", username, userId, string.Join(", ", roles));

            var categories = await _categoryService.GetCategoriesAsync();
            return Ok(new ApiResponse<IEnumerable<CategoryResponseDTO>>(categories));
        }

        [HttpGet("{id}")]
        /*        [Authorize(Roles = Roles.Admin)]
        */
        public async Task<ActionResult<CategoryResponseDTO>> GetCategory(int id)
        {
            var cateogry = await _categoryService.GetCategoryByIdAsync(id);
            return Ok(new ApiResponse<CategoryResponseDTO>(cateogry));
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
        [HttpPatch("{id}")]
        public async Task<ActionResult> UpdateCategory(int id, [FromBody] CategoryPostDTO categoryPatchDto)
        {
            if (id == 0 || categoryPatchDto == null)
            {
                throw new ArgumentNullException();
            }
            await _categoryService.UpdateCategoryAsync(id, categoryPatchDto);
            return NoContent();
        }
        [HttpGet]
        [Route("GetTotalProductsWithCateogry")]
        public async Task<ActionResult<PaginatedList<CategoryWithProductsResponseDTO>>> GetTotalProductsWitheachCategory([FromQuery] int pageIndex = 1, [FromQuery] int pageSize=10)
        {
            var categories = await _categoryService.GetTotalProductsWithCategoryAsync(pageIndex, pageSize);
            return Ok(new ApiResponse<PaginatedList<CategoryWithProductsResponseDTO>>(categories));
        }

    }
}
