using System.Threading;
using Catalog.Host.Data.Entities;
using Catalog.Host.Models.Dtos;
using Infrastructure.Exceptions;

namespace Catalog.UnitTests.Services
{
    public class CatalogRadiusServiceTest
    {
        private readonly ICatalogRadiusService _catalogRadiusService;

        private readonly Mock<ICatalogRadiusRepository> _catalogRadiusRepository;
        private readonly Mock<IDbContextWrapper<ApplicationDbContext>> _dbContextWrapper;
        private readonly Mock<ILogger<CatalogRadiusService>> _logger;
        private readonly Mock<IMapper> _mapper;

        private readonly CatalogRadius _testRadius = new CatalogRadius()
        {
            Id = 1,
            Radius = "Name"
        };
        private readonly CatalogRadiusDto _testRadiusDto = new CatalogRadiusDto()
        {
            Id = 1,
            Radius = "Name"
        };

        public CatalogRadiusServiceTest()
        {
            _catalogRadiusRepository = new Mock<ICatalogRadiusRepository>();
            _dbContextWrapper = new Mock<IDbContextWrapper<ApplicationDbContext>>();
            _logger = new Mock<ILogger<CatalogRadiusService>>();
            _mapper = new Mock<IMapper>();

            var dbContextTransaction = new Mock<IDbContextTransaction>();
            _dbContextWrapper.Setup(s => s.BeginTransactionAsync(CancellationToken.None)).ReturnsAsync(dbContextTransaction.Object);

            _catalogRadiusService = new CatalogRadiusService(_dbContextWrapper.Object, _logger.Object, _catalogRadiusRepository.Object, _mapper.Object);
        }

        [Fact]
        public async Task AddAsync_Success()
        {
            // arrange
            var testResult = 1;

            _catalogRadiusRepository.Setup(s => s.AddAsync(
                It.IsAny<string>())).ReturnsAsync(testResult);

            // act
            var result = await _catalogRadiusService.AddAsync(_testRadius.Radius);

            // assert
            result.Should().Be(testResult);
        }

        [Fact]
        public async Task AddAsync_Failed()
        {
            // arrange
            int? testResult = null;

            _catalogRadiusRepository.Setup(s => s.AddAsync(
                It.IsAny<string>())).ReturnsAsync(testResult);

            // act
            var result = await _catalogRadiusService.AddAsync(_testRadius.Radius);

            // assert
            result.Should().Be(testResult);
        }

        [Fact]
        public async Task AddRangeAsync_Success()
        {
            // arrange
            var testResult = new List<int> { 1 };

            _catalogRadiusRepository.Setup(s => s.AddRangeAsync(
                It.IsAny<IEnumerable<string>>())).ReturnsAsync(testResult);

            // act
            var result = await _catalogRadiusService.AddRangeAsync(new List<string> { _testRadius.Radius });

            // assert
            result.Should().BeEquivalentTo(testResult);
        }

        [Fact]
        public async Task AddRangeAsync_Failed()
        {
            // arrange
            var testResult = new List<int>();

            _catalogRadiusRepository.Setup(s => s.AddRangeAsync(
                It.IsAny<IEnumerable<string>>())).ReturnsAsync(testResult);

            // act
            var result = await _catalogRadiusService.AddRangeAsync(new List<string> { _testRadius.Radius });

            // assert
            result.Should().BeEquivalentTo(testResult);
        }

        [Fact]
        public async Task DeleteAsync_Success()
        {
            // arrange
            var testResult = true;
            var testId = 1;

            _catalogRadiusRepository.Setup(s => s.DeleteAsync(
                It.Is<int>(i => i == testId))).ReturnsAsync(testResult);

            // act
            var result = await _catalogRadiusService.DeleteAsync(testId);

            // assert
            result.Should().Be(testResult);
        }

        [Fact]
        public async Task DeleteAsync_Failed()
        {
            // arrange
            var testId = 1000;

            _catalogRadiusRepository.Setup(s => s.DeleteAsync(
                It.Is<int>(i => i == testId))).Throws(new BusinessException("Тестовое исключение"));

            // act
            Func<Task<bool>> testAction = async () => await _catalogRadiusService.DeleteAsync(testId);

            // assert
            await testAction.Should().ThrowAsync<BusinessException>();
        }

        [Fact]
        public async Task UppdateAsync_Success()
        {
            // arrange
            var testResult = true;
            var testId = 1;

            _catalogRadiusRepository.Setup(s => s.UpdateAsync(
                It.Is<int>(i => i == testId),
                It.IsAny<string>())).ReturnsAsync(testResult);

            // act
            var result = await _catalogRadiusService.UpdateAsync(testId, _testRadius.Radius);

            // assert
            result.Should().Be(testResult);
        }

        [Fact]
        public async Task UppdateAsync_Failed()
        {
            // arrange
            var testId = 1000;

            _catalogRadiusRepository.Setup(s => s.UpdateAsync(
                It.Is<int>(i => i == testId),
                It.IsAny<string>())).Throws(new BusinessException("Тестовое исключение"));

            // act
            Func<Task<bool>> testAction = async () => await _catalogRadiusService.UpdateAsync(testId, _testRadius.Radius);

            // assert
            await testAction.Should().ThrowAsync<BusinessException>();
        }

        [Fact]
        public async Task GetByIdAsync_Success()
        {
            // arrange
            var testId = 1;
            var testRadius = _testRadius;
            var testResult = _testRadiusDto;

            _catalogRadiusRepository.Setup(s => s.GetByIdAsync(
                It.Is<int>(i => i == testId))).ReturnsAsync(testRadius);
            _mapper.Setup(s => s.Map<CatalogRadiusDto>(
            It.Is<CatalogRadius>(i => i.Equals(testRadius)))).Returns(_testRadiusDto);

            // act
            var result = await _catalogRadiusService.GetByIdAsync(testId);

            // assert
            result.Should().Be(testResult);
        }

        [Fact]
        public async Task GetByIdAsync_Failed()
        {
            // arrange
            var testId = 1000;

            _catalogRadiusRepository.Setup(s => s.GetByIdAsync(
                It.Is<int>(i => i == testId))).Throws(new BusinessException("Тестовое исключение"));

            // act
            Func<Task<CatalogRadiusDto>> testAction = async () => await _catalogRadiusService.GetByIdAsync(testId);

            // assert
            await testAction.Should().ThrowAsync<BusinessException>();
        }

        [Fact]
        public async Task GetAllRadiusesAsync_Success()
        {
            // arrange
            var testResult = new List<CatalogRadiusDto>() { _testRadiusDto };
            var testListRadius = new List<CatalogRadius>() { _testRadius };

            _catalogRadiusRepository.Setup(s => s.GetAllRadiusesAsync()).ReturnsAsync(testListRadius);
            _mapper.Setup(s => s.Map<CatalogRadiusDto>(
            It.Is<CatalogRadius>(i => i.Equals(_testRadius)))).Returns(_testRadiusDto);

            // act
            var result = await _catalogRadiusService.GetAllRadiusesAsync();

            // assert
            result.Should().BeEquivalentTo(testListRadius);
        }

        [Fact]
        public async Task GetAllRadiusesAsync_Failed()
        {
            // arrange
            var testResult = new List<CatalogRadiusDto>();
            var testListRadius = new List<CatalogRadius>();

            _catalogRadiusRepository.Setup(s => s.GetAllRadiusesAsync()).ReturnsAsync(testListRadius);
            _mapper.Setup(s => s.Map<CatalogRadiusDto>(
            It.Is<CatalogRadius>(i => i.Equals(_testRadius)))).Returns(_testRadiusDto);

            // act
            var result = await _catalogRadiusService.GetAllRadiusesAsync();

            // assert
            result.Should().BeEquivalentTo(testListRadius);
        }
    }
}
