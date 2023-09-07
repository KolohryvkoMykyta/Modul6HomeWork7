namespace MVC.Models.Requests
{
    public class AddBasketItemRequest
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set;}
    }
}
