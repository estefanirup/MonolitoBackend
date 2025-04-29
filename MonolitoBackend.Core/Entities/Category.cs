namespace MonolitoBackend.Core.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }

        // Relacionamento um para muitos
        public ICollection<Product> Products { get; set; } = new List<Product>(); // verificar category DTO!!!!!
    }
}