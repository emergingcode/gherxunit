#nullable enable
global using Examples = Xunit.InlineDataAttribute;
using System.ComponentModel;
using Xunit;

namespace GherXunit.Annotations;

// Description attributes
public sealed class FeatureAttribute(string description) : DescriptionAttribute($"Feature: {description}");
public sealed class RuleAttribute(string description) : DescriptionAttribute($"Rule: {description}");
public sealed class BackgroundAttribute() : DescriptionAttribute("Background");

// Xunit attributes
public sealed class ScenarioAttribute(string displayName) : GherXunitFactAttribute(displayName);
public sealed class ExampleAttribute(string displayName) : GherXunitFactAttribute(displayName);
public sealed class ScenarioOutlineAttribute(string displayName) : GherXunitTheoryAttribute(displayName);
public sealed class ScenariosAttribute(string displayName) : GherXunitTheoryAttribute(displayName);

// Custom Xunit attributes
public class GherXunitFactAttribute(string displayName) : FactAttribute
{
    public override string DisplayName { get; set; } = displayName;
}

public class GherXunitTheoryAttribute(string displayName) : TheoryAttribute
{
    public override string DisplayName { get; set; } = displayName;
}
