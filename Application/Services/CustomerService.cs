using Application.Exceptions;
using Domains.DTO;
using Domains.Interfaces.IGenericRepository;
using Domains.Interfaces.IServices;
using Domains.Interfaces.IUnitofWork;
using Domains.Models;
namespace Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CustomerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
            await _unitOfWork.GetGenericRepository<Customer>().AddAsync(newCustomer);
            await _unitOfWork.SaveAsync();
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

        public async Task DeleteCustomerAsync(int id)
        {
            await _unitOfWork.GetGenericRepository<Customer>().SoftDeleteAsync(id);
            await _unitOfWork.SaveAsync();
        }

        public async Task<CustomerResponseDTO> GetCustomerByIdAsync(int id)
        {
            var customer = await _unitOfWork.GetGenericRepository<Customer>().GetByIdAsync(id);
            if (customer == null)
            {
                throw new NotFoundException($"Customer with id {id} not found");
            }
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
            var customers = await _unitOfWork.GetGenericRepository<Customer>().GetAllAsync();
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

        public async Task UpdateCustomerAsync(int id, CustomerPostDTO customerPostDto)
        {
            var customer = await _unitOfWork.GetGenericRepository<Customer>().GetByIdAsync(id);

            if (customer == null)
            {
                throw new NotFoundException($"Customer with id {id} not found");
            }
            if (!string.IsNullOrWhiteSpace(customerPostDto.FirstName))
            {
                customer.FirstName = customerPostDto.FirstName;
            }
            if (!string.IsNullOrWhiteSpace(customerPostDto.LastName))
            {
                customer.LastName = customerPostDto.LastName;
            }
            if (!string.IsNullOrWhiteSpace(customerPostDto.Email))
            {
                customer.Email = customerPostDto.Email;
            }
            if (!string.IsNullOrWhiteSpace(customerPostDto.PhoneNumber))
            {
                customer.PhoneNumber = customerPostDto.PhoneNumber;
            }
            if (!string.IsNullOrWhiteSpace(customerPostDto.Address))
            {
                customer.Address = customerPostDto.Address;
            }
            await _unitOfWork.SaveAsync();

        }
    }
}
