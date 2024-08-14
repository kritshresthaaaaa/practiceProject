using Domains.Models;
using Infrastructure.DTO;
using Infrastructure.Repository.IRepository;
using WebHost.Services.IServices;

namespace WebHost.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IGenericRepository<Customer> _repository;
        public CustomerService(IGenericRepository<Customer> repository)
        {
            _repository = repository;
        }
        public async Task<CustomerResponseDTO> CreateCustomerAsync(CustomerPostDTO customerPostDto)
        {
            var newCustomer = new Customer
            {
                FirstName = customerPostDto.FirstName,
                LastName = customerPostDto.LastName,
                Email = customerPostDto.Email,
                PhoneNumber = customerPostDto.PhoneNumber,
                Address = customerPostDto.Address,

            };
            await _repository.AddAsync(newCustomer);
            return new CustomerResponseDTO
            (
                CustomerId: newCustomer.Id,
                FirstName: newCustomer.FirstName,
                LastName: newCustomer.LastName,
                Email: newCustomer.Email,
                PhoneNumber: newCustomer.PhoneNumber,
                Address: newCustomer.Address
            );
        }

        public Task DeleteCustomerAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<CustomerResponseDTO> GetCustomerByIdAsync(int id)
        {
            var customer = await _repository.GetByIdAsync(id);
            return new CustomerResponseDTO
            (
                CustomerId: customer.Id,
                FirstName: customer.FirstName,
                LastName: customer.LastName,
                Email: customer.Email,
                PhoneNumber: customer.PhoneNumber,
                Address: customer.Address

            );

        }

        public async Task<IEnumerable<CustomerResponseDTO>> GetCustomersAsync()
        {
            var customers = await _repository.GetAllAsync();
            return customers.Select(c => new CustomerResponseDTO
            (
                 c.Id,
                c.FirstName,
                c.LastName,
               c.Email,
               c.PhoneNumber,
                c.Address
            )).ToList();

        }

        public Task UpdateCustomerAsync(int id, CustomerPostDTO customerPostDto)
        {
            throw new NotImplementedException();
        }
    }
}
