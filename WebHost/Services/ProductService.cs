
using Domains.Models;
using Domains.Models.BridgeEntity;
using Infrastructure.DTO;
using Infrastructure.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using WebHost.Services.IServices;

namespace WebHost.Services
{
    public class ProductService : IProductService
    {
        private readonly IGenericRepository<Product> _repository;

        public ProductService(IGenericRepository<Product> repository)
        {
            _repository = repository;
        }


        public async Task<ProductResponseDTO> CreateProductAsync(ProductPostDTO productDto)
        {
            var product = new Product
            {
                Name = productDto.Name,
                Price = productDto.Price,
                StockQuantity = productDto.StockQuantity,
                Description = productDto.Description,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow,
                ProductCategories = productDto.CategoryIds.Select(c => new ProductCategory { CategoryId = c }).ToList()
            };
            await _repository.AddAsync(product);

            var productResponse = new ProductResponseDTO
              (
                  Id: product.Id,
                  Name: product.Name,
                  Price: product.Price,
                  StockQuantity: product.StockQuantity,
                  Description: product.Description,
                  CategoryIds: product.ProductCategories.Select(pc => pc.CategoryId).ToList()
              );
            return productResponse;
        }
        public async Task DeleteProductAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
        public async Task SoftDeleteProductAsync(int id)
        {
            await _repository.SoftDeleteAsync(id);
        }
        public async Task<ProductResponseDTO> GetProductByIdAsync(int id)
        {
            var productIqueryable = await _repository.GetAllAsync();
            var product = await productIqueryable.Include(p => p.ProductCategories).FirstOrDefaultAsync(p => p.Id == id);


            if (product == null)
            {
                return null;
            }

            var productResponse = new ProductResponseDTO
            (
                Id: product.Id,
                Name: product.Name,
                Price: product.Price,
                StockQuantity: product.StockQuantity,
                Description: product.Description,
                CategoryIds: product.ProductCategories.Select(pc => pc.CategoryId).ToList()
            );
            return productResponse;
        }

        public async Task<List<ProductStockDTO>> ReduceStockQuantitiesAsync(List<OrderDetailPostDTO> orderDetails)
        {
            var productStockList = new List<ProductStockDTO>();

            foreach (var detail in orderDetails)
            {
                var product = await _repository.GetByIdAsync(detail.ProductId);

                if (product == null)
                {
                    throw new Exception($"Product with ID {detail.ProductId} not found");
                }

                if (product.StockQuantity < detail.Quantity)
                {
                    throw new Exception($"Insufficient stock for Product ID {detail.ProductId}");
                }

                product.StockQuantity -= detail.Quantity;
                await _repository.UpdateAsync(product);

                productStockList.Add(new ProductStockDTO
                {
                    ProductId = product.Id,
                    RemainingStock = product.StockQuantity
                });
            }

            return productStockList;
        }

        public async Task<IEnumerable<ProductResponseDTO>> GetProductsAsync()
        {
            var products = await _repository.GetAllAsync();  // still not executed


            var productswithCategory = products.Include(p => p.ProductCategories).ToList(); // executed here, making it eager loading because we want to include the product categories

            return productswithCategory.Select(p => new ProductResponseDTO // for each product in productwithCategory, we are creating a new ProductResponseDTO object
            (
                Id: p.Id,
                Name: p.Name,
                Price: p.Price,
                StockQuantity: p.StockQuantity,
                Description: p.Description,
                CategoryIds: p.ProductCategories.Select(pc => pc.CategoryId).ToList()

            )).ToList();
        }

        public async Task UpdateProductPatchAsync(int id, ProductPatchDTO productDto)
        {
            var product = await _repository.GetByIdAsync(id);

            if (product == null)
            {
                throw new KeyNotFoundException("Product not found");
            }
            product.ModifiedDate = DateTime.UtcNow;
            if (productDto.Name != null)
            {
                product.Name = productDto.Name;
            }
            if (productDto.Price != null)
            {
                product.Price = productDto.Price;
            }
            if (productDto.StockQuantity != null)
            {
                product.StockQuantity = productDto.StockQuantity;
            }
            if (productDto.Description != null)
            {
                product.Description = productDto.Description;
            }
            await _repository.UpdateAsync(product);
        }
    }
}
