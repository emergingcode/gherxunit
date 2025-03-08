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
            .Replace("Feature",    $"{"\u2937 FEATURE",8}")
            .Replace("Scenario",   $"{"\u2937 SCENARIO",13}")
            .Replace("Background", $"{"\u2937 BACKGROUND",15}")
            .Replace("Given",      $"{"|",7} {"GIVEN",5} \u2198")
            .Replace("When",       $"{"|",7} {"WHEN" ,5} \u2198")
            .Replace("Then",       $"{"|",7} {"THEN" ,5} \u2198")
            .Replace("And",        $"{"|",7} {"AND"  ,5} \u2198");
    }
}