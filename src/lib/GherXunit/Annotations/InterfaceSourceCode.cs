namespace GherXunit.Annotations;

/// <summary>
/// The source code of the interfaces that are used to annotate the Gherkin features.
/// It's
/// </summary>
public struct InterfaceSourceCode
{
    public const string SOURCE =
        """
        #nullable enable
        using Xunit.Abstractions;

        namespace GherXunit.Annotations;

        public interface IGherXunitBackground<T> : IGherXunit, IClassFixture<T> where T : class
        {
            void Setup();
        }

        public interface IGherXunit
        {
            public ITestOutputHelper Output { get; }
        }
        """;
}