using Domains.DTO;
using Domains.Interfaces.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebHost.DTO.BaseResponse;


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
            return Ok(new ApiResponse<IEnumerable<CustomerResponseDTO>>(customers));
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerResponseDTO>> GetCustomer(int id)
        {
            var customer = await _customerService.GetCustomerByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return Ok(new ApiResponse<CustomerResponseDTO>(customer));
        }
        [HttpPost]
        public async Task<ActionResult<CustomerResponseDTO>> CreateCustomer([FromBody] CustomerPostDTO customerPostDTO)
        {
            var newCustomer = await _customerService.CreateCustomerAsync(customerPostDTO);
            return CreatedAtAction(nameof(GetCustomer), new { id = newCustomer.CustomerId }, newCustomer);
        }
    }
}
