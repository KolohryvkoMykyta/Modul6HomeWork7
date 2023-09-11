namespace MVC.ViewModels
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public int UserId { get; set; }
        public List<OrderProduct> Products { get; set; } = null!;
    }
}
