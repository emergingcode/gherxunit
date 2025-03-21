# 🚀 GherXUnit: An Alternative for BDD with xUnit
[🇧🇷 Versão em Português](README_PTBR.md) | [🇬🇧 English Version](README.md)  
[![NuGet](https://img.shields.io/nuget/v/GherXunit.svg)](https://www.nuget.org/packages/GherXunit)

The adoption of Behavior-Driven Development (BDD) has become increasingly common in software development, promoting better communication between technical and non-technical teams. However, its integration with traditional testing frameworks is not always straightforward.

> [!IMPORTANT]  
> According to the study *Behavior Driven Development: A Systematic Literature Review* (Farooq et al., 2023, IEEE Access), some recurring difficulties in using BDD include:
> - **Complex automation**: Integration with external tools can increase configuration and test execution complexity.
> - **Difficult maintenance**: As the test base grows, Gherkin scenarios can become hard to manage.
> - **Learning curve**: The need to master new tools can hinder BDD adoption, especially in teams already familiar with traditional frameworks.

**GherXunit** emerges as a viable alternative for teams looking to explore the benefits of BDD within the xUnit framework, without requiring external tools such as Cucumber or SpecFlow. It acts as a superset of xUnit, allowing tests to be written in Gherkin.

### ✅ Where Can GherXunit Help?

**GherXunit** aims to offer an alternative for teams already using xUnit and looking to incorporate the BDD structure without completely changing their tools. Among its benefits are:

- ✔ **Using Gherkin syntax directly in xUnit**, reducing external dependencies.
- ✔ **More modular and organized code**, using partial classes to separate scenarios and steps.
- ✔ **Better integration with unit tests**, allowing a smoother transition between different levels of testing.

### 📦 Getting Started

This package is available through [Nuget Packages](https://www.nuget.org/packages/GherXunit/).

| Version                                                                                        | Downloads | Status |  
|------------------------------------------------------------------------------------------------| ----- |----- |
| [![NuGet](https://img.shields.io/nuget/v/GherXunit.svg)](https://www.nuget.org/packages/GherXunit) | [![Nuget](https://img.shields.io/nuget/dt/GherXunit.svg)](https://www.nuget.org/packages/GherXunit) | [![.NET](https://github.com/emergingcode/gherxunit/actions/workflows/dotnet.yml/badge.svg)](https://github.com/emergingcode/gherxunit/actions/workflows/dotnet.yml) |


### 💡 How Does It Work?

The core idea of **GherXunit** is to allow test scenarios to be written in a structure familiar to those already using xUnit.
For that, it provides a set of attributes and methods that allow the definition of test scenarios using Gherkin syntax.
The following sections provide examples of how to define test scenarios and implement step methods using **GherXunit**.

#### 📌 Example of Scenario Definition:
The following code snippet shows a test scenario defined using Gherkin syntax in a class named `SubscriptionTest`:

```csharp
using GherXunit.Annotations;
...

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

#### 📌 Example of Step Implementation:
The following code snippet shows the implementation of the step methods for the test scenario defined in the `SubscriptionTest` class:
```csharp
public partial class SubscriptionTest(ITestOutputHelper output): IGherXunit
{
    public ITestOutputHelper Output { get; } = output;
    private void WhenPattyLogsSteps() => Assert.True(true);
    private async Task WhenFriedaLogsSteps() => await Task.CompletedTask;
}
```

> [!TIP]  
> In this example, the `SubscriptionTest` class is split into two files. The first file defines the test scenarios, while the second file defines the step methods. Using `partial` allows both files to contribute to the definition of the same `SubscriptionTest` class.

#### 📌 Example of output highlighting the test results:
The result of running the test scenarios defined in the `SubscriptionTest` class would be similar to the following output:
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

### ✏️ Customizing the lexical elements of Gherkin

The **GherXunit** allows you to customize the lexical elements of Gherkin, such as `Given`, `When`, `Then`, `And`, `Background`, `Scenario`, and `Feature`. 
You can define your custom emojis or symbols to represent these elements. The following code snippet shows an example of a custom lexer for emojis:
```csharp
// Custom lexer for emojis
public record EmojiGherXunitLexer : IGherXunitLexer
{
    public (string Key, string Value)[] Given => [("Given", "\ud83d\ude10")];
    public (string Key, string Value)[] When => [("When", "\ud83c\udfac")];
    public (string Key, string Value)[] Then => [("Then", "\ud83d\ude4f")];
    public (string Key, string Value)[] And => [("And", "\ud83d\ude02")];
    public string Background => "\ud83d\udca4";
    public string Scenario => "\ud83e\udd52\ud83d\udcd5";
    public string Feature => "\ud83d\udcda";
}
```
The Gherkin provides two built-in lexers: `Lexers.PtBr` for Portuguese (🇵🇹🇧🇷) and `Lexers.EnUs` for English (🇺🇸). 
You can also create your custom lexer by implementing the `IGherXunitLexer` interface. To use the custom lexer, 
you need to pass it as a parameter when defining the test scenario.

```csharp
[Feature("Subscribers see different articles based on their subscription level")]
public partial class LocalizationTest
{
    // Using Portuguese (🇵🇹🇧🇷) lexer
    [Scenario("Inscrever-se para ver artigos gratuitos")]
    async Task WhenFriedaLogs() => await this.ExecuteAscync(
        refer: WhenFriedaLogsSteps,
        lexer: Lexers.PtBr,
        steps: """
               Dado Free Frieda possui uma assinatura gratuita
               Quando Free Frieda faz login com suas credenciais válidas
               Então ela vê um artigo gratuito
               """);

    // Using custom emoji lexer
    [Scenario("Subscriber with a paid subscription can access both free and paid articles")]
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
The result of running the test scenarios defined in the `LocalizationTest` class using the custom lexer would be similar to the following output:
```gherkindotnet
TEST RESULT: 🟢 SUCCESS
⤷ FUNCIONALIDADE Subscribers see different articles based on their subscription level
  ⤷ CENARIO Inscrever-se para ver artigos gratuitos
    |   DADO ↘ Free Frieda possui uma assinatura gratuita
    | QUANDO ↘ Free Frieda faz login com suas credenciais válidas
    |  ENTÃO ↘ ela vê um artigo gratuito
    
TEST RESULT: 🟢 SUCCESS
⤷ 📚 Subscribers see different articles based on their subscription level
  ⤷ 🥒📕 Subscriber with a paid subscription can access both free and paid articles
    | 😐 ↘ Paid Patty has a basic-level paid subscription
    | 🎬 ↘ Paid Patty logs in with her valid credentials
    | 🙏 ↘ she sees a Free article and a Paid article    
```

### 🔎 Is GherXunit for You?
If your team already uses xUnit and wants to experiment with a BDD approach without drastically changing its workflow, **GherXunit** may be an option to consider. It does not eliminate all BDD challenges but seeks to facilitate its adoption in environments where xUnit is already widely used.
See more usage examples and implementation details for `Background`, `Rule`, `Features`, and other elements in the [sample code](/src/base/GherXunit.Core/Samples) available in the **GherXunit** repository.


## 📚 References

- 📖 **Farooq, M. S., et al. (2023). Behavior Driven Development**: *A Systematic Literature Review. IEEE* Access. DOI: [10.1109/ACCESS.2023.3302356](https://doi.org/10.1109/ACCESS.2023.3302356).
- 📖 **North, D. (2006)**. *Introducing BDD. DanNorth.net.* Available at: [https://dannorth.net/introducing-bdd/](https://dannorth.net/introducing-bdd/).
- 📖 **xUnit. (2023)**. *xUnit.net.* Available at: [https://xunit.net/](https://xunit.net/).
- 📖 **Gherkin. (2023)**. *Gherkin.* Available at: [https://cucumber.io/docs/gherkin/](https://cucumber.io/docs/gherkin/).
