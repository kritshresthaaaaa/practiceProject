
using Infrastructure.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebHost.Services.IServices;

namespace InventoryMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerResponseDTO>>> GetCustomers()
        {
            var customers = await _customerService.GetCustomersAsync();
            return Ok(customers);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerResponseDTO>> GetCustomer(int id)
        {
            var customer = await _customerService.GetCustomerByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return Ok(customer);
        }
        [HttpPost]
        public async Task<ActionResult<CustomerResponseDTO>> CreateCustomer([FromBody] CustomerPostDTO customerPostDTO)
        {
            var newCustomer = await _customerService.CreateCustomerAsync(customerPostDTO);
            return CreatedAtAction(nameof(GetCustomer), new { id = newCustomer.CustomerId }, newCustomer);
        }
    }
}
