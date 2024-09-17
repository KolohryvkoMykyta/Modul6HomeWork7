using MVC.ViewModels;

namespace MVC.Services.Interfaces
{
    public interface IOrderService
    {
        Task<int> AddOrderItem(List<BasketItem> items);
        Task<List<Order>> GetOrders();
    }
}
