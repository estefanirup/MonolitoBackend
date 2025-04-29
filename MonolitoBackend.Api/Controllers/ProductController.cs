using Microsoft.AspNetCore.Mvc;
using MonolitoBackend.Api.DTOs;
using MonolitoBackend.Core.Entities;
using MonolitoBackend.Core.Services;

using System.Text.Json;

namespace MonolitoBackend.Api.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly ProductService _productService;

    public ProductsController(ProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetAll()
    {
        var products = await _productService.GetAllProductsAsync();

        var jsonOptions = new JsonSerializerOptions
        {
            ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve,
            MaxDepth = 64 
        };

        return Ok(JsonSerializer.Serialize(products, jsonOptions));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetById(int id)
    {
        var product = await _productService.GetProductByIdAsync(id);

        if (product == null)
            return NotFound();

        return Ok(product);
    }

    [HttpGet("by-category/{categoryId:int}")]
    public async Task<ActionResult<IEnumerable<Product>>> GetByCategoryId(int categoryId)
    {
        var products = await _productService.GetProductsByCategoryIdAsync(categoryId);
        return Ok(products);
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] ProductDTO productDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var product = new Product
        {
            Name = productDTO.Name,
            Price = productDTO.Price,
            CategoryId = productDTO.CategoryId
        };

        await _productService.AddProductAsync(product);
        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Update(int id, [FromBody] ProductDTO productDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var existingProduct = await _productService.GetProductByIdAsync(id);
        if (existingProduct == null)
            return NotFound();

        existingProduct.Name = productDTO.Name;
        existingProduct.Price = productDTO.Price;
        existingProduct.CategoryId = productDTO.CategoryId;

        await _productService.UpdateProductAsync(existingProduct);
        return NoContent();
    }

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
