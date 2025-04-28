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
    public async Task<ActionResult<IEnumerable<Category>>> GetAll()
    {
        var categories = await _categoryService.GetAllCategoriesAsync();
        return Ok(categories);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Category>> GetById(int id)
    {
        var category = await _categoryService.GetCategoryByIdAsync(id);

        if (category == null)
            return NotFound();

        return Ok(category);
    }

    [HttpPost]
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