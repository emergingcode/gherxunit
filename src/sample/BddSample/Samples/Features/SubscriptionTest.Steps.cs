using GherXunit.Annotations;
using Xunit.Abstractions;

namespace BddTests.Samples.Features;

public partial class SubscriptionTest(ITestOutputHelper output): IGherXunit
{
    public ITestOutputHelper Output { get; } = output;

    private async Task WhenFriedaLogsSteps() => await Task.CompletedTask;
    private void WhenPattyLogsSteps() { }
}