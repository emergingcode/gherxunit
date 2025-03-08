# Refactoring tests and extracting more readable scenarios
It's easy to mess up your test code when you're trying to write test scenarios. I know how it feels to have a test code 
that is hard to read and maintain.**GherXunit** is a library that helps you write test scenarios in a structure familiar 
to those already using xUnit.

In this section, you will learn how to define test scenarios and implement step methods using **GherXunit**.
For that, we will show you how to define test scenarios using Gherkin syntax and implement step methods using **GherXunit**.

## Example of Scenario Definition
The following test class `DataProcessingServiceTests` represents a common scenario where we have a complex test that needs to be more readable and maintainable.
In this case, the behavior is hidden into the test methods, becoming hard to understand and maintain.
In fact, the test methods are mixed with the setup of the mocks and the configuration of the test environment.

```csharp
The test class contains the method: `ProcessData_ShouldProcessSuccessfully`. Also, the test class contains the setup of 
the mocks and the configuration of the test environment. The following code snippet shows the test class `DataProcessingServiceTests`:

```csharp
public class DataProcessingServiceTests
{
    private readonly Mock<IApiClient> _apiClientMock;
    private readonly Mock<IRepository> _repositoryMock;
    private readonly Mock<IQueueService> _queueServiceMock;
    private readonly Mock<ILogger<DataProcessingService>> _loggerMock;
    private readonly DataProcessingService _dataProcessingService;

    public DataProcessingServiceTests()
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
    }

    [Fact]
    public async Task ProcessData_ShouldProcessSuccessfully()
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
    
    ...
}
```
We will refactor this test class using **GherXunit** to separate the test methods from the Feature definition.
The first step is to define the Feature and Scenarios using the Gherkin syntax.

### Removing Test Methods to Separate Classes
For that, we will use `partial` classes to separate the test methods from the Feature definition.
The `DataProcessingServiceTests` class will contain the test methods, and the `DataProcessingServiceTests.Features` class will contain the Feature definition.
In the `DataProcessingServiceTests` we will use the `IGherXunit` interface. 
It's a marker interface that allows the `DataProcessingServiceTests` class to be recognized as a GherXunit test class.

> [!IMPORTANT]  
> The Output property is used to write messages to the test output.

```csharp
// DataProcessingServiceTests.cs
public partial class DataProcessingServiceTests: IGherXunit
{
    public ITestOutputHelper Output { get; }
    ...

    public DataProcessingServiceTests(..., ITestOutputHelper output)
    {
        ...
        Output = output;
    }
}

// DataProcessingServiceTests.Features.cs
public partial class DataProcessingServiceTests
{
}
```

### Writing the Feature Definition
Now, we will define the Feature and Scenarios using the Gherkin syntax. For that, we will use the `Feature` and `Scenario` attributes.

```csharp
// DataProcessingServiceTests.Features.cs
[Feature("Data Processing")]
public partial class DataProcessingServiceTests : IAsyncLifetime
{
    [Background]
    public async Task InitializeAsync() => await this.ExecuteAscync(
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

// DataProcessingServiceTests.cs
public partial class DataProcessingServiceTests : IGherXunit
{
    ...
        
    private Task Setup()
    {
        //TODO: Instantiate the mocks and configure the setup here.
        return Task.CompletedTask;
    }

    private async Task ProcessData_ShouldProcessSuccessfully()
    {
        ...
    }
    
    ...
}
```
In the file `DataProcessingServiceTests.Features.cs`, we define the Feature and Scenarios using the Gherkin syntax. Please note
the method `InitializeAsync` is used to set up the test environment before executing the scenarios. this method is decorated with the `Background` attribute and 
calls the `Setup` method to configure the mocks. 

The `ProcessData_ShouldProcessSuccessfully_Scenario` method represents the test scenario and calls the `ProcessData_ShouldProcessSuccessfully` 
method to execute the steps. Notice that the `ProcessData_ShouldProcessSuccessfully` does not have the Fact attribute anymore. 
It is now a private method that represents the steps of the test scenario. The result of running the test scenarios defined 
in the `DataProcessingServiceTests` class would be similar to the following output:

```gherkindotnet
.
TEST RESULT: 🟢 SUCCESS
⤷ FEATURE Data Processing
   ⤷ BACKGROUND Successful data processing
      | GIVEN ↘ the processing service is configured
.
TEST RESULT: 🟢 SUCCESS
⤷ FEATURE Data Processing
   ⤷ SCENARIO Successful data processing
      | GIVEN ↘ the external API returns valid data
      |   AND ↘ the repository saves the data successfully
      |   AND ↘ the queue service publishes the message successfully
      |  WHEN ↘ the processing service is executed
      |  THEN ↘ the processing is completed successfully
      |   AND ↘ a confirmation message is logged
```
The steps shown before are the simplified version of the test class `DataProcessingServiceTests` using **GherXunit**.
It's the first step to make your test code more readable and maintainable. We could go further and refactor the test methods
to be more descriptive and easier to understand, like removing repetitive code and separating concerns.

We hope this guide helps you get started with **GherXunit**. If you have any questions or need further assistance, please let us know.
