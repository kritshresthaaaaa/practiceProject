using Domains.Models;
using Infrastructure.DTO;
namespace WebHost.Services.IServices
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryResponseDTO>> GetCategoriesAsync();
        Task<CategoryResponseDTO> GetCategoryByIdAsync(int id);
        Task<CategoryResponseDTO> CreateCategoryAsync(CategoryPostDTO categoryPostDto);
        Task UpdateCategoryAsync(int id, Category categoryDto);
        Task SoftDeleteCategoryAsync(int id);
    }
}
