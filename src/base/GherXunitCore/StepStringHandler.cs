#nullable enable
using System.Runtime.CompilerServices;

namespace GherXunit.Annotations;

[InterpolatedStringHandler]
public ref struct StepStringHandler()
{
    private string _message = "";

    public void AppendLiteral(string s) => _message += HighlightKeyword(s);
    public void AppendFormatted<T>(T value) => _message += value?.ToString();
    public override string ToString() => _message;

    private string HighlightKeyword(string input)
    {
        return input
            .Replace("Given", "GIVEN".PadLeft(5)) // Verde
            .Replace("When",  "WHEN".PadLeft(5)) // Azul
            .Replace("Then",  "THEN".PadLeft(5)) // Amarelo
            .Replace("And",   "AND".PadLeft(5)); // Ciano
    }
}