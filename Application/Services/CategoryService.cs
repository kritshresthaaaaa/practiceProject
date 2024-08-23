using Application.Exceptions;
using Domains.DTO;
using Domains.Interfaces.IGenericRepository;
using Domains.Interfaces.IServices;
using Domains.Interfaces.IUnitofWork;
using Domains.Models;

namespace Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<CategoryResponseDTO> CreateCategoryAsync(CategoryPostDTO categoryPostDto)
        {
            var category = new Category
            {
                CategoryName = categoryPostDto.CategoryName,
                CreatedDate =  DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow,
            };
            await _unitOfWork.GetGenericRepository<Category>().AddAsync(category);
            await _unitOfWork.SaveAsync();

            var categoryResponse = new CategoryResponseDTO
            (
                Id: category.Id,
                Name: category.CategoryName
            );
            return categoryResponse;

        }
        public async Task<IEnumerable<CategoryResponseDTO>> GetCategoriesAsync()
        {
            var categories = await _unitOfWork.GetGenericRepository<Category>().GetAllAsync();
            return categories.Select(c => new CategoryResponseDTO
            (
                c.Id,
                c.CategoryName
            )).ToList();

        }
        public async Task SoftDeleteCategoryAsync(int id)
        {
            await _unitOfWork.GetGenericRepository<Category>().SoftDeleteAsync(id);
            await _unitOfWork.SaveAsync();
        }
        public async Task<CategoryResponseDTO> GetCategoryByIdAsync(int id)
        {
            var category = await _unitOfWork.GetGenericRepository<Category>().GetByIdAsync(id);
            if (category == null)
            {
                throw new NotFoundException($"Category with id {id} not found");
            }
            return new CategoryResponseDTO
            (
                Id: category.Id,
                Name: category.CategoryName
            );
        }
        //left to complete
        public async Task UpdateCategoryAsync(int id, CategoryPostDTO categoryPatchDto)
        {
            var category = await _unitOfWork.GetGenericRepository<Category>().GetByIdAsync(id);
            if (category == null)
            {
                throw new NotFoundException($"Category with id {id} not found");
            }
            category.ModifiedDate = DateTime.UtcNow;
            if (categoryPatchDto.CategoryName != null)
            {
                categoryPatchDto.CategoryName = categoryPatchDto.CategoryName;
            }
            await _unitOfWork.GetGenericRepository<Category>().UpdateAsync(category);
            await _unitOfWork.SaveAsync();
        }
    }
}
