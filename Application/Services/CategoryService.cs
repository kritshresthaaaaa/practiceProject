﻿using Application.Exceptions;
using Domains.DTO;
using Domains.Interfaces.IGenericRepository;
using Domains.Interfaces.IServices;
using Domains.Models;

namespace Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IGenericRepository<Category> _repository;
        public CategoryService(IGenericRepository<Category> repository)
        {
            _repository = repository;
        }
        public async Task<CategoryResponseDTO> CreateCategoryAsync(CategoryPostDTO categoryPostDto)
        {
            var category = new Category
            {
                CategoryName = categoryPostDto.CategoryName
            };
            await _repository.AddAsync(category);

            var categoryResponse = new CategoryResponseDTO
            (
                Id: category.Id,
                Name: category.CategoryName
            );
            return categoryResponse;

        }
        public async Task<IEnumerable<CategoryResponseDTO>> GetCategoriesAsync()
        {
            var categories = await _repository.GetAllAsync();
            return categories.Select(c => new CategoryResponseDTO
            (
                c.Id,
                c.CategoryName
            )).ToList();

        }
        public async Task SoftDeleteCategoryAsync(int id)
        {
            await _repository.SoftDeleteAsync(id);
        }
        public async Task<CategoryResponseDTO> GetCategoryByIdAsync(int id)
        {
            var category = await _repository.GetByIdAsync(id);
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
        public Task UpdateCategoryAsync(int id, Category categoryDto)
        {
            throw new NotImplementedException();
        }
    }
}
