using GherXunit.Annotations;
using Xunit.Abstractions;

namespace BddTests.Samples.Rules;

public partial class RuleTest(ITestOutputHelper output) : IGherXunit
{
    public ITestOutputHelper Output { get; } = output;
}