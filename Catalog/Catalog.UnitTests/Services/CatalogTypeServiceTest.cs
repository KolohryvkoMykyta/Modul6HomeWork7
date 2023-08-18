using System.Threading;
using Catalog.Host.Data.Entities;
using Catalog.Host.Models.Dtos;
using Infrastructure.Exceptions;

namespace Catalog.UnitTests.Services
{
    public class CatalogTypeServiceTest
    {
        private readonly ICatalogTypeService _catalogTypeService;

        private readonly Mock<ICatalogTypeRepository> _catalogTypeRepository;
        private readonly Mock<IDbContextWrapper<ApplicationDbContext>> _dbContextWrapper;
        private readonly Mock<ILogger<CatalogTypeService>> _logger;
        private readonly Mock<IMapper> _mapper;

        private readonly CatalogType _testType = new CatalogType()
        {
            Id = 1,
            Type = "Name"
        };
        private readonly CatalogTypeDto _testTypeDto = new CatalogTypeDto()
        {
            Id = 1,
            Type = "Name"
        };

        public CatalogTypeServiceTest()
        {
            _catalogTypeRepository = new Mock<ICatalogTypeRepository>();
            _dbContextWrapper = new Mock<IDbContextWrapper<ApplicationDbContext>>();
            _logger = new Mock<ILogger<CatalogTypeService>>();
            _mapper = new Mock<IMapper>();

            var dbContextTransaction = new Mock<IDbContextTransaction>();
            _dbContextWrapper.Setup(s => s.BeginTransactionAsync(CancellationToken.None)).ReturnsAsync(dbContextTransaction.Object);

            _catalogTypeService = new CatalogTypeService(_dbContextWrapper.Object, _logger.Object, _catalogTypeRepository.Object, _mapper.Object);
        }

        [Fact]
        public async Task AddAsync_Success()
        {
            // arrange
            var testResult = 1;

            _catalogTypeRepository.Setup(s => s.AddAsync(
                It.IsAny<string>())).ReturnsAsync(testResult);

            // act
            var result = await _catalogTypeService.AddAsync(_testType.Type);

            // assert
            result.Should().Be(testResult);
        }

        [Fact]
        public async Task AddAsync_Failed()
        {
            // arrange
            int? testResult = null;

            _catalogTypeRepository.Setup(s => s.AddAsync(
                It.IsAny<string>())).ReturnsAsync(testResult);

            // act
            var result = await _catalogTypeService.AddAsync(_testType.Type);

            // assert
            result.Should().Be(testResult);
        }

        [Fact]
        public async Task AddRangeAsync_Success()
        {
            // arrange
            var testResult = new List<int> { 1 };

            _catalogTypeRepository.Setup(s => s.AddRangeAsync(
                It.IsAny<IEnumerable<string>>())).ReturnsAsync(testResult);

            // act
            var result = await _catalogTypeService.AddRangeAsync(new List<string> { _testType.Type });

            // assert
            result.Should().BeEquivalentTo(testResult);
        }

        [Fact]
        public async Task AddRangeAsync_Failed()
        {
            // arrange
            var testResult = new List<int>();

            _catalogTypeRepository.Setup(s => s.AddRangeAsync(
                It.IsAny<IEnumerable<string>>())).ReturnsAsync(testResult);

            // act
            var result = await _catalogTypeService.AddRangeAsync(new List<string> { _testType.Type });

            // assert
            result.Should().BeEquivalentTo(testResult);
        }

        [Fact]
        public async Task DeleteAsync_Success()
        {
            // arrange
            var testResult = true;
            var testId = 1;

            _catalogTypeRepository.Setup(s => s.DeleteAsync(
                It.Is<int>(i => i == testId))).ReturnsAsync(testResult);

            // act
            var result = await _catalogTypeService.DeleteAsync(testId);

            // assert
            result.Should().Be(testResult);
        }

        [Fact]
        public async Task DeleteAsync_Failed()
        {
            // arrange
            var testId = 1000;

            _catalogTypeRepository.Setup(s => s.DeleteAsync(
                It.Is<int>(i => i == testId))).Throws(new BusinessException("Тестовое исключение"));

            // act
            Func<Task<bool>> testAction = async () => await _catalogTypeService.DeleteAsync(testId);

            // assert
            await testAction.Should().ThrowAsync<BusinessException>();
        }

        [Fact]
        public async Task UppdateAsync_Success()
        {
            // arrange
            var testResult = true;
            var testId = 1;

            _catalogTypeRepository.Setup(s => s.UpdateAsync(
                It.Is<int>(i => i == testId),
                It.IsAny<string>())).ReturnsAsync(testResult);

            // act
            var result = await _catalogTypeService.UpdateAsync(testId, _testType.Type);

            // assert
            result.Should().Be(testResult);
        }

        [Fact]
        public async Task UppdateAsync_Failed()
        {
            // arrange
            var testId = 1000;

            _catalogTypeRepository.Setup(s => s.UpdateAsync(
                It.Is<int>(i => i == testId),
                It.IsAny<string>())).Throws(new BusinessException("Тестовое исключение"));

            // act
            Func<Task<bool>> testAction = async () => await _catalogTypeService.UpdateAsync(testId, _testType.Type);

            // assert
            await testAction.Should().ThrowAsync<BusinessException>();
        }

        [Fact]
        public async Task GetByIdAsync_Success()
        {
            // arrange
            var testId = 1;
            var testType = _testType;
            var testResult = _testTypeDto;

            _catalogTypeRepository.Setup(s => s.GetByIdAsync(
                It.Is<int>(i => i == testId))).ReturnsAsync(testType);
            _mapper.Setup(s => s.Map<CatalogTypeDto>(
            It.Is<CatalogType>(i => i.Equals(testType)))).Returns(_testTypeDto);

            // act
            var result = await _catalogTypeService.GetByIdAsync(testId);

            // assert
            result.Should().Be(testResult);
        }

        [Fact]
        public async Task GetByIdAsync_Failed()
        {
            // arrange
            var testId = 1000;

            _catalogTypeRepository.Setup(s => s.GetByIdAsync(
                It.Is<int>(i => i == testId))).Throws(new BusinessException("Тестовое исключение"));

            // act
            Func<Task<CatalogTypeDto>> testAction = async () => await _catalogTypeService.GetByIdAsync(testId);

            // assert
            await testAction.Should().ThrowAsync<BusinessException>();
        }

        [Fact]
        public async Task GetAllTypesAsync_Success()
        {
            // arrange
            var testResult = new List<CatalogTypeDto>() { _testTypeDto };
            var testListTypes = new List<CatalogType>() { _testType };

            _catalogTypeRepository.Setup(s => s.GetAllTypeAsync()).ReturnsAsync(testListTypes);
            _mapper.Setup(s => s.Map<CatalogTypeDto>(
            It.Is<CatalogType>(i => i.Equals(_testType)))).Returns(_testTypeDto);

            // act
            var result = await _catalogTypeService.GetAllTypeAsync();

            // assert
            result.Should().BeEquivalentTo(testListTypes);
        }

        [Fact]
        public async Task GetAllTypesAsync_Failed()
        {
            // arrange
            var testResult = new List<CatalogTypeDto>();
            var testListTypes = new List<CatalogType>();

            _catalogTypeRepository.Setup(s => s.GetAllTypeAsync()).ReturnsAsync(testListTypes);
            _mapper.Setup(s => s.Map<CatalogTypeDto>(
            It.Is<CatalogType>(i => i.Equals(_testType)))).Returns(_testTypeDto);

            // act
            var result = await _catalogTypeService.GetAllTypeAsync();

            // assert
            result.Should().BeEquivalentTo(testListTypes);
        }
    }
}
