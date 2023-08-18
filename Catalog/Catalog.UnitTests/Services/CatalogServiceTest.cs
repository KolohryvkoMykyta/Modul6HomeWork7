using System.Threading;
using Catalog.Host.Data.Entities;
using Catalog.Host.Models.Dtos;
using Catalog.Host.Models.Response;
using Infrastructure.Exceptions;

namespace Catalog.UnitTests.Services;

public class CatalogServiceTest
{
    private readonly ICatalogService _catalogService;

    private readonly Mock<ICatalogItemRepository> _catalogItemRepository;
    private readonly Mock<IMapper> _mapper;
    private readonly Mock<IDbContextWrapper<ApplicationDbContext>> _dbContextWrapper;
    private readonly Mock<ILogger<CatalogService>> _logger;

    private readonly CatalogItem _testItem = new CatalogItem()
    {
        Id = 1,
        Name = "Name",
        Description = "Description",
        Price = 1000,
        AvailableStock = 100,
        CatalogBrandId = 1,
        CatalogBrand = new CatalogBrand() { Id = 1, Brand = "Test" },
        CatalogTypeId = 1,
        CatalogType = new CatalogType() { Id = 1, Type = "Test" },
        CatalogRadiusId = 1,
        CatalogRadius = new CatalogRadius() { Id = 1, Radius = "Test" },
        PictureFileName = "1.png"
    };

    private readonly CatalogItemDto _testItemDto = new CatalogItemDto()
    {
        Id = 1,
        Name = "Name",
        Description = "Description",
        Price = 1000,
        AvailableStock = 100,
        CatalogBrand = new CatalogBrandDto() { Id = 1, Brand = "Test" },
        CatalogType = new CatalogTypeDto() { Id = 1, Type = "Test" },
        CatalogRadius = new CatalogRadiusDto() { Id = 1, Radius = "Test" },
        PictureUrl = "1.png"
    };

    public CatalogServiceTest()
    {
        _catalogItemRepository = new Mock<ICatalogItemRepository>();
        _mapper = new Mock<IMapper>();
        _dbContextWrapper = new Mock<IDbContextWrapper<ApplicationDbContext>>();
        _logger = new Mock<ILogger<CatalogService>>();

        var dbContextTransaction = new Mock<IDbContextTransaction>();
        _dbContextWrapper.Setup(s => s.BeginTransactionAsync(CancellationToken.None)).ReturnsAsync(dbContextTransaction.Object);

        _catalogService = new CatalogService(_dbContextWrapper.Object, _logger.Object, _catalogItemRepository.Object, _mapper.Object);
    }

    [Fact]
    public async Task GetCatalogItemsAsync_Success()
    {
        // arrange
        var testPageIndex = 0;
        var testPageSize = 4;
        var testTotalCount = 12;

        var pagingPaginatedItemsSuccess = new PaginatedItems<CatalogItem>()
        {
            Data = new List<CatalogItem>()
            {
                new CatalogItem()
                {
                    Name = "TestName",
                },
            },
            TotalCount = testTotalCount,
        };

        var catalogItemSuccess = new CatalogItem()
        {
            Name = "TestName"
        };

        var catalogItemDtoSuccess = new CatalogItemDto()
        {
            Name = "TestName"
        };

        _catalogItemRepository.Setup(s => s.GetByPageAsync(
            It.Is<int>(i => i == testPageIndex),
            It.Is<int>(i => i == testPageSize),
            It.IsAny<int?>(),
            It.IsAny<int?>(),
            It.IsAny<int?>())).ReturnsAsync(pagingPaginatedItemsSuccess);

        _mapper.Setup(s => s.Map<CatalogItemDto>(
            It.Is<CatalogItem>(i => i.Equals(catalogItemSuccess)))).Returns(catalogItemDtoSuccess);

        // act
        var result = await _catalogService.GetCatalogItemsAsync(testPageSize, testPageIndex, null);

        // assert
        result.Should().NotBeNull();
        result?.Data.Should().NotBeNull();
        result?.Count.Should().Be(testTotalCount);
        result?.PageIndex.Should().Be(testPageIndex);
        result?.PageSize.Should().Be(testPageSize);
    }

    [Fact]
    public async Task GetCatalogItemsAsync_Failed()
    {
        // arrange
        var testPageIndex = 1000;
        var testPageSize = 10000;
        PaginatedItems<CatalogItem> item = null!;

        _catalogItemRepository.Setup(s => s.GetByPageAsync(
            It.Is<int>(i => i == testPageIndex),
            It.Is<int>(i => i == testPageSize),
            It.IsAny<int?>(),
            It.IsAny<int?>(),
            It.IsAny<int?>())).ReturnsAsync(item);

        // act
        var result = await _catalogService.GetCatalogItemsAsync(testPageSize, testPageIndex, null);

        // assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetItemByIdAsync_Success()
    {
        // arrange
        var testResult = _testItemDto;
        var testId = 1;

        _catalogItemRepository.Setup(s => s.GetItemByIdAsync(
            It.Is<int>(i => i == testId))).Returns(Task.FromResult(_testItem));

        _mapper.Setup(s => s.Map<CatalogItemDto>(
            It.Is<CatalogItem>(i => i.Equals(_testItem)))).Returns(_testItemDto);

        // act
        var result = await _catalogService.GetItemByIdAsync(testId);

        // assert
        result.Should().Be(_testItemDto);
    }

    [Fact]
    public async Task GetItemByIdAsync_Failed()
    {
        // arrange
        var testResult = _testItemDto;
        var testId = 1000;

        _catalogItemRepository.Setup(s => s.GetItemByIdAsync(
            It.Is<int>(i => i == testId))).Throws(new BusinessException("Тестовое исключение"));

        // act
        Func<Task<CatalogItemDto>> testAction = async () => await _catalogService.GetItemByIdAsync(testId);

        // assert
        await testAction.Should().ThrowAsync<BusinessException>();
    }
}