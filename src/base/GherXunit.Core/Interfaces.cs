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

//Lexer
public interface IGherXunitLexer
{
    public (string Key, string Value)[] Given { get; }
    public (string Key, string Value)[] When { get; }
    public (string Key, string Value)[] Then { get; }
    public (string Key, string Value)[] And { get; }
    public string Background { get; }
    public string Scenario { get; }
    public string Feature { get; }
}
