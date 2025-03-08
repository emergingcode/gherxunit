using GherXunit.Annotations;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit.Abstractions;

namespace BddTests.Samples.Refactoring.ExemploA;

[Feature("Data Processing")]
public partial class DataProcessingServiceTests : IAsyncLifetime
{
    [Background]
    public async Task InitializeAsync() => await this.BackgroundAsync(
        refer: Setup,
        steps: "Given the processing service is configured");

    [Scenario("Successful data processing")]
    public async Task ProcessData_ShouldProcessSuccessfully_Scenario() => await this.ExecuteAscync(
        refer: ProcessData_ShouldProcessSuccessfully,
        steps: """
               Given the external API returns valid data
               And the repository saves the data successfully
               And the queue service publishes the message successfully
               When the processing service is executed
               Then the processing is completed successfully
               And a confirmation message is logged
               """);
}

public partial class DataProcessingServiceTests(ITestOutputHelper output) : IGherXunit
{
    private Mock<IApiClient> _apiClientMock;
    private Mock<IRepository> _repositoryMock;
    private Mock<IQueueService> _queueServiceMock;
    private Mock<ILogger<DataProcessingService>> _loggerMock;
    private DataProcessingService _dataProcessingService;

    public ITestOutputHelper? Output { get; } = output;

    private Task Setup()
    {
        _apiClientMock = new Mock<IApiClient>();
        _repositoryMock = new Mock<IRepository>();
        _queueServiceMock = new Mock<IQueueService>();
        _loggerMock = new Mock<ILogger<DataProcessingService>>();

        _dataProcessingService = new DataProcessingService(
            _apiClientMock.Object,
            _repositoryMock.Object,
            _queueServiceMock.Object,
            _loggerMock.Object
        );
        return Task.CompletedTask;
    }

    private async Task ProcessData_ShouldProcessSuccessfully()
    {
        // Arrange
        var data = new Data { Value = "Test" };

        _apiClientMock.Setup(api => api.FetchDataAsync())
            .ReturnsAsync(data);

        _repositoryMock.Setup(repo => repo.SaveDataAsync(data))
            .Returns(Task.CompletedTask);

        _queueServiceMock.Setup(queue => queue.PublishMessage(data.Value))
            .Returns(Task.CompletedTask);

        // Act
        await _dataProcessingService.ProcessData();

        // Assert
        _apiClientMock.Verify(api => api.FetchDataAsync(), Times.Once);
        _repositoryMock.Verify(repo => repo.SaveDataAsync(data), Times.Once);
        _queueServiceMock.Verify(queue => queue.PublishMessage(data.Value), Times.Once);

        _loggerMock.Verify(
            log => log.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Processing completed")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception, string>>()
            ), Times.Once);
    }

    [Fact]
    public async Task ProcessData_ShouldThrowExceptionWhenApiFails()
    {
        // Arrange
        _apiClientMock.Setup(api => api.FetchDataAsync())
            .ThrowsAsync(new HttpRequestException("API failure"));

        // Act & Assert
        await Assert.ThrowsAsync<HttpRequestException>(() => _dataProcessingService.ProcessData());

        _repositoryMock.Verify(repo => repo.SaveDataAsync(It.IsAny<Data>()), Times.Never);
        _queueServiceMock.Verify(queue => queue.PublishMessage(It.IsAny<string>()), Times.Never);

        _loggerMock.Verify(
            log => log.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error fetching data from API")),
                It.IsAny<HttpRequestException>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()
            ), Times.Once);
    }

    [Fact]
    public async Task ProcessData_ShouldThrowExceptionWhenDatabaseFails()
    {
        // Arrange
        var data = new Data { Value = "Test" };

        _apiClientMock.Setup(api => api.FetchDataAsync())
            .ReturnsAsync(data);

        _repositoryMock.Setup(repo => repo.SaveDataAsync(data))
            .ThrowsAsync(new Exception("Database failure"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _dataProcessingService.ProcessData());

        _queueServiceMock.Verify(queue => queue.PublishMessage(It.IsAny<string>()), Times.Never);

        _loggerMock.Verify(
            log => log.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error saving data to the database")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()
            ), Times.Once);
    }

    [Fact]
    public async Task ProcessData_ShouldThrowExceptionWhenQueueFails()
    {
        // Arrange
        var data = new Data { Value = "Test" };

        _apiClientMock.Setup(api => api.FetchDataAsync())
            .ReturnsAsync(data);

        _repositoryMock.Setup(repo => repo.SaveDataAsync(data))
            .Returns(Task.CompletedTask);

        _queueServiceMock.Setup(queue => queue.PublishMessage(data.Value))
            .ThrowsAsync(new Exception("Queue failure"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _dataProcessingService.ProcessData());

        _loggerMock.Verify(
            log => log.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error publishing message to the queue")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()
            ), Times.Once);
    }
    
    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}

public class Data
{
    public string Value { get; set; } = "";
}

public class DataProcessingService(
    IApiClient apiClient,
    IRepository repository,
    IQueueService queueService,
    ILogger<DataProcessingService> logger)
{
    public async Task ProcessData()
    {
        try
        {
            var data = await apiClient.FetchDataAsync();
            await repository.SaveDataAsync(data);
            await queueService.PublishMessage(data.Value);

            logger.LogInformation("Processing completed");
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, "Error fetching data from API");
            throw;
        }
        catch (Exception ex)
        {
            if (ex.Message == "Database failure")
                logger.LogError(ex, "Error saving data to the database");
            else
                logger.LogError(ex, "Error publishing message to the queue");
            throw;
        }
    }
}

public interface IRepository
{
    Task SaveDataAsync(Data data);
}

public interface IQueueService
{
    Task PublishMessage(string value);
}

public interface IApiClient
{
    Task<Data> FetchDataAsync();
}