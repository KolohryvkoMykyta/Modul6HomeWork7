using MVC.ViewModels;

namespace MVC.Models.Requests
{
    public class AddOrderRequest
    {
        public List<OrderProduct> Data { get; set; } = null!;
    }
}
