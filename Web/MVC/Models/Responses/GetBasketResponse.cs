using MVC.ViewModels;

namespace MVC.Models.Responses
{
    public class GetBasketResponse
    {
        public IReadOnlyList<BasketItem> Data { get; set; }
    }
}
