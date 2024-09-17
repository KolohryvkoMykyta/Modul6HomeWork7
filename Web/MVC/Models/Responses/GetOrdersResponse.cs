using MVC.ViewModels;

namespace MVC.Models.Responses
{
    public class GetOrdersResponse
    {
        public List<Order> Data { get; set; } = null!;
    }
}
