using Order.Host.Data.Entities;

namespace Order.Host.Models.Dtos
{
    public class OrderDto
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public int UserId { get; set; }
        public List<ProductDto> Products { get; set; } = null!;
    }
}
