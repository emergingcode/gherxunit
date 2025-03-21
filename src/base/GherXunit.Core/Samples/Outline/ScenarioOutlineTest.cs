using GherXunit.Annotations;

namespace BddTests.Samples.Outline;

[Feature("Outline")]
public partial class ScenarioOutlineTest
{
    [Scenarios("eating")]
    [Examples(12, 05, 07)]
    [Examples(20, 05, 15)]
    public async Task Scenario01(int start, int eat, int left) => await this.ExecuteAscync(
        refer: Step01, 
        param: [start, eat, left],
        steps: $"""
                Given there are {start} cucumbers
                When I eat {eat} cucumbers
                Then I should have {left} cucumbers
                """);
    
    [ScenarioOutline("eating")]
    [Examples(12, 05, 07)]
    [Examples(20, 05, 15)]
    public async Task Scenario02(int start, int eat, int left) => await this.ExecuteAscync(
        refer: Step01, 
        param: [start, eat, left],
        steps: $"""
                Given there are {start} cucumbers
                When I eat {eat} cucumbers
                Then I should have {left} cucumbers
                """);    
}