using Application.Services;
using Domains.DTO;

namespace Domains.Interfaces.IServices
{
    public interface IProductService
    {
        Task<IEnumerable<ProductResponseDTO>> GetProductsAsync();
        Task<PaginatedList<ProductResponseDTO>> GetPaginatedProductsAsync(int pageIndex, int pageSize);
        Task<ProductResponseDTO> GetProductByIdAsync(int id);
        Task<ProductResponseDTO> CreateProductAsync(ProductPostDTO productDto);
        Task UpdateProductPatchAsync(int id, ProductPatchDTO productDto);
        Task DeleteProductAsync(int id);
        Task SoftDeleteProductAsync(int id);
        Task<List<ProductStockDTO>> ReduceStockQuantitiesAsync(List<OrderDetailPostDTO> orderDetails);

    }
}
