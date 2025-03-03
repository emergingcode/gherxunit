using GherXunit.Annotations;
using Xunit.Abstractions;

namespace BddTests.Samples.Features;

public partial class SubscriptionTest(ITestOutputHelper output): IGherXunit
{
    public ITestOutputHelper Output { get; } = output;
    private void WhenPattyLogsSteps() { }
    private async Task WhenFriedaLogsSteps() => await Task.CompletedTask;
}