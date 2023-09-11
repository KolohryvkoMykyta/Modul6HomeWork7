using Moq;
using Order.Host.Data;
using Order.Host.Data.Entities;
using Order.Host.Models.Dtos;
using Order.Host.Repositories.Interfaces;
using Order.Host.Services;
using Order.Host.Services.Interfaces;

namespace Order.UnitTests.Services
{
    public class OrderServiceTest
    {
        private readonly IOrderService _orderService;

        private readonly Mock<IOrderRepository> _orderRepository;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IDbContextWrapper<ApplicationDbContext>> _dbContextWrapper;
        private readonly Mock<ILogger<OrderService>> _logger;

        public OrderServiceTest()
        {
            _orderRepository = new Mock<IOrderRepository>();
            _mapper = new Mock<IMapper>();
            _dbContextWrapper = new Mock<IDbContextWrapper<ApplicationDbContext>>();
            _logger = new Mock<ILogger<OrderService>>();

            var dbContextTransaction = new Mock<IDbContextTransaction>();
            _dbContextWrapper.Setup(s => s.BeginTransactionAsync(CancellationToken.None)).ReturnsAsync(dbContextTransaction.Object);

            _orderService = new OrderService(_dbContextWrapper.Object, _logger.Object, _orderRepository.Object, _mapper.Object);
        }

        private string testUserId = "Test Id";

        private List<OrderProductDto> testProducts = new List<OrderProductDto>
        {
            new OrderProductDto { Id = 0, ProductId = 1, Name = "Test Name 1", OrderId = 0, Price = 10 },
            new OrderProductDto { Id = 0, ProductId = 2, Name = "Test Name 2", OrderId = 0, Price = 20 }
        };
        private List<ProductEntity> testProductEntities = new List<ProductEntity>
        {
            new ProductEntity { Id = 0, ProductId = 1, Name = "Test Name 1", OrderId = 0, Price = 10 },
            new ProductEntity { Id = 0, ProductId = 2, Name = "Test Name 2", OrderId = 0, Price = 20 }
        };
        private List<OrderEntity> testOrders = new List<OrderEntity>()
        {
            new OrderEntity(),
            new OrderEntity()
        };

        [Fact]
        public async Task AddOrderAsync_Success()
        {
            // arrange            
            var testResult = 42;

            _mapper.Setup(s => s.Map<ProductEntity>(It.IsAny<OrderProductDto>())).Returns(new ProductEntity());
            _orderRepository.Setup(s => s.AddOrderAsync(testUserId, It.IsAny<List<ProductEntity>>())).ReturnsAsync(testResult);

            // act
            var result = await _orderService.AddOrderAsync(testUserId, testProducts);

            // assert
            result.Should().Be(testResult);
        }

        [Fact]
        public async Task AddOrderAsync_Failed()
        {
            // arrange
            _mapper.Setup(s => s.Map<ProductEntity>(It.IsAny<OrderProductDto>())).Returns(new ProductEntity());
            _orderRepository.Setup(s => s.AddOrderAsync(testUserId, It.IsAny<List<ProductEntity>>())).ThrowsAsync(new Exception());

            // act
            Func<Task<int>> testAction = async () => await _orderService.AddOrderAsync(testUserId, testProducts);

            // assert
            await testAction.Should().ThrowAsync<Exception>();
        }

        [Fact]
        public async Task GetOrdersAsync_Success()
        {
            // arrange
            _orderRepository.Setup(s => s.GetOrdersAsync(testUserId)).ReturnsAsync(testOrders);
            _mapper.Setup(s => s.Map<OrderDto>(It.IsAny<OrderEntity>())).Returns(new OrderDto());

            // act
            var result = await _orderService.GetOrdersAsync(testUserId);

            // assert
            result.Should().NotBeNull();
            result.Count.Should().Be(testProductEntities.Count);
        }

        [Fact]
        public async Task GetOrdersAsync_Failed()
        {
            // arrange
            _orderRepository.Setup(s => s.GetOrdersAsync(testUserId)).ThrowsAsync(new Exception());
            _mapper.Setup(s => s.Map<OrderDto>(It.IsAny<OrderEntity>())).Returns(new OrderDto());

            // act
            Func<Task<List<OrderDto>>> testAction = async () => await _orderService.GetOrdersAsync(testUserId);

            // assert
            await testAction.Should().ThrowAsync<Exception>();
        }
    }
}