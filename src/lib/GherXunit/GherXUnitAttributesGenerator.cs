namespace GherXunit;

/// <summary>
/// Source generator that creates the attributes and interfaces used to annotate the Gherkin features.
/// </summary>
[Generator]
public class GherXUnitAttributesGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var referencesProvider = context.CompilationProvider.Select((compilation, _) => compilation.ReferencedAssemblyNames);
        
        context.RegisterSourceOutput(referencesProvider, (spc, references) =>
        {
            var hasXUnitReference = references.Any(reference => reference.Name.ToLower().Contains("xunit"));
            
            if (!hasXUnitReference) return;
            spc.AddSource("GherXUnitInterfaces.g.cs", SourceText.From(InterfaceSourceCode.SOURCE, Encoding.UTF8));
            spc.AddSource("GherXUnitSteps.g.cs", SourceText.From(StepSourceCode.SOURCE, Encoding.UTF8));
            spc.AddSource("GherXUnitAnnotations.g.cs", SourceText.From(AttributeSourceCode.SOURCE, Encoding.UTF8));
        });
    }
}