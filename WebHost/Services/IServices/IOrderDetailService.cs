using Infrastructure.DTO;

namespace WebHost.Services.IServices
{
    public interface IOrderDetailService
    {
        Task<IEnumerable<OrderDetailResponseDTO>> GetOrderDetailsAsync();
        Task<OrderDetailFromOrderIdResponseDTO> GetOrderDetailByOrderIdAsync(int orderId);
    }
}
