using GherXunit.Sources;

namespace GherXunit;

[Generator]
public class GherXUnitAttributesGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(ctx =>
        {
            ctx.AddSource("GherXUnitInterfaces.g.cs", SourceText.From(InterfaceSourceCode.SOURCE, Encoding.UTF8));
            ctx.AddSource("GherXUnitSteps.g.cs", SourceText.From(StepSourceCode.SOURCE, Encoding.UTF8));
            ctx.AddSource("GherXUnitAnnotations.g.cs", SourceText.From(AttributeSourceCode.SOURCE, Encoding.UTF8));
        });
    }
}