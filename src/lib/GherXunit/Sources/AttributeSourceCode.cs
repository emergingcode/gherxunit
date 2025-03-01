namespace GherXunit.Sources;

internal struct AttributeSourceCode
{
    public const string SOURCE =
        """
        #nullable enable
        global using Examples = Xunit.InlineDataAttribute;
        using System;
        using System.ComponentModel;
        using Xunit;

        namespace GherXunit.Annotations;

        internal class FeatureAttribute(string description) : DescriptionAttribute($"Feature: {description}");
        internal class RuleAttribute(string description) : DescriptionAttribute($"Rule: {description}");
        internal class BackgroundAttribute() : DescriptionAttribute("Background");

        internal sealed class GivenAttribute : FactAttribute
        {
            public GivenAttribute(string displayName) => DisplayName = $"GIVEN {displayName}";
        }
        
        internal sealed class WhenAttribute : FactAttribute
        {
            public WhenAttribute(string displayName) => DisplayName = $"WHEN {displayName}";
        }
        
        internal sealed class ThenAttribute : FactAttribute
        {
            public ThenAttribute(string displayName) => DisplayName = $"THEN {displayName}";
        }

        internal sealed class AndAttribute : FactAttribute
        {
            public AndAttribute(string displayName) => DisplayName = $"AND {displayName}";
        }
        
        internal sealed class ScenarioAttribute : FactAttribute
        {
            public ScenarioAttribute(string displayName) => DisplayName = $"Scenario: {displayName}";
        }

        internal sealed class ExampleAttribute : FactAttribute
        {
            public ExampleAttribute(string displayName) => DisplayName = $"Example: {displayName}";
        }

        internal sealed class ScenarioOutlineAttribute : TheoryAttribute
        {
            public ScenarioOutlineAttribute(string displayName) => DisplayName = $"Scenario Outline: {displayName}";
        }    
        """;
}