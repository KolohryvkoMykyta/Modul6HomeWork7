using System.Threading;
using Catalog.Host.Data.Entities;
using Catalog.Host.Models.Dtos;
using Infrastructure.Exceptions;

namespace Catalog.UnitTests.Services
{
    public class CatalogBrandServiceTest
    {
        private readonly ICatalogBrandService _catalogBrandService;

        private readonly Mock<ICatalogBrandRepository> _catalogBrandRepository;
        private readonly Mock<IDbContextWrapper<ApplicationDbContext>> _dbContextWrapper;
        private readonly Mock<ILogger<CatalogBrandService>> _logger;
        private readonly Mock<IMapper> _mapper;

        private readonly CatalogBrand _testBrand = new CatalogBrand()
        {
            Id = 1,
            Brand = "Name"
        };
        private readonly CatalogBrandDto _testBrandDto = new CatalogBrandDto()
        {
            Id = 1,
            Brand = "Name"
        };

        public CatalogBrandServiceTest()
        {
            _catalogBrandRepository = new Mock<ICatalogBrandRepository>();
            _dbContextWrapper = new Mock<IDbContextWrapper<ApplicationDbContext>>();
            _logger = new Mock<ILogger<CatalogBrandService>>();
            _mapper = new Mock<IMapper>();

            var dbContextTransaction = new Mock<IDbContextTransaction>();
            _dbContextWrapper.Setup(s => s.BeginTransactionAsync(CancellationToken.None)).ReturnsAsync(dbContextTransaction.Object);

            _catalogBrandService = new CatalogBrandService(_dbContextWrapper.Object, _logger.Object, _catalogBrandRepository.Object, _mapper.Object);
        }

        [Fact]
        public async Task AddAsync_Success()
        {
            // arrange
            var testResult = 1;

            _catalogBrandRepository.Setup(s => s.AddAsync(
                It.IsAny<string>())).ReturnsAsync(testResult);

            // act
            var result = await _catalogBrandService.AddAsync(_testBrand.Brand);

            // assert
            result.Should().Be(testResult);
        }

        [Fact]
        public async Task AddAsync_Failed()
        {
            // arrange
            int? testResult = null;

            _catalogBrandRepository.Setup(s => s.AddAsync(
                It.IsAny<string>())).ReturnsAsync(testResult);

            // act
            var result = await _catalogBrandService.AddAsync(_testBrand.Brand);

            // assert
            result.Should().Be(testResult);
        }

        [Fact]
        public async Task AddRangeAsync_Success()
        {
            // arrange
            var testResult = new List<int> { 1 };

            _catalogBrandRepository.Setup(s => s.AddRangeAsync(
                It.IsAny<IEnumerable<string>>())).ReturnsAsync(testResult);

            // act
            var result = await _catalogBrandService.AddRangeAsync(new List<string> { _testBrand.Brand });

            // assert
            result.Should().BeEquivalentTo(testResult);
        }

        [Fact]
        public async Task AddRangeAsync_Failed()
        {
            // arrange
            var testResult = new List<int>();

            _catalogBrandRepository.Setup(s => s.AddRangeAsync(
                It.IsAny<IEnumerable<string>>())).ReturnsAsync(testResult);

            // act
            var result = await _catalogBrandService.AddRangeAsync(new List<string> { _testBrand.Brand });

            // assert
            result.Should().BeEquivalentTo(testResult);
        }

        [Fact]
        public async Task DeleteAsync_Success()
        {
            // arrange
            var testResult = true;
            var testId = 1;

            _catalogBrandRepository.Setup(s => s.DeleteAsync(
                It.Is<int>(i => i == testId))).ReturnsAsync(testResult);

            // act
            var result = await _catalogBrandService.DeleteAsync(testId);

            // assert
            result.Should().Be(testResult);
        }

        [Fact]
        public async Task DeleteAsync_Failed()
        {
            // arrange
            var testId = 1000;

            _catalogBrandRepository.Setup(s => s.DeleteAsync(
                It.Is<int>(i => i == testId))).Throws(new BusinessException("Тестовое исключение"));

            // act
            Func<Task<bool>> testAction = async () => await _catalogBrandService.DeleteAsync(testId);

            // assert
            await testAction.Should().ThrowAsync<BusinessException>();
        }

        [Fact]
        public async Task UppdateAsync_Success()
        {
            // arrange
            var testResult = true;
            var testId = 1;

            _catalogBrandRepository.Setup(s => s.UpdateAsync(
                It.Is<int>(i => i == testId),
                It.IsAny<string>())).ReturnsAsync(testResult);

            // act
            var result = await _catalogBrandService.UpdateAsync(testId, _testBrand.Brand);

            // assert
            result.Should().Be(testResult);
        }

        [Fact]
        public async Task UppdateAsync_Failed()
        {
            // arrange
            var testId = 1000;

            _catalogBrandRepository.Setup(s => s.UpdateAsync(
                It.Is<int>(i => i == testId),
                It.IsAny<string>())).Throws(new BusinessException("Тестовое исключение"));

            // act
            Func<Task<bool>> testAction = async () => await _catalogBrandService.UpdateAsync(testId, _testBrand.Brand);

            // assert
            await testAction.Should().ThrowAsync<BusinessException>();
        }

        [Fact]
        public async Task GetByIdAsync_Success()
        {
            // arrange
            var testId = 1;
            var testBrand = _testBrand;
            var testResult = _testBrandDto;

            _catalogBrandRepository.Setup(s => s.GetByIdAsync(
                It.Is<int>(i => i == testId))).ReturnsAsync(testBrand);
            _mapper.Setup(s => s.Map<CatalogBrandDto>(
            It.Is<CatalogBrand>(i => i.Equals(testBrand)))).Returns(_testBrandDto);

            // act
            var result = await _catalogBrandService.GetByIdAsync(testId);

            // assert
            result.Should().Be(testResult);
        }

        [Fact]
        public async Task GetByIdAsync_Failed()
        {
            // arrange
            var testId = 1000;

            _catalogBrandRepository.Setup(s => s.GetByIdAsync(
                It.Is<int>(i => i == testId))).Throws(new BusinessException("Тестовое исключение"));

            // act
            Func<Task<CatalogBrandDto>> testAction = async () => await _catalogBrandService.GetByIdAsync(testId);

            // assert
            await testAction.Should().ThrowAsync<BusinessException>();
        }

        [Fact]
        public async Task GetAllBrandsAsync_Success()
        {
            // arrange
            var testResult = new List<CatalogBrandDto>() { _testBrandDto };
            var testListBrands = new List<CatalogBrand>() { _testBrand };

            _catalogBrandRepository.Setup(s => s.GetAllBrands()).ReturnsAsync(testListBrands);
            _mapper.Setup(s => s.Map<CatalogBrandDto>(
            It.Is<CatalogBrand>(i => i.Equals(_testBrand)))).Returns(_testBrandDto);

            // act
            var result = await _catalogBrandService.GetAllBrandsAsync();

            // assert
            result.Should().BeEquivalentTo(testListBrands);
        }

        [Fact]
        public async Task GetAllBrandsAsync_Failed()
        {
            // arrange
            var testResult = new List<CatalogBrandDto>();
            var testListBrands = new List<CatalogBrand>();

            _catalogBrandRepository.Setup(s => s.GetAllBrands()).ReturnsAsync(testListBrands);
            _mapper.Setup(s => s.Map<CatalogBrandDto>(
            It.Is<CatalogBrand>(i => i.Equals(_testBrand)))).Returns(_testBrandDto);

            // act
            var result = await _catalogBrandService.GetAllBrandsAsync();

            // assert
            result.Should().BeEquivalentTo(testListBrands);
        }
    }
}
