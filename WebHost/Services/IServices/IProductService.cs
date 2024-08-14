using Infrastructure.DTO;
namespace WebHost.Services.IServices
{
    public interface IProductService
    {
        Task<IEnumerable<ProductResponseDTO>> GetProductsAsync();
        Task<ProductResponseDTO> GetProductByIdAsync(int id);
        Task<ProductResponseDTO> CreateProductAsync(ProductPostDTO productDto);
        Task UpdateProductPatchAsync(int id, ProductPatchDTO productDto);
        Task DeleteProductAsync(int id);
        Task SoftDeleteProductAsync(int id);
        Task<List<ProductStockDTO>> ReduceStockQuantitiesAsync(List<OrderDetailPostDTO> orderDetails);

    }
}
