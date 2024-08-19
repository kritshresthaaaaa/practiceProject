
using Domains.DTO;

namespace Domains.Interfaces.IServices
{
    public interface IOrderDetailService
    {
        Task<IEnumerable<OrderDetailResponseDTO>> GetOrderDetailsAsync();
        Task<OrderDetailFromOrderIdResponseDTO> GetOrderDetailByOrderIdAsync(int orderId);
    }
}
