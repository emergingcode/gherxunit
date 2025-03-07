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
            .Replace("Scenario", $"{"SCENARIO",8} ⇲") // Verde
            .Replace("Given", $"{"GIVEN",8} ⇲") // Verde
            .Replace("When", $"{"WHEN",8} ⇲") // Azul
            .Replace("Then", $"{"THEN",8} ⇲") // Amarelo
            .Replace("And", $"{"AND",8} ⇲"); // Ciano
    }
}