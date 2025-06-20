# 🚀 GherXUnit: An Alternative for BDD with xUnit

[![NuGet](https://img.shields.io/nuget/v/GherXunit.svg)](https://www.nuget.org/packages/GherXunit)

The adoption of Behavior-Driven Development (BDD) has become increasingly common in software development, promoting better communication between technical and non-technical teams. However, its integration with traditional testing frameworks is not always straightforward.

> **Important:**
> According to the study _Behavior Driven Development: A Systematic Literature Review_ (Farooq et al., 2023, IEEE Access), some recurring difficulties in using BDD include:
> - **Complex automation:** Integration with external tools can increase configuration and test execution complexity.
> - **Difficult maintenance:** As the test base grows, Gherkin scenarios can become hard to manage.
> - **Learning curve:** The need to master new tools can hinder BDD adoption, especially in teams already familiar with traditional frameworks.

**GherXunit** emerges as a viable alternative for teams looking to explore the benefits of BDD within the xUnit framework, without requiring external tools such as Cucumber or SpecFlow. It acts as a superset of xUnit, allowing tests to be written in Gherkin.

---

## ✅ Where Can GherXunit Help?

- **Using Gherkin syntax directly in xUnit**, reducing external dependencies.
- **More modular and organized code**, using partial classes to separate scenarios and steps.
- **Better integration with unit tests**, allowing a smoother transition between different levels of testing.

---

## 📦 Getting Started

This package is available through [NuGet Packages](https://www.nuget.org/packages/GherXunit/).

| Version | Downloads | Status |
| ------- | --------- | ------ |
| [![NuGet](https://img.shields.io/nuget/v/GherXunit.svg)](https://www.nuget.org/packages/GherXunit) | [![Nuget](https://img.shields.io/nuget/dt/GherXunit.svg)](https://www.nuget.org/packages/GherXunit) | [![.NET](https://github.com/emergingcode/gherxunit/actions/workflows/dotnet.yml/badge.svg)](https://github.com/emergingcode/gherxunit/actions/workflows/dotnet.yml) |

Install via NuGet:

```sh
dotnet add package GherXunit
```

---

## 💡 How Does It Work?

The core idea of **GherXunit** is to allow test scenarios to be written in a structure familiar to those already using xUnit. For that, it provides a set of attributes and methods that allow the definition of test scenarios using Gherkin syntax.

### 📌 Example of Scenario Definition

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

    [Scenario("Subscriber with a paid subscription can access both free and paid articles")]
    void WhenPattyLogs() => this.Execute(
        refer: WhenPattyLogsSteps,
        steps: """
               Given Paid Patty has a basic-level paid subscription
               When Paid Patty logs in with her valid credentials
               Then she sees a Free article and a Paid article
               """);
}
```

### 📌 Example of Step Implementation

```csharp
public partial class SubscriptionTest(ITestOutputHelper output): IGherXunit
{
    public ITestOutputHelper Output { get; } = output;
    private void WhenPattyLogsSteps() => Assert.True(true);
    private async Task WhenFriedaLogsSteps() => await Task.CompletedTask;
}
```

> **Tip:** In this example, the `SubscriptionTest` class is split into two files. The first file defines the test scenarios, while the second file defines the step methods. Using `partial` allows both files to contribute to the definition of the same `SubscriptionTest` class.

### 📌 Example of Output

```gherkindotnet
TEST RESULT: 🟢 SUCCESS
⤷ FEATURE Subscribers see different articles based on their subscription level
  ⤷ SCENARIO Free subscribers see only the free articles
    | GIVEN ↘ Free Frieda has a free subscription
    |  WHEN ↘ Free Frieda logs in with her valid credentials
    |  THEN ↘ she sees a Free article

TEST RESULT: 🟢 SUCCESS
⤷ FEATURE Subscribers see different articles based on their subscription level
  ⤷ SCENARIO Subscriber with a paid subscription can access both free and paid articles
    | GIVEN ↘ Paid Patty has a basic-level paid subscription
    |  WHEN ↘ Paid Patty logs in with her valid credentials
    |  THEN ↘ she sees a Free article and a Paid article
```

---

## ✏️ Customizing the Lexer

GherXunit allows you to customize the lexical elements of Gherkin, such as `Given`, `When`, `Then`, `And`, `Background`, `Scenario`, and `Feature`. You can define your custom emojis or symbols to represent these elements. Built-in lexers include `Lexers.PtBr` (Portuguese) and `Lexers.EnUs` (English).

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

---

## 🌍 Setting a Global Lexer

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

---

## 🔎 Is GherXunit for You?

If your team already uses xUnit and wants to experiment with a BDD approach without drastically changing its workflow, **GherXunit** may be an option to consider. It does not eliminate all BDD challenges but seeks to facilitate its adoption in environments where xUnit is already widely used. See more usage examples and implementation details for `Background`, `Rule`, `Features`, and other elements in the [sample code](/src/base/GherXunit.Core/Samples) available in the **GherXunit** repository.

---

## 📚 References

- Farooq, M. S., et al. (2023). Behavior Driven Development: A Systematic Literature Review. IEEE Access. [DOI](https://doi.org/10.1109/ACCESS.2023.3302356)
- North, D. (2006). Introducing BDD. [DanNorth.net](https://dannorth.net/introducing-bdd/)
- xUnit. (2023). [xUnit.net](https://xunit.net/)
- Gherkin. (2023). [Gherkin](https://cucumber.io/docs/gherkin/)
