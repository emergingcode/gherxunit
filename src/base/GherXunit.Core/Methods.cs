#nullable enable
using System.Reflection;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace GherXunit.Annotations;

public static class GherXunitSteps
{
    // Async methods
    public static async Task ExecuteAscync(this IGherXunitStep feature, string steps) => await ExecuteAscync(feature, null, steps, []);
    public static async Task ExecuteAscync(this IGherXunitStep feature, Delegate refer, string steps) => await ExecuteAscync(feature, refer.Method, steps);
    public static async Task ExecuteAscync(this IGherXunitStep feature, Delegate refer, object[] param, string steps) => await ExecuteAscync(feature, refer.Method, steps, param);
    public static async Task NonExecutableAsync(this IGherXunitStep feature, string? steps = null) => await ExecuteAscync(feature, null, steps, []);

    // Sync methods
    public static void Execute(this IGherXunitStep feature, string steps) => Execute(feature, null, steps, []);
    public static void Execute(this IGherXunitStep feature, Delegate refer, string steps) => Execute(feature, refer.Method, steps);
    public static void Execute(this IGherXunitStep feature, Delegate refer, object[] param, string steps) => Execute(feature, refer.Method, steps, param);
    public static void NonExecutable(this IGherXunitStep feature, string? steps) => Execute(feature, null, steps, []);

    // Private methods
    private static void Execute(this IGherXunitStep feature, MethodInfo? method, string? steps, params object?[] param)
    {
        try
        {
            method?.Invoke(feature, param);
            feature.Write(method?.Name, steps);
        }
        catch (Exception)
        {
            feature.Write(method?.Name, steps, true);
            throw;
        }
    }

    private static async Task ExecuteAscync(this IGherXunitStep feature, MethodInfo? method, string? steps,
        params object?[] param)
    {
        try
        {
            var task = method is null ? Task.CompletedTask : (Task)method.Invoke(feature, param)!;
            await task;

            feature.Write(method?.Name, steps);
        }
        catch (Exception)
        {
            feature.Write(method?.Name, steps, true);
            throw;
        }
    }

    private static void Write(this IGherXunitStep feature, string? methodName, string? steps, bool isException = false)
    {
        if (steps is null) return;

        var status = isException ? "🔴" : "🟢";
        var iTest = GetTest(feature, out var output);

        var display = iTest is null
            ? $"Scenario [{status}]{methodName}\r\n{steps}"
            : $"Scenario [{status}]{iTest.DisplayName}\r\n{steps}";

        var stepString = new StepStringHandler();
        stepString.AppendLiteral(display);

        output?.WriteLine(string.Empty);
        output?.WriteLine(stepString.ToString());
        Console.WriteLine(string.Empty);
        Console.WriteLine(stepString.ToString());
    }

    private static ITest? GetTest(IGherXunitStep feature, out ITestOutputHelper? outputHelper)
    {
        outputHelper = null;
        if (feature is not IGherXunit { Output: { } output }) return null;

        var testField =
            typeof(TestOutputHelper).GetField("test", BindingFlags.Instance | BindingFlags.NonPublic) ??
            typeof(TestOutputHelper).GetField("_test", BindingFlags.Instance | BindingFlags.NonPublic);

        outputHelper = output;
        return testField?.GetValue(output) as ITest;
    }
}