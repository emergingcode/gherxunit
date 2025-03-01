using GherXunit.Annotations;

namespace BddTests.Samples.Backgrounds;

[Feature("Multiple site support Only blog owners can post to a blog, except administrators, who can post to all blogs.")]
public partial class BackgroundTest
{
    [Background]
    public void Setup() => this.Execute(
        steps: """
               Given a global administrator named <<"Greg">>
               And a blog named <<"Greg's anti-tax rants">>
               And a customer named <<"Dr. Bill">>
               And a blog named <<"Expensive Therapy">> owned by <<"Dr. Bill">>
               """);

    [Scenario("Dr. Bill posts to his own blog")]
    public async Task Scenario01() => await this.ExecuteAscync(
        refer: Step01,
        steps: """
               Given I am logged in as Dr. Bill
               When I try to post to "Expensive Therapy"
               Then I should see "Your article was published."
               """);

    [Scenario("Dr. Bill tries to post to somebody else's blog, and fails")]
    public async Task Scenario02() => await this.ExecuteAscync(
        refer: Step02,
        steps: """
               Given I am logged in as Dr. Bill
               When I try to post to "Greg's anti-tax rants"
               Then I should see "Hey! That's not your blog!"
               """);

    [Scenario("Greg posts to a client's blog")]
    public async Task Scenario03() => await this.ExecuteAscync(
        refer: Step03,
        steps: """
               Given I am logged in as Greg
               When I try to post to "Expensive Therapy"
               Then I should see "Your article was published."
               """);
}