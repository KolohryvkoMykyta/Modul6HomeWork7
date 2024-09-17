using Order.Host.Data.Entities;

namespace Order.Host.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        Task<int> AddOrderAsync(string userEmail, List<ProductEntity> products);

        Task<List<OrderEntity>> GetOrdersAsync(string userEmail);
    }
}
