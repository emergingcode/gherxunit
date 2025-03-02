namespace GherXunit.Sources;

/// <summary>
/// The source code of the interfaces that are used to annotate the Gherkin features.
/// It's
/// </summary>
internal struct InterfaceSourceCode
{
    public const string SOURCE =
        """
        #nullable enable
        using Xunit.Abstractions;

        namespace GherXunit.Annotations;

        internal interface IGherXunitBackground<T>:IGherXunit, IClassFixture<T> where T : class
        {
            void Setup();
        }

        internal interface IGherXunit
        {
            public ITestOutputHelper Output { get; }
        }
        """;
}