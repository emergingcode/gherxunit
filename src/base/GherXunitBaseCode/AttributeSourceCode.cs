#nullable enable
global using Examples = Xunit.InlineDataAttribute;
using System.ComponentModel;
using Xunit;

namespace GherXunit.Annotations;

internal class FeatureAttribute(string description) : DescriptionAttribute($"Feature: {description}");

internal class RuleAttribute(string description) : DescriptionAttribute($"Rule: {description}");

internal class BackgroundAttribute() : DescriptionAttribute("Background");

internal sealed class ScenarioAttribute : FactAttribute
{
    public ScenarioAttribute(string displayName) => DisplayName = displayName;
}

internal sealed class ExampleAttribute : FactAttribute
{
    public ExampleAttribute(string displayName) => DisplayName = displayName;
}

internal sealed class ScenarioOutlineAttribute : TheoryAttribute
{
    public ScenarioOutlineAttribute(string displayName) => DisplayName = displayName;
}

internal sealed class ScenariosAttribute : TheoryAttribute
{
    public ScenariosAttribute(string displayName) => DisplayName = displayName;
}