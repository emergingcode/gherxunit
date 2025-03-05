#nullable enable
global using Examples = Xunit.InlineDataAttribute;
using System.ComponentModel;
using Xunit;

namespace GherXunit.Annotations;

public class FeatureAttribute(string description) : DescriptionAttribute($"Feature: {description}");

public class RuleAttribute(string description) : DescriptionAttribute($"Rule: {description}");

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

public sealed class ScenariosAttribute : TheoryAttribute
{
    public ScenariosAttribute(string displayName) => DisplayName = displayName;
}