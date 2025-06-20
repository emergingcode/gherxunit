using GherXunit.Annotations;

namespace BddTests.Samples.Localization;

[Feature("Subscribers see different articles based on their subscription level")]
public partial class LocalizationTest
{
    static LocalizationTest()
    {
        GherXunitConfig.DefaultLexer = Lexers.PtBr;
    }

    [Scenario("Inscrever-se para ver artigos gratuitos")]
    async Task WhenFriedaLogs() => await this.ExecuteAscync(
        refer: WhenFriedaLogsSteps,
        steps: """
               Dado Free Frieda possui uma assinatura gratuita
               Quando Free Frieda faz login com suas credenciais válidas
               Então ela vê um artigo gratuito
               """);

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