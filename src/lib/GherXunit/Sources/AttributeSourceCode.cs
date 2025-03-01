namespace GherXunit.Sources;

public struct AttributeSourceCode
{
    public const string SOURCE =
        """
        #nullable enable
        global using Examples = Xunit.InlineDataAttribute;
        using System;
        using System.ComponentModel;
        using Xunit;

        namespace GherXunit.Annotations;

        public class FeatureAttribute(string description) : DescriptionAttribute(description);
        public class RuleAttribute(string displayName) : DescriptionAttribute(displayName);
        public class BackgroundAttribute() : DescriptionAttribute("Background");

        public sealed class ScenarioAttribute : FactAttribute
        {
            public ScenarioAttribute(string displayName) => DisplayName = displayName;
        }

        public sealed class ExampleAttribute : FactAttribute
        {
            public ExampleAttribute(string displayName) => DisplayName = displayName;
        }

        public sealed class ScenarioOutlineAttribute : TheoryAttribute
        {
            public ScenarioOutlineAttribute(string displayName) => DisplayName = displayName;
        }    
        """;
}