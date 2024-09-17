using FluentAssertions;
using System.Diagnostics;
using System.Net;

namespace MVC.Test.Services
{
    public class OrderServiceTest
    {
        private readonly IOrderService _orderService;

        private readonly Mock<IOptions<AppSettings>> _settings;
        private readonly Mock<IHttpClientService> _httpClient;
        private readonly Mock<ILogger<OrderService>> _logger;

        public OrderServiceTest()
        {
            _settings = new Mock<IOptions<AppSettings>>();
            _httpClient = new Mock<IHttpClientService>();
            _logger = new Mock<ILogger<OrderService>>();
            _settings.Setup(opt => opt.Value).Returns(new AppSettings { OrderUrl = "http://example.com/api/order" });

            _orderService = new OrderService(_httpClient.Object, _logger.Object, _settings.Object);
        }

        [Fact]
        public async Task GetOrders_Success()
        {
            // Arrange
            _httpClient.Setup(httpClient => httpClient.SendAsync<GetOrdersResponse, object>(
                    "http://example.com/api/order/getOrders",
                    HttpMethod.Post,
                    default))
                .ReturnsAsync(new GetOrdersResponse { Data = new List<Order> { new Order { Id = 1, UserId = 1 } } });

            // Act
            var result = await _orderService.GetOrders();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(1, result[0].Id);
            Assert.Equal(1, result[0].UserId);
        }

        [Fact]
        public async Task GetOrders_Failed()
        {
            // Arrange
            _httpClient.Setup(httpClient => httpClient.SendAsync<GetOrdersResponse, object>(
                    "http://example.com/api/order/getOrders",
                    HttpMethod.Post,
                    default))
                .ReturnsAsync((GetOrdersResponse)null);

            // Act
            var result = await _orderService.GetOrders();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task AddOrderItem_Success()
        {
            // Arrange
            var basketItems = new List<BasketItem>
            {
            new BasketItem { Id = 1, Name = "Product 1", Price = 100.0m },
            new BasketItem { Id = 2, Name = "Product 2", Price = 50.0m }
            };

            var orderItems = basketItems.Select(o => new OrderProduct
            {
                Id = 0,
                ProductId = o.Id,
                Name = o.Name,
                Price = o.Price,
                OrderId = 0
            }).ToList();

            var addOrderResponse = new AddOrderResponse { Id = 12345 };

            _httpClient.Setup(httpClient => httpClient.SendAsync<AddOrderResponse, AddOrderRequest>(
                    "http://example.com/api/order/addOrder",
                    HttpMethod.Post,
                    It.IsAny<AddOrderRequest>()))
                .ReturnsAsync(addOrderResponse);

            // Act
            var result = await _orderService.AddOrderItem(basketItems);

            // Assert
            result.Should().Be(12345);
        }

        [Fact]
        public async Task AddOrderItem_Failed()
        {
            // Arrange
            var basketItems = new List<BasketItem>
            {
            new BasketItem { Id = 1, Name = "Product 1", Price = 100.0m },
            new BasketItem { Id = 2, Name = "Product 2", Price = 50.0m }
            };

            var orderItems = basketItems.Select(o => new OrderProduct
            {
                Id = 0,
                ProductId = o.Id,
                Name = o.Name,
                Price = o.Price,
                OrderId = 0
            }).ToList();

            _httpClient.Setup(httpClient => httpClient.SendAsync<AddOrderResponse, AddOrderRequest>(
                    "http://example.com/api/order/addOrder",
                    HttpMethod.Post,
                    It.IsAny<AddOrderRequest>()))
                .ReturnsAsync((AddOrderResponse)null);

            // Act
            Func<Task<int>> testAction = async () => await _orderService.AddOrderItem(basketItems);

            // Assert
            await testAction.Should().ThrowAsync<Exception>();
        }
    }
}
