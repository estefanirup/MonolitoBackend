using System.ComponentModel.DataAnnotations;
using static MonolitoBackend.Api.DTOs.CategoryDTO;

namespace MonolitoBackend.Api.DTOs;

public class ProductDTO
{
    [Required(ErrorMessage = "ATENÇÃO: O nome do produto é obrigatório!")]
    [StringLength(100, ErrorMessage = "ATENÇÃO: O nome deve ter no máximo 100 caracteres!")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "ATENÇÃO: O preço é obrigatório")]
    [Range(0.01, double.MaxValue, ErrorMessage = "ATENÇÃO: O preço deve ser maior que zero!")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "ATENÇÃO: A categoria é obrigatória!")]
    public int CategoryId { get; set; }

    public CategoryDTO Category { get; set; } = null!;
}
