namespace Order.Host.Data.Entities
{
    public class OrderEntity
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public int UserId { get; set; }
        public List<ProductEntity> Products { get; set; } = null!;

    }
}
