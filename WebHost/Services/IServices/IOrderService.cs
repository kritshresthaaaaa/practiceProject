
using Infrastructure.DTO;
using WebHost.DTO;

namespace WebHost.Services.IServices
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderResponseDTO>> GetSalesAsync();
        Task<OrderResponseDTO> GetSaleByIdAsync(int id);
        Task<OrderResponseDTO> CreateSaleAsync(OrderPostDTOController saleDto);
        Task UpdateSaleAsync(int id, OrderPostDTO saleDto);
        Task SoftDeleteSaleAsync(int id);
        Task<IEnumerable<OrderResponseDTO>> GetSalesPerCustomerAsync(int customerId);
/*        Task<IEnumerable<Order>> GetSalesPerDateAsync(DateTime date);
        Task<IEnumerable<Order>> GetSalesPerMonthAsync(int year, int month);
        Task<IEnumerable<Order>> GetSalesPerWeekAsync(DateTime startDate);*/
    }
}
