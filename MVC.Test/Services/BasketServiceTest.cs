namespace MVC.Test.Services
{
    public class BasketServiceTest
    {
        private readonly IBasketService _basketService;

        private readonly Mock<IOptions<AppSettings>> _settings;
        private readonly Mock<IHttpClientService> _httpClient;
        private readonly Mock<ILogger<BasketService>> _logger;

        public BasketServiceTest()
        {
            _settings = new Mock<IOptions<AppSettings>>();
            _httpClient = new Mock<IHttpClientService>();
            _logger = new Mock<ILogger<BasketService>>();
            _settings.Setup(opt => opt.Value).Returns(new AppSettings { BasketUrl = "http://example.com/api/basket" });

            _basketService = new BasketService(_httpClient.Object, _logger.Object, _settings.Object);
        }

        [Fact]
        public async Task AddBasketItem_Success()
        {
            // Arrange
            var id = 1;
            var name = "TestItem";
            var price = 10.0m;

            // Act
            await _basketService.AddBasketItem(id, name, price);

            // Assert
            _httpClient.Verify(
                httpClient => httpClient.SendAsync<object, AddBasketItemRequest>(
                    "http://example.com/api/basket/addItem",
                    HttpMethod.Post,
                    It.Is<AddBasketItemRequest>(request =>
                        request.Id == id && request.Name == name && request.Price == price)),
                Times.Once);
        }

        [Fact]
        public async Task RemoveBasketItem_Success()
        {
            // Arrange
            var id = 1;

            // Act
            await _basketService.RemoveBasketItem(id);

            // Assert
            _httpClient.Verify(
                httpClient => httpClient.SendAsync<object, RemoveBasketItemRequest>(
                    "http://example.com/api/basket/removeItem",
                    HttpMethod.Post,
                    It.Is<RemoveBasketItemRequest>(request => request.Id == id)),
                Times.Once);
        }

        [Fact]
        public async Task ClearData_Success()
        {
            // Act
            await _basketService.ClearData();

            // Assert
            _httpClient.Verify(
                httpClient => httpClient.SendAsync<object, object>(
                    "http://example.com/api/basket/clearData",
                    HttpMethod.Post,
                    It.IsAny<object>()),
                Times.Once);
        }

        [Fact]
        public async Task GetBasketItems_Success()
        {
            // Arrange

            _httpClient.Setup(httpClient => httpClient.SendAsync<GetBasketResponse, string>(
                    "http://example.com/api/basket/getItems",
                    HttpMethod.Post,
                    default))
                .ReturnsAsync(new GetBasketResponse { Data = new List<BasketItem> { new BasketItem { Id = 1, Name = "Item1", Price = 10.0m } } });

            // Act
            var result = await _basketService.GetBasketItems();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(1, result[0].Id);
            Assert.Equal("Item1", result[0].Name);
            Assert.Equal(10.0m, result[0].Price);
        }

        [Fact]
        public async Task GetBasketItems_Failed()
        {
            // Arrange
            _httpClient.Setup(httpClient => httpClient.SendAsync<GetBasketResponse, string>(
                    "http://example.com/api/basket/getItems",
                    HttpMethod.Post,
                    default))
                .ReturnsAsync((GetBasketResponse)null);

            // Act
            var result = await _basketService.GetBasketItems();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}
