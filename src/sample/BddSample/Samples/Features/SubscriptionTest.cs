using GherXunit.Annotations;

namespace BddTests.Samples.Features;

[Feature("Subscribers see different articles based on their subscription level")]
public partial class SubscriptionTest
{
    [Scenario("Free subscribers see only the free articles")]
    async Task WhenFriedaLogs() => await this.ExecuteAscync(
        refer: WhenFriedaLogsSteps,
        steps: """
               Given Free Frieda has a free subscription
               When Free Frieda logs in with her valid credentials
               Then she sees a Free article
               """);

    [Scenario("Subscriber with a paid subscription can access both free and paid articles")]
    void WhenPattyLogs() => this.Execute(
        refer: WhenPattyLogsSteps,
        steps: """
               Given Paid Patty has a basic-level paid subscription
               When Paid Patty logs in with her valid credentials
               Then she sees a Free article and a Paid article
               """);
}