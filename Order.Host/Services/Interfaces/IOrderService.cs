using Order.Host.Models.Dtos;

namespace Order.Host.Services.Interfaces
{
    public interface IOrderService
    {
        Task<int> AddOrderAsync(string userEmail, List<OrderProductDto> products);
        Task<List<OrderDto>> GetOrdersAsync(string userEmail);
    }
}
