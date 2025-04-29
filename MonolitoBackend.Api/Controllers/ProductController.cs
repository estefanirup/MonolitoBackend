using Microsoft.AspNetCore.Mvc;
using MonolitoBackend.Api.DTOs;
using MonolitoBackend.Core.Entities;
using MonolitoBackend.Core.Services;
using AutoMapper;

namespace MonolitoBackend.Api.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _productService;
        private readonly IMapper _mapper;

        public ProductsController(ProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        // Get all products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAll()
        {
            var products = await _productService.GetAllProductsAsync();

            var productDtos = _mapper.Map<IEnumerable<ProductDTO>>(products);

            return Ok(productDtos);
        }

        // Get a product by id
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDTO>> GetById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);

            if (product == null)
                return NotFound();

            var productDto = _mapper.Map<ProductDTO>(product);
            return Ok(productDto);
        }

        // Get all products by category
        [HttpGet("by-category/{categoryId:int}")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAllProductsByCategory(int categoryId)
        {
            var products = await _productService.GetProductsByCategoryIdAsync(categoryId);

            if (products == null || !products.Any())
                return NotFound();

            var productDtos = _mapper.Map<IEnumerable<ProductDTO>>(products);
            return Ok(productDtos);
        }

        // Create a new product
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] ProductDTO productDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var product = _mapper.Map<Product>(productDTO);

            await _productService.AddProductAsync(product);

            var createdProductDto = _mapper.Map<ProductDTO>(product);

            return CreatedAtAction(nameof(GetById), new { id = product.Id }, createdProductDto);
        }

        // Update a product by id
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update(int id, [FromBody] ProductDTO productDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingProduct = await _productService.GetProductByIdAsync(id);
            if (existingProduct == null)
                return NotFound();

            _mapper.Map(productDTO, existingProduct);

            await _productService.UpdateProductAsync(existingProduct);

            return NoContent();
        }

        // Delete a product by id
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
                return NotFound();

            await _productService.DeleteProductAsync(id);
            return NoContent();
        }
    }
}
