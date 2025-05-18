namespace MonolitoBackend.Api.DTOs;

public class ProductCreateOrUpdateDTO
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int CategoryId { get; set; }
}
