using MonolitoBackend.Core.Entities;
using MonolitoBackend.Core.Repositories;

namespace MonolitoBackend.Core.Services;

public class ProductService(IProductRepository productRepository)
{
    private readonly IProductRepository _productRepository = productRepository;

    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        return await _productRepository.GetAllAsync();
    }

    public async Task<IEnumerable<Product>> GetProductsByCategoryIdAsync(int categoryId)
    {
        return await _productRepository.GetProductsByCategoryIdAsync(categoryId);
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        return await _productRepository.GetByIdAsync(id);
    }

    public async Task AddProductAsync(Product product)
        => await _productRepository.AddAsync(product);

    public async Task UpdateProductAsync(Product product)
        => await _productRepository.UpdateAsync(product);

    public async Task DeleteProductAsync(int id)
        => await _productRepository.DeleteAsync(id);

}