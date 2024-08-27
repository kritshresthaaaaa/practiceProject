using Application.Exceptions;
using Application.Extensions;
using Application.Services;
using Domains.DTO;
using Domains.Interfaces.IGenericRepository;
using Domains.Interfaces.IServices;
using Domains.Interfaces.IUnitofWork;
using Domains.Models;
using Domains.Models.BridgeEntity;
using System.Diagnostics;
using System.Xml.Linq;

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
                CreatedDate = DateTime.UtcNow,
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
        public async Task<PaginatedList<CategoryWithProductsResponseDTO>> GetTotalProductsWithCategoryAsync(int pageIndex, int pageSize)
        {
            var categories = await _unitOfWork.GetGenericRepository<Category>().GetAllAsync();
            var products = _unitOfWork.GetGenericRepository<Product>().GetQueryable();
            var productCategories = _unitOfWork.GetGenericRepository<ProductCategory>().GetQueryable();
           var queryPC = from c in categories
                         join pc in productCategories on c.Id equals pc.CategoryId
                         join p in products on pc.ProductId equals p.Id
                         group p by new { c.Id, c.CategoryName } into g
                         select new CategoryWithProductsResponseDTO
                         {
                             Id = g.Key.Id,
                             CategoryName = g.Key.CategoryName,
                             TotalProducts = g.Count(),
                             productResponseDTOs = g.Select(p => new ProductResponseDTO
                             (
                                 p.Id,
                                 p.Name,
                                 p.Price,
                                 p.StockQuantity,
                                 p.Description,
                                 p.ProductCategories.Select(pc => pc.CategoryId).ToList(),
                                 p.CreatedDate.ToString(),
                                 p.ModifiedDate.ToString()
                             )).ToList()
                         };


            return await queryPC.ToPaginatedListAsync(pageIndex, pageSize); ;
        }
    }
}

