#nullable enable
using System.Reflection;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace GherXunit.Annotations;

public static class GherXunitSteps
{
    public static async Task ExecuteAscync(this IGherXunitStep feature, string steps) => await ExecuteAscync(feature, null, steps, []);
    public static async Task ExecuteAscync(this IGherXunitStep feature, Delegate refer, string steps) => await ExecuteAscync(feature, refer.Method, steps);
    public static async Task ExecuteAscync(this IGherXunitStep feature, Delegate refer, object[] param, string steps) => await ExecuteAscync(feature, refer.Method, steps, param);
    public static async Task NonExecutableAsync(this IGherXunitStep feature, string? steps = null) => await feature.WriteAsync(null, steps);

    public static void Execute(this IGherXunitStep feature, string steps) => Execute(feature, null, steps, []);
    public static void Execute(this IGherXunitStep feature, Delegate refer, string steps) => Execute(feature, refer.Method, steps);
    public static void Execute(this IGherXunitStep feature, Delegate refer, object[] param, string steps) => Execute(feature, refer.Method, steps, param);
    public static void NonExecutable(this IGherXunitStep feature, string? steps) => feature.Write(null, steps);

    private static void Execute(this IGherXunitStep feature, MethodInfo? method, string steps, params object?[] param)
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

    private static async Task ExecuteAscync(this IGherXunitStep feature, MethodInfo? method, string steps, params object?[] param)
    {
        try
        {
            var task = method is null ? Task.CompletedTask : (Task)method.Invoke(feature, param)!;
            await task;

            await feature.WriteAsync(method?.Name, steps);
        }
        catch (Exception)
        {
            await feature.WriteAsync(method?.Name, steps, true);
            throw;
        }
    }

    private static async Task WriteAsync(this IGherXunitStep feature, string? methodName, string? steps, bool isException = false)
    {
        if (steps is null) return;
        Write(feature, methodName, steps, isException);
        await Task.Yield();
    }

    private static void Write(this IGherXunitStep feature, string? methodName, string? steps, bool isException = false)
    {
        var status = isException ? "\u274c" : "\u2705";

        if (steps is null) return;
        var stepString = new StepStringHandler();
        stepString.AppendLiteral(steps);

        var displayName = methodName is null ? status : $"{status} {methodName}";
        if (feature is IGherXunit { Output: { } output })
        {
            var iTest = GetTest(output);
            displayName = iTest is null ? status : $"{status} {iTest.DisplayName}";

            output.WriteLine(displayName);
            output.WriteLine(stepString.ToString());
            output.WriteLine(string.Empty);
        }

        Console.WriteLine(displayName);
        Console.WriteLine(steps);
        Console.WriteLine(string.Empty);
    }

    public static ITest? GetTest(ITestOutputHelper outputHelper)
    {
        if (outputHelper is not TestOutputHelper testOutputHelper) return null;
        
        var testField = 
            typeof(TestOutputHelper).GetField("test", BindingFlags.Instance | BindingFlags.NonPublic) ?? 
            typeof(TestOutputHelper).GetField("_test", BindingFlags.Instance | BindingFlags.NonPublic);

        return testField?.GetValue(testOutputHelper) as ITest;
    }
}