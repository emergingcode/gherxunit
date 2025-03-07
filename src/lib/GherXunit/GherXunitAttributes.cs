namespace GherXunit.Annotations;

/// <summary>
/// The source code of the attributes that are used to annotate the Gherkin features.
/// This works as a superset of the Xunit attributes and it is used to create the custom attributes
/// that are used to annotate the Gherkin features.
/// </summary>
public struct GherXunitAttributes
{
    internal const string SOURCE =
        """
        #nullable enable
        global using Examples = Xunit.InlineDataAttribute;
        using System.ComponentModel;
        using Xunit;
        
        namespace GherXunit.Annotations;
        
        public sealed class FeatureAttribute(string description) : DescriptionAttribute($"Feature: {description}");
        public sealed class RuleAttribute(string description) : DescriptionAttribute($"Rule: {description}");
        public sealed class BackgroundAttribute() : DescriptionAttribute("Background");
        public sealed class ScenarioAttribute(string displayName) : GherXunitCustomFactAttribute(displayName);
        public sealed class ExampleAttribute(string displayName) : GherXunitCustomFactAttribute(displayName);
        public sealed class ScenarioOutlineAttribute(string displayName) : GherXunitCustomTheoryAttribute(displayName);
        public sealed class ScenariosAttribute(string displayName) : GherXunitCustomTheoryAttribute(displayName);
        
        public class GherXunitCustomFactAttribute(string displayName) : FactAttribute
        {
            public override string DisplayName { get; set; } = displayName;
        }
        
        public class GherXunitCustomTheoryAttribute(string displayName) : TheoryAttribute
        {
            public override string DisplayName { get; set; } = displayName;
        }
        """;
}