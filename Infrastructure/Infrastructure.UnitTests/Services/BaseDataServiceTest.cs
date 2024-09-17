using System.Threading;
using FluentAssertions;
using Infrastructure.Exceptions;
using Infrastructure.UnitTests.Mocks;

namespace Infrastructure.UnitTests.Services;

public class BaseDataServiceTest
{
    private readonly Mock<IDbContextTransaction> _dbContextTransaction;
    private readonly Mock<ILogger<MockService>> _logger;
    private readonly MockService _mockService;

    public BaseDataServiceTest()
    {
        var dbContextWrapper = new Mock<IDbContextWrapper<MockDbContext>>();
        _dbContextTransaction = new Mock<IDbContextTransaction>();
        _logger = new Mock<ILogger<MockService>>();

        dbContextWrapper.Setup(s => s.BeginTransactionAsync(It.IsAny<CancellationToken>())).ReturnsAsync(_dbContextTransaction.Object);

        _mockService = new MockService(dbContextWrapper.Object, _logger.Object);
    }

    [Fact]
    public async Task ExecuteSafe_Success()
    {
        // arrange

        // act
        await _mockService.RunWithoutException();

        // assert
        _dbContextTransaction.Verify(t => t.CommitAsync(CancellationToken.None), Times.Once);
        _dbContextTransaction.Verify(t => t.RollbackAsync(CancellationToken.None), Times.Never);
    }

    [Fact]
    public async Task ExecuteSafe_Failed()
    {
        // arrange

        // act
        Func<Task> testAction = async () => await _mockService.RunWithException();

        await testAction.Should().ThrowAsync<System.Exception>();

            // assert
        _dbContextTransaction.Verify(t => t.CommitAsync(CancellationToken.None), Times.Never);
        _dbContextTransaction.Verify(t => t.RollbackAsync(CancellationToken.None), Times.Once);

        _logger.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((object o, Type t) => o.ToString() !
                    .Contains($"transaction is rollbacked")),
                It.IsAny<System.Exception>(),
                It.IsAny<Func<It.IsAnyType, System.Exception, string>>() !),
            Times.Once);
    }
}