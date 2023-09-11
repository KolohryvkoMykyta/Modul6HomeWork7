using Order.Host.Data;
using Order.Host.Data.Entities;
using Order.Host.Repositories.Interfaces;

namespace Order.Host.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<OrderRepository> _logger;

        public OrderRepository(
            IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
            ILogger<OrderRepository> logger)
        {
            _dbContext = dbContextWrapper.DbContext;
            _logger = logger;
        }

        public async Task<int> AddOrderAsync(string userId, List<ProductEntity> products)
        {
            var order = new OrderEntity() { UserId = userId, Products = products };
            var item = await _dbContext.AddAsync(order);

            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Added new order");

            return item.Entity.Id;
        }

        public async Task<List<OrderEntity>> GetOrdersAsync(string userId)
        {
            var orders = await _dbContext.Orders.Where(i => i.UserId == userId).Include(o => o.Products).OrderByDescending(o => o.CreatedDate).ToListAsync();

            if (orders == null)
            {
                return new List<OrderEntity>();
            }

            return orders;
        }
    }
}
