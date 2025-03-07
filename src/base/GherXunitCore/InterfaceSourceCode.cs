#nullable enable
using Xunit;
using Xunit.Abstractions;

namespace GherXunit.Annotations;

[Obsolete($"Use {nameof(IGherXunit<T>)} instead")]
public interface IGherXunitBackground<T> : IGherXunitStep, IClassFixture<T> where T : class;

public interface IGherXunit : IGherXunitStep, IGherXunitOutputProvider;

public interface IGherXunit<T> : IGherXunit, IClassFixture<T> where T : class;

public interface IGherXunitStep;


public interface IGherXunitOutputProvider
{
    public ITestOutputHelper Output { get; }
}