using Infrastructure.DTO;
using System.Collections.Generic;

namespace WebHost.Services.IServices
{
    public interface ICustomerService
    {
        Task<IEnumerable<CustomerResponseDTO>> GetCustomersAsync();
        Task<CustomerResponseDTO> GetCustomerByIdAsync(int id);
        Task<CustomerResponseDTO> CreateCustomerAsync(CustomerPostDTO customerPostDto);
        Task UpdateCustomerAsync(int id, CustomerPostDTO customerPostDto);
        Task DeleteCustomerAsync(int id);

    }
}
