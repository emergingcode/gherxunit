# 🚀 GherXUnit: Uma Alternativa para BDD com xUnit
[🇧🇷 Versão em Português](README_PTBR.md) | [🇬🇧 English Version](README.md)  
[![NuGet](https://img.shields.io/nuget/v/GherXunit.svg)](https://www.nuget.org/packages/GherXunit)

A adoção do Behavior-Driven Development (BDD) tem se tornado cada vez mais comum no desenvolvimento de software, promovendo melhor comunicação entre times técnicos e não técnicos. No entanto, sua integração com frameworks tradicionais de testes nem sempre é simples.

> [!IMPORTANT]  
> De acordo com o estudo **“Behavior Driven Development: A Systematic Literature Review” (Farooq et al., 2023, IEEE Access)**, algumas dificuldades recorrentes no uso do BDD incluem
> - **Automação complexa**: A integração com ferramentas externas pode aumentar a complexidade da configuração e execução dos testes.
> - **Manutenção difícil**: À medida que a base de testes cresce, cenários Gherkin podem se tornar difíceis de gerenciar.
> - **Curva de aprendizado**: A necessidade de dominar novas ferramentas pode dificultar a adoção do BDD, especialmente em times já acostumados com frameworks tradicionais.

O **GherXunit** surge como uma alternativa viável para equipes que desejam explorar os benefícios do BDD dentro da estrutura do xUnit, sem precisar de ferramentas externas como Cucumber ou SpecFlow. Ele atua como um superset de xUnit, permitindo a escrita de testes em Gherkin.

### ✅ Onde o GherXunit Pode Ajudar?

O GherXUnit busca oferecer uma alternativa para equipes que já utilizam xUnit e gostariam de incorporar a estrutura do BDD sem mudar completamente suas ferramentas. Entre os seus benefícios, estão:

- ✔ **Uso da sintaxe Gherkin diretamente no xUnit**, reduzindo dependências externas.
- ✔ **Código mais modular e organizado**, utilizando partial classes para separar cenários e passos.
- ✔ **Maior integração com testes unitários**, permitindo uma transição mais suave entre diferentes níveis de teste.

### 📦 Começando

Este pacote está disponível através do [Nuget Packages](https://www.nuget.org/packages/GherXunit/).

| Version                                                                                        | Downloads | Status |  
|------------------------------------------------------------------------------------------------| ----- |----- |
| [![NuGet](https://img.shields.io/nuget/v/GherXunit.svg)](https://www.nuget.org/packages/GherXunit) | [![Nuget](https://img.shields.io/nuget/dt/GherXunit.svg)](https://www.nuget.org/packages/GherXunit) | [![.NET](https://github.com/emergingcode/gherxunit/actions/workflows/dotnet.yml/badge.svg)](https://github.com/emergingcode/gherxunit/actions/workflows/dotnet.yml) |


### 💡 Como funciona?

A ideia central do **GherXunit** é permitir que cenários de testes sejam escritos em uma estrutura familiar para quem já usa xUnit.
Para isso, ele fornece um conjunto de atributos e métodos que permitem a definição de cenários de teste usando a sintaxe Gherkin.
As seções a seguir fornecem exemplos de como definir cenários de teste e implementar métodos de passos usando o **GherXunit**.

####  📌 Exemplo de Definição de Cenário:
O trecho de código a seguir mostra um cenário de teste definido usando a sintaxe Gherkin em uma classe chamada `SubscriptionTest`:

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

#### 📌 Exemplo de Implementação de Passos:
O trecho de código a seguir mostra a implementação dos métodos de passos para o cenário de teste definido na classe `SubscriptionTest`:

```csharp
public partial class SubscriptionTest(ITestOutputHelper output): IGherXunit
{
    public ITestOutputHelper Output { get; } = output;
    private void WhenPattyLogsSteps() => Assert.True(true);
    private async Task WhenFriedaLogsSteps() => await Task.CompletedTask;
}
```

> [!TIP]  
> Neste exemplo, a classe `SubscriptionTest` é dividida em dois arquivos. O primeiro arquivo define os cenários de teste, enquanto o segundo arquivo define os métodos de passos. O uso de `partial` permite que ambos os arquivos contribuam para a definição da mesma classe `SubscriptionTest`.

#### 📌 Exemplo de saída destacando os resultados dos testes:
O resultado da execução dos cenários de teste definidos na classe `SubscriptionTest` seria semelhante à saída a seguir:

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

### ✏️ Customizando a Sintaxe Gherkin

O **GherXunit** permite personalizar os elementos lexicais do Gherkin, como `Given`, `When`, `Then`, `And`, `Background`, `Scenario` e `Feature`.
Você pode definir seus emojis ou símbolos personalizados para representar esses elementos. O trecho de código a seguir mostra um exemplo de um lexer personalizado para emojis:

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
O **GherXunit** fornece dois lexers embutidos: `Lexers.PtBr` para Português (🇵🇹🇧🇷) e `Lexers.EnUs` para Inglês (🇺🇸).
Você também pode criar seu lexer personalizado implementando a interface `IGherXunitLexer`. Para usar o lexer personalizado,
você precisa passá-lo como parâmetro ao definir o cenário de teste.

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

O resultado da execução dos cenários de teste definidos na classe `LocalizationTest` usando o lexer personalizado seria semelhante à saída a seguir:
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

### 🔎 O GherXunit é para você?

Se a sua equipe já usa xUnit e deseja experimentar uma abordagem mais próxima do BDD, sem mudar drasticamente seu fluxo de trabalho, o GherXunit pode ser uma opção a considerar. Ele não elimina todos os desafios do BDD, mas busca facilitar sua adoção em ambientes onde o xUnit já é amplamente utilizado.
Veja mais exemplos de uso e detalhes de implementação de `Background`, `Rule`, `Features` e outros elementos
no [código exemplo](/src/sample/BddSample/Samples) disponível no repositório do GherXUnit.


## 📚 Referências

- 📖 **Farooq, M. S., et al. (2023). Behavior Driven Development**: _A Systematic Literature Review. IEEE_ Access. DOI: [10.1109/ACCESS.2023.3302356](https://doi.org/10.1109/ACCESS.2023.3302356).
- 📖 **North, D. (2006)**. _Introducing BDD. DanNorth.net._ Disponível em: https://dannorth.net/introducing-bdd/.
- 📖 **xUnit. (2023)**. _xUnit.net._ Disponível em: https://xunit.net/.
- 📖 **Gherkin. (2023)**. _Gherkin._ Disponível em: https://cucumber.io/docs/gherkin/.


