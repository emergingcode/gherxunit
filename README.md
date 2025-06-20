# 🚀 GherXUnit: An Alternative for BDD with xUnit

[![NuGet](https://img.shields.io/nuget/v/GherXunit.svg)](https://www.nuget.org/packages/GherXunit)

## Introduction

GherXunit is a BDD extension for xUnit, allowing you to write Gherkin scenarios directly in your xUnit tests, without the need for external tools like Cucumber or SpecFlow.

## Why GherXunit?

- **Gherkin syntax directly in xUnit**: No extra dependencies.
- **Modular and organized code**: Use partial classes to separate scenarios and steps.
- **Smooth integration**: Easily mix BDD and traditional unit tests.

## Getting Started

Install via [NuGet](https://www.nuget.org/packages/GherXunit/):

```sh
dotnet add package GherXunit
```

## Usage

### Scenario Definition

```csharp
using GherXunit.Annotations;

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
}
```

### Step Implementation

```csharp
public partial class SubscriptionTest(ITestOutputHelper output): IGherXunit
{
    public ITestOutputHelper Output { get; } = output;
    private void WhenPattyLogsSteps() => Assert.True(true);
    private async Task WhenFriedaLogsSteps() => await Task.CompletedTask;
}
```

### Output Example

```gherkindotnet
TEST RESULT: 🟢 SUCCESS
⤷ FEATURE Subscribers see different articles based on their subscription level
  ⤷ SCENARIO Free subscribers see only the free articles
    | GIVEN ↘ Free Frieda has a free subscription
    |  WHEN ↘ Free Frieda logs in with her valid credentials
    |  THEN ↘ she sees a Free article
```

## Customizing the Lexer

You can customize the Gherkin keywords (Given, When, Then, etc.) by implementing `IGherXunitLexer`. Built-in lexers include `Lexers.PtBr` (Portuguese) and `Lexers.EnUs` (English).

#### Example: Custom Emoji Lexer

```csharp
public record EmojiGherXunitLexer : IGherXunitLexer
{
    public (string Key, string Value)[] Given => [("Given", "😐")];
    public (string Key, string Value)[] When => [("When", "🎬")];
    public (string Key, string Value)[] Then => [("Then", "🙏")];
    public (string Key, string Value)[] And => [("And", "😂")];
    public string Background => "💤";
    public string Scenario => "🥒📕";
    public string Feature => "📚";
}
```

## Setting a Global Lexer

From version 1.3.0, you can set a default lexer for all tests in your project or class using `GherXunitConfig.DefaultLexer`:

```csharp
public partial class LocalizationTest
{
    static LocalizationTest()
    {
        GherXunitConfig.DefaultLexer = Lexers.PtBr;
    }

    [Scenario("Free articles in Portuguese")]
    async Task WhenFriedaLogs() => await this.ExecuteAscync(
        refer: WhenFriedaLogsSteps,
        steps: """
               Dado Free Frieda possui uma assinatura gratuita
               Quando Free Frieda faz login com suas credenciais válidas
               Então ela vê um artigo gratuito
               """);

    // To override the default lexer for a specific scenario:
    [Scenario("Custom emoji lexer")]
    void WhenPattyLogs() => this.Execute(
        refer: WhenPattyLogsSteps,
        lexer: new EmojiGherXunitLexer(),
        steps: """
               Given Paid Patty has a basic-level paid subscription
               When Paid Patty logs in with her valid credentials
               Then she sees a Free article and a Paid article
               """);
}
```

> **Tip:** Only specify the `lexer` parameter if you want to override the global default.

## Is GherXunit for You?

If your team already uses xUnit and wants to experiment with a BDD approach without drastically changing its workflow, **GherXunit** may be an option to consider. See more usage examples and implementation details for `Background`, `Rule`, `Features`, and other elements in the [sample code](/src/base/GherXunit.Core/Samples).

## References

- Farooq, M. S., et al. (2023). Behavior Driven Development: A Systematic Literature Review. IEEE Access. [DOI](https://doi.org/10.1109/ACCESS.2023.3302356)
- North, D. (2006). Introducing BDD. [DanNorth.net](https://dannorth.net/introducing-bdd/)
- xUnit. (2023). [xUnit.net](https://xunit.net/)
- Gherkin. (2023). [Gherkin](https://cucumber.io/docs/gherkin/)
