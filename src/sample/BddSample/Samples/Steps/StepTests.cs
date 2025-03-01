using GherXunit.Annotations;
using Xunit.Abstractions;

namespace BddTests.Samples.Steps;


public partial class StepTests(ITestOutputHelper output): IGherXunit
{
    public ITestOutputHelper Output { get; } = output;
}

[Feature("Subscribers see different articles based on their subscription level")]
public partial class StepTests
{
    [Scenario("Free subscribers see only the free articles")]
    async Task Scenario() => await this.NonExecutableAsync();       
    
    [Given("Given Free Frieda has a free subscription")]
    async Task Given() => await this.NonExecutableAsync();       

    [When("When Free Frieda logs in with her valid credentials")]
    async Task When() => await this.NonExecutableAsync();       
    
    [Then("Then she sees a Free article")]
    async Task Then() => await this.NonExecutableAsync();       
}