using GherXunit.Annotations;
using Xunit.Abstractions;

namespace BddTests.Samples.Localization;

public partial class LocalizationTest(ITestOutputHelper output): IGherXunit
{
    public ITestOutputHelper Output { get; } = output;
    private void WhenPattyLogsSteps() => Assert.True(true);
    private async Task WhenFriedaLogsSteps() => await Task.CompletedTask;
}