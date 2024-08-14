
using Infrastructure.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebHost.Services.IServices;

namespace InventoryMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductResponseDTO>>> GetProducts()
        {
            var products = await _productService.GetProductsAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductResponseDTO>> GetProduct(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<ProductResponseDTO>> CreateProduct([FromBody] ProductPostDTO productPostDTO)
        {
            var newProduct = await _productService.CreateProductAsync(productPostDTO);

            if (newProduct == null)
            {
                return BadRequest();
            }
            

            return CreatedAtAction(nameof(GetProduct), new { id = newProduct.Id }, newProduct);
        }
        [HttpPatch("{id}")]
        public async Task<ActionResult> UpdateProductPatch(int id, [FromBody] ProductPatchDTO productDTO)
        {
            await _productService.UpdateProductPatchAsync(id, productDTO);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> SoftDeleteProduct(int id)
        {
            await _productService.SoftDeleteProductAsync(id);
            return NoContent();
        }
    }
}
