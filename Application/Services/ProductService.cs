
using Application.Exceptions;
using Application.Extensions;
using Domains.DTO;
using Domains.Interfaces.IServices;
using Domains.Interfaces.IUnitofWork;
using Domains.Models;
using Domains.Models.BridgeEntity;
using Microsoft.EntityFrameworkCore;


namespace Application.Services
{
    public class ProductService : IProductService
    {

        private readonly IUnitOfWork _unitOfWork;
        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedList<ProductResponseDTO>> GetPaginatedProductsAsync(int pageIndex, int pageSize)
        {
            var productsQuery = await _unitOfWork.GetGenericRepository<Product>().GetAllAsync();
            var products = productsQuery.Include(p => p.ProductCategories);
            var productDTOs = products.Select(p => new ProductResponseDTO
            (
                p.Id,
                p.Name,
                p.Price,
                p.StockQuantity,
                p.Description,
                p.ProductCategories.Select(c => c.CategoryId).ToList(),
                p.CreatedDate.ToFormattedString("yyyy-MM-dd HH:mm:ss"),
                p.ModifiedDate.ToFormattedString("yyyy-MM-dd HH:mm:ss")
            ));
            return await productDTOs.ToPaginatedListAsync(pageIndex, pageSize);
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
            await _unitOfWork.GetGenericRepository<Product>().AddAsync(product);
            await _unitOfWork.SaveAsync();

            var productResponse = new ProductResponseDTO
              (
                  Id: product.Id,
                  Name: product.Name,
                  Price: product.Price,
                  StockQuantity: product.StockQuantity,
                  Description: product.Description,
                  CategoryIds: product.ProductCategories.Select(pc => pc.CategoryId).ToList(),
                  CreatedAt: product.CreatedDate.ToFormattedString(),
                  ModifiedAt: product.ModifiedDate.ToFormattedString()

              );
            return productResponse;
        }

        public async Task DeleteProductAsync(int id)
        {
            await _unitOfWork.GetGenericRepository<Product>().DeleteAsync(id);
            await _unitOfWork.SaveAsync();
        }
        public async Task SoftDeleteProductAsync(int id)
        {
            await _unitOfWork.GetGenericRepository<Product>().SoftDeleteAsync(id);
            await _unitOfWork.SaveAsync();
        }
        public async Task<ProductResponseDTO> GetProductByIdAsync(int id)
        {
            var productIqueryable = await _unitOfWork.GetGenericRepository<Product>().GetAllAsync();
            var product = await productIqueryable.Include(p => p.ProductCategories).FirstOrDefaultAsync(p => p.Id == id);


            if (product == null)
            {
                throw new NotFoundException($"Product with ID {id} not found.");
            }

            var productResponse = new ProductResponseDTO
            (
                Id: product.Id,
                Name: product.Name,
                Price: product.Price,
                StockQuantity: product.StockQuantity,
                Description: product.Description,
                CategoryIds: product.ProductCategories.Select(pc => pc.CategoryId).ToList(),
                CreatedAt: product.CreatedDate.ToFormattedString(),
                ModifiedAt: product.ModifiedDate.ToFormattedString()
            );
            return productResponse;
        }

        public async Task<List<ProductStockDTO>> ReduceStockQuantitiesAsync(List<OrderDetailPostDTO> orderDetails)
        {

            var productList = await _unitOfWork.GetGenericRepository<Product>().GetQueryable().Where(x => orderDetails.Select(y => y.ProductId).Contains(x.Id)).ToListAsync();

            if (!productList.Any())
            {
                throw new NotFoundException("No products found for the given order details.");
            }
            // NULL CHECK Product List

            var productStockList = (from prod in productList
                                    join order in orderDetails on prod.Id equals order.ProductId
                                    let temp = prod.StockQuantity < order.Quantity
                                    where temp == false
                                    select new ProductStockDTO
                                    {
                                        ProductId = prod.Id,
                                        RemainingStock = prod.StockQuantity - order.Quantity,
                                    }).ToList();

            if (!productStockList.Any())
            {
                throw new Exception("Insufficient stock for some products.");
            }
            //Null Check for productStockList
            foreach (var product in productList)
            {
                var stockDTO = productStockList.First(ps => ps.ProductId == product.Id);
                product.StockQuantity = stockDTO.RemainingStock;
                await _unitOfWork.GetGenericRepository<Product>().UpdateAsync(product);
            }
            await _unitOfWork.SaveAsync();

            //foreach (var updateProduct in productList)
            //{

            //    updateProduct.StockQuantity = productStockList.FirstOrDefault(x => x.ProductId == updateProduct.Id)!.RemainingStock;
            //    await _repository.UpdateAsync(updateProduct);
            //}




            //foreach (var detail in orderDetails)
            //{
            //    var product = await _repository.GetByIdAsync(detail.ProductId);

            //    if (product == null)
            //    {
            //        throw new Exception($"Product with ID {detail.ProductId} not found");
            //    }

            //    if (product.StockQuantity < detail.Quantity)
            //    {
            //        throw new Exception($"Insufficient stock for Product ID {detail.ProductId}");
            //    }

            //    product.StockQuantity -= detail.Quantity;
            //    await _repository.UpdateAsync(product);

            //    productStockList.Add(new ProductStockDTO
            //    {
            //        ProductId = product.Id,
            //        RemainingStock = product.StockQuantity
            //    });
            //}

            return productStockList;
        }

        public async Task<IEnumerable<ProductResponseDTO>> GetProductsAsync()
        {
            var productsQuery = await _unitOfWork.GetGenericRepository<Product>().GetAllAsync();  // still not executed


            var products = await productsQuery.Include(p => p.ProductCategories).ToListAsync();            // executed here, making it eager loading because we want to include the product categories

            return products.Select(p => new ProductResponseDTO // for each product in productwithCategory, we are creating a new ProductResponseDTO object
            (
                Id: p.Id,
                Name: p.Name,
                Price: p.Price,
                StockQuantity: p.StockQuantity,
                Description: p.Description,
                CategoryIds: p.ProductCategories.Select(pc => pc.CategoryId).ToList(),
                CreatedAt: p.CreatedDate.ToFormattedString(),
                ModifiedAt: p.ModifiedDate.ToFormattedString()

            )).ToList();
        }
        public async Task UpdateProductPatchAsync(int id, ProductPatchDTO productDto)
        {
            var product = await _unitOfWork.GetGenericRepository<Product>().GetByIdAsync(id);

            if (product == null)
            {
                throw new NotFoundException($"Product with ID {id} not found");

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
            await _unitOfWork.GetGenericRepository<Product>().UpdateAsync(product);
            await _unitOfWork.SaveAsync();
        }
    }
}
