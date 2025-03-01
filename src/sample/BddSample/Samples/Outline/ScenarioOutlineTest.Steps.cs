using GherXunit.Annotations;
using Xunit.Abstractions;

namespace BddTests.Samples.Outline;

public partial class ScenarioOutlineTest(ITestOutputHelper output) : IGherXunit
{
    public ITestOutputHelper Output { get; } = output;
    
    private async Task Step01(int start, int eat, int left) => await Task.CompletedTask;
}