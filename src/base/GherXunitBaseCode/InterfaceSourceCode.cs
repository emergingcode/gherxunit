#nullable enable
using Xunit.Abstractions;

namespace GherXunit.Annotations;

internal interface IGherXunitBackground<T> : IGherXunit, IClassFixture<T> where T : class
{
    void Setup();
}

internal interface IGherXunit
{
    public ITestOutputHelper Output { get; }
}