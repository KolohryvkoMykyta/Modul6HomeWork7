namespace Basket.Host.Models;

public class GetResponse
{
    public List<BasketItem> Data { get; set; } = new List<BasketItem>();
}