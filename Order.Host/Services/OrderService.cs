using Order.Host.Data;
using Order.Host.Data.Entities;
using Order.Host.Models.Dtos;
using Order.Host.Repositories.Interfaces;
using Order.Host.Services.Interfaces;

namespace Order.Host.Services
{
    public class OrderService : BaseDataService<ApplicationDbContext>, IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public OrderService(
            IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
            ILogger<BaseDataService<ApplicationDbContext>> logger,
            IOrderRepository orderRepository,
            IMapper mapper)
            : base(dbContextWrapper, logger)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<int> AddOrderAsync(string userId, List<OrderProductDto> products)
        {
            return await ExecuteSafeAsync(async () =>
            {
                var productEntities = products.Select(p => _mapper.Map<ProductEntity>(p)).ToList();

                return await _orderRepository.AddOrderAsync(userId, productEntities);
            });
        }

        public async Task<List<OrderDto>> GetOrdersAsync(string userId)
        {
            return await ExecuteSafeAsync(async () =>
            {
                var productEntities = await _orderRepository.GetOrdersAsync(userId);

                if (productEntities == null)
                {
                    return new List<OrderDto>();
                }

                var result = productEntities.Select(p => _mapper.Map<OrderDto>(p)).ToList();

                return result;
            });
        }
    }
}
