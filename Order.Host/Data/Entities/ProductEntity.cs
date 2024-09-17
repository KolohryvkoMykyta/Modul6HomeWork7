namespace Order.Host.Data.Entities
{
    public class ProductEntity
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public int OrderId { get; set; }
        public OrderEntity Order { get; set; }
    }
}
