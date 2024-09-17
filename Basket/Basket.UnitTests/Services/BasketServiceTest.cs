using System.Collections.Generic;
using Basket.Host.Models;
using Basket.Host.Services;
using Basket.Host.Services.Interfaces;

namespace Basket.UnitTests.Services
{
    public class BasketServiceTest
    {
        private readonly BasketService _basketService;

        private readonly Mock<ILogger<BasketService>> _logger;
        private readonly Mock<ICacheService> _cacheService;

        public BasketServiceTest()
        {
            _logger = new Mock<ILogger<BasketService>>();
            _cacheService = new Mock<ICacheService>();

            _basketService = new BasketService(_cacheService.Object, _logger.Object);
        }

        [Fact]
        public async Task AddItemSucces()
        {
            // arrange
            var userId = "Test Id";
            var testItem = new BasketItem() { Id = 1, Name = "Test Name", Price = 100 };

            _cacheService.Setup(c => c.GetAsync<List<BasketItem>>(userId)).ReturnsAsync(new List<BasketItem>());

            // Act
            await _basketService.AddItem(userId, testItem);

            // Assert
            _cacheService.Verify(c => c.AddOrUpdateAsync(userId, It.IsAny<List<BasketItem>>()), Times.Once);
        }

        [Fact]
        public async Task AddItemFailed()
        {
            // arrange
            var userId = "Test Id";
            var testItem = new BasketItem() { Id = 1, Name = "Test Name", Price = 100 };

            _cacheService.Setup(c => c.GetAsync<List<BasketItem>>(userId)).ThrowsAsync(new Exception("Some error occurred."));

            // act
            Func<Task> testAction = async () => await _basketService.AddItem(userId, testItem);

            // assert
            await testAction.Should().ThrowAsync<Exception>();
        }

        [Fact]
        public async Task RemoveItemSucces()
        {
            // arrange
            var userId = "Test Id";
            var itemId = 1;
            var existingData = new List<BasketItem> { new BasketItem { Id = 1, Name = "Test Item", Price = 100 } };
            _cacheService.Setup(c => c.GetAsync<List<BasketItem>>(userId)).ReturnsAsync(existingData);

            // act
            await _basketService.RemoveItem(userId, itemId);

            // Assert
            _cacheService.Verify(c => c.AddOrUpdateAsync(userId, It.IsAny<List<BasketItem>>()), Times.Once);
        }

        [Fact]
        public async Task RemoveItemFailed()
        {
            // arrange
            var userId = "Test Id";
            var itemId = 2;
            var existingData = new List<BasketItem> { new BasketItem { Id = 1, Name = "Test Item", Price = 100 } };
            _cacheService.Setup(c => c.GetAsync<List<BasketItem>>(userId)).ReturnsAsync(existingData);

            // Act
            await _basketService.RemoveItem(userId, itemId);

            // Assert
            _cacheService.Verify(c => c.AddOrUpdateAsync(userId, It.IsAny<List<BasketItem>>()), Times.Never);
        }

        [Fact]
        public async Task GetItemsSuccess()
        {
            // arrange
            var userId = "Test Id";
            var existingData = new List<BasketItem> { new BasketItem { Id = 1, Name = "Test Item", Price = 100 } };
            _cacheService.Setup(c => c.GetAsync<List<BasketItem>>(userId)).ReturnsAsync(existingData);

            // Act
            var result = await _basketService.GetItems(userId);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().NotBeNull().And.HaveCount(1);
        }

        [Fact]
        public async Task GetItemsFailed()
        {
            // arrange
            var userId = "Test Id";
            _cacheService.Setup(c => c.GetAsync<List<BasketItem>>(userId)).ReturnsAsync((List<BasketItem>)null);

            // Act
            var result = await _basketService.GetItems(userId);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().NotBeNull().And.BeEmpty();
        }

        [Fact]
        public async Task ClearDataSuccess()
        {
            // arrange
            var userId = "Test Id";

            // Act
            await _basketService.ClearData(userId);

            // Assert
            _cacheService.Verify(c => c.ClearData(userId), Times.Once);
        }

        [Fact]
        public async Task ClearDataFailed()
        {
            // arrange
            var userId = "Test Id";
            _cacheService.Setup(c => c.ClearData(userId)).ThrowsAsync(new Exception("Some error occurred."));

            // act
            Func<Task> testAction = async () => await _basketService.ClearData(userId);

            // assert
            await testAction.Should().ThrowAsync<Exception>();
        }
    }
}
