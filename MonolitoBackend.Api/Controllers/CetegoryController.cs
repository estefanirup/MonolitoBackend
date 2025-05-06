using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MonolitoBackend.Api.DTOs;
using MonolitoBackend.Core.Entities;
using MonolitoBackend.Core.Services;

namespace MonolitoBackend.Api.Controllers;

[ApiController]
[Route("api/categories")]
public class CategoriesController : ControllerBase
{
    private readonly CategoryService _categoryService;

    public CategoriesController(CategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<CategoryWithProductsDTO>>> GetAll()
    {
        var categories = await _categoryService.GetAllCategoriesAsync();

        var categoryDTOs = categories.Select(category => new CategoryWithProductsDTO
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            Products = category.Products.Select(p => new ProductDTO
            {
                Name = p.Name,
                Price = p.Price
            }).ToList()
        });

        return Ok(categoryDTOs);
    }


    [HttpGet("{id:int}")]
    [Authorize(Roles = "Client")] 
    public async Task<ActionResult<Category>> GetById(int id)
    {
        var category = await _categoryService.GetCategoryByIdAsync(id);

        if (category == null)
            return NotFound();

        return Ok(category);
    }

    [HttpPost]
    [Authorize] 
    public async Task<ActionResult> Create([FromBody] CategoryDTO categoryDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var category = new Category
        {
            Name = categoryDTO.Name,
            Description = categoryDTO.Description ?? string.Empty
        };

        await _categoryService.AddCategoryAsync(category);
        return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")] 
    public async Task<ActionResult> Update(int id, [FromBody] CategoryDTO categoryDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var existingCategory = await _categoryService.GetCategoryByIdAsync(id);
        if (existingCategory == null)
            return NotFound();

        existingCategory.Name = categoryDTO.Name;
        existingCategory.Description = categoryDTO.Description;

        await _categoryService.UpdateCategoryAsync(existingCategory);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")] 
    public async Task<ActionResult> Delete(int id)
    {
        var category = await _categoryService.GetCategoryByIdAsync(id);
        if (category == null)
            return NotFound();

        await _categoryService.DeleteCategoryAsync(id);
        return NoContent();
    }

    [HttpGet("{id:int}/products")]
    public async Task<ActionResult<IEnumerable<Product>>> GetProductsByCategory(int id)
    {
        var products = await _categoryService.GetProductsByCategoryIdAsync(id);
        return Ok(products);
    }
}