#nullable enable
using System.Runtime.CompilerServices;

namespace GherXunit.Annotations;

[InterpolatedStringHandler]
public class StringHandler(IGherXunitLexer lexer)
{
    private string _message = "";

    public void AppendLiteral(string s) => _message += HighlightKeyword(s);
    public override string ToString() => _message;

    private string HighlightKeyword(string input)
    {
        int[] words =
        [
            lexer.Given.Max(x => x.Value.Length),
            lexer.When.Max(x => x.Value.Length),
            lexer.Then.Max(x => x.Value.Length),
            lexer.And.Max(x => x.Value.Length)
        ];
            
        var max = words.Max() + 1;
        
        lexer.Given.ToList().ForEach(x => input = input.Replace($"\n{x.Key}", $"\n{"|",5}{x.Value.PadLeft(max)} \u2198"));
        lexer.When.ToList().ForEach(x => input = input.Replace($"\n{x.Key}", $"\n{"|",5}{x.Value.PadLeft(max)} \u2198"));
        lexer.Then.ToList().ForEach(x => input = input.Replace($"\n{x.Key}", $"\n{"|",5}{x.Value.PadLeft(max)} \u2198"));
        lexer.And.ToList().ForEach(x => input = input.Replace($"\n{x.Key}", $"\n{"|",5}{x.Value.PadLeft(max)} \u2198"));

        return input
            .Replace(nameof(lexer.Feature), $"\u2937 {lexer.Feature}")
            .Replace(nameof(lexer.Scenario), $"{"\u2937",3} {lexer.Scenario}")
            .Replace(nameof(lexer.Background), $"{"\u2937",3} {lexer.Background}");
    }
}

public static class Lexers
{
    public static IGherXunitLexer EnUs => new DefaultGherXunitLexer();
    public static IGherXunitLexer PtBr => new PtBrGherXunitLexer();
}

public record DefaultGherXunitLexer : IGherXunitLexer
{
    public (string Key, string Value)[] Given => [("Given", "GIVEN")];
    public (string Key, string Value)[] When => [("When", "WHEN")];
    public (string Key, string Value)[] Then => [("Then", "THEN")];
    public (string Key, string Value)[] And => [("And", "AND")];
    public string Background => "BACKGROUND";
    public string Scenario => "SCENARIO";
    public string Feature => "FEATURE";
}

public record PtBrGherXunitLexer : IGherXunitLexer
{
    public (string Key, string Value)[] Given => [("Dado", "DADO"), ("Dada", "DADA"), ("Dados", "DADOS"), ("Dadas", "DADAS")];
    public (string Key, string Value)[] When => [("Quando", "QUANDO")];
    public (string Key, string Value)[] Then => [("Então", "ENTÃO"), ("Entao", "ENTAO")];
    public (string Key, string Value)[] And => [("E", "E")];
    public string Background => "CONTEXTO";
    public string Scenario => "CENARIO";
    public string Feature => "FUNCIONALIDADE";
}