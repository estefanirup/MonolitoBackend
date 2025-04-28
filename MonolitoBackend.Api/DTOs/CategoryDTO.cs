using System.ComponentModel.DataAnnotations;

namespace MonolitoBackend.Api.DTOs;

public class CategoryDTO
{
    [Required(ErrorMessage = "ATENÇÃO: O nome da categoria é obrigatório!")]
    [StringLength(100, ErrorMessage = "ATENÇÃO: O nome deve ter no máximo 100 caracteres!")]
    public string Name { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "ATENÇÃO: A descrição deve ter no máximo 500 caracteres!")]
    public string? Description { get; set; }
}