namespace GherXunit.Annotations;

/// <summary>
/// Source generator that creates the attributes and interfaces used to annotate the Gherkin features.
/// </summary>
[Generator]
public class GherXUnitSourceGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var referencesProvider = context.CompilationProvider.Select((compilation, _) => compilation.ReferencedAssemblyNames);
        
        context.RegisterSourceOutput(referencesProvider, (spc, references) =>
        {
            var hasXUnitReference = references.Any(reference => reference.Name.ToLower().Contains("xunit"));
            
            if (!hasXUnitReference) return;
            
            spc.AddSource("GherXunitStepStringHandler.g.cs", SourceText.From(GherXunitStepStringHandler.SOURCE, Encoding.UTF8));
            spc.AddSource("GherXUnitInterfaces.g.cs", SourceText.From(GherXunitInterfaces.SOURCE, Encoding.UTF8));
            spc.AddSource("GherXunitMethods.g.cs", SourceText.From(GherXunitMethods.SOURCE, Encoding.UTF8));
            spc.AddSource("GherXUnitAttributes.g.cs", SourceText.From(GherXunitAttributes.SOURCE, Encoding.UTF8));
        });
    }
}