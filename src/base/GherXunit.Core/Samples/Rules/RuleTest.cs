using GherXunit.Annotations;

namespace BddTests.Samples.Rules;

[Feature("Highlander")]
public partial class RuleTest
{
    [Rule("There can be only One")]
    [Example("Only One -- One alive")]
    async Task Example01() => await this.NonExecutableAsync(
        steps: """
               Given there are <<3>> ninjas
               And there are more than one ninja alive
               When 2 ninjas meet, they will fight
               Then one ninja dies (but not me)
               And there is one ninja less alive
               """);

    [Example("There can be only One")]
    void Example02() => this.NonExecutable(
        steps: """
               Given there is only 1 ninja alive
               Then they will live forever ;-)
               """);
}

public partial class RuleTest
{
    [Rule("There can be Two (in some cases)")]
    [Example("Two -- Dead and Reborn as Phoenix")]
    async Task Example03() => await this.NonExecutableAsync(
        steps: """
               Given there are <<3>> ninjas
               And there are more than one ninja alive
               When 2 ninjas meet, they will fight
               Then one ninja dies (but not me)
               And there is one ninja less alive
               """);

    [Example("There can be only One")]
    void Example04() => this.NonExecutable(
        steps: """
               Given there is only 1 ninja alive
               Then they will live forever ;-)
               """);
}