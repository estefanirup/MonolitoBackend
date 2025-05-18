namespace MonolitoBackend.Core.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }

        // Chave esttrangeira
        public int CategoryId { get; set; }

        // Propriedade de navegação
        public Category Category { get; set; } = null!;
    }
}