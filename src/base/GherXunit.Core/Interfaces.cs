#nullable enable
using Xunit;
using Xunit.Abstractions;

namespace GherXunit.Annotations;

// Obsolete attributes
[Obsolete($"Use {nameof(IGherXunit<T>)} instead")]
public interface IGherXunitBackground<T> : IGherXunitStep, IClassFixture<T> where T : class;

// Interfaces
public interface IGherXunitStep;
public interface IGherXunit : IGherXunitStep, IGherXunitOutputProvider;
public interface IGherXunit<T> : IGherXunit, IClassFixture<T> where T : class;

// Output provider
public interface IGherXunitOutputProvider
{
    public ITestOutputHelper Output { get; }
}