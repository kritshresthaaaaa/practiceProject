using Domains.DTO;
using Domains.Models;
namespace Domains.Interfaces.IServices
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryResponseDTO>> GetCategoriesAsync();
        Task<CategoryResponseDTO> GetCategoryByIdAsync(int id);
        Task<CategoryResponseDTO> CreateCategoryAsync(CategoryPostDTO categoryPostDto);
        Task UpdateCategoryAsync(int id, CategoryPostDTO categoryPatchDto);
        Task SoftDeleteCategoryAsync(int id);
    }
}
