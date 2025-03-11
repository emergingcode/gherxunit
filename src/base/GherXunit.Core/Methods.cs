#nullable enable
using System.Reflection;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace GherXunit.Annotations;

public static class GherXunitSteps
{
    // Background methods
    public static void Background(this IGherXunitStep scenario, Delegate refer, string steps, IGherXunitLexer? lexer = null) => InternalExecute(scenario, refer.Method, steps, true, lexer);
    public static async Task BackgroundAsync(this IGherXunitStep scenario, Delegate refer, string steps, IGherXunitLexer? lexer = null) => await InternalExecuteAscync(scenario, refer.Method, steps, true, lexer);
    
    // Execute methods
    public static void Execute(this IGherXunitStep scenario, string steps, Delegate? refer = null, object[]? param = null,  IGherXunitLexer? lexer = null) => InternalExecute(scenario, refer?.Method, steps, false, lexer, param);
    public static async Task ExecuteAscync(this IGherXunitStep scenario, string steps, Delegate? refer = null, object[]? param = null, IGherXunitLexer? lexer = null) => await InternalExecuteAscync(scenario, refer?.Method, steps, false, lexer, param);

    // Non Executable 
    public static void NonExecutable(this IGherXunitStep feature, string steps, IGherXunitLexer? lexer = null) => InternalExecute(feature, null, steps, false, lexer);
    public static async Task NonExecutableAsync(this IGherXunitStep scenario, string steps, IGherXunitLexer? lexer = null) => await InternalExecuteAscync(scenario, null, steps, false, lexer);

    // Private methods
    private static void InternalExecute(this IGherXunitStep scenario, MethodInfo? method, string? steps,
        bool isBackground = false, IGherXunitLexer? lexer = null, params object[]? param)
    {
        try
        {
            method?.Invoke(scenario, param);
            scenario.Write(method?.Name, steps, false, isBackground, lexer);
        }
        catch (Exception)
        {
            scenario.Write(method?.Name, steps, true, isBackground, lexer);
            throw;
        }
    }

    private static async Task InternalExecuteAscync(this IGherXunitStep scenario, MethodInfo? method, string? steps,
        bool isBackground = false, IGherXunitLexer? lexer = null, params object[]? param)
    {
        try
        {
            var task = method is null ? Task.CompletedTask : (Task)method.Invoke(scenario, param)!;
            await task;

            scenario.Write(method?.Name, steps, false, isBackground, lexer);
        }
        catch (Exception)
        {
            scenario.Write(method?.Name, steps, true, isBackground, lexer);
            throw;
        }
    }

    private static void Write(this IGherXunitStep scenario, string? methodName, string? steps, bool isException = false, bool isBackground = false, IGherXunitLexer? lexer = null)
    {
        if (steps is null) return;

        var iTest = GetTest(scenario, out var output);
        var featuresText = TryGetFeature(iTest);
        var statusResult = $"TEST RESULT: {(isException ? "\U0001F534 FAIL" : "\U0001F7E2 SUCCESS")}\r\n";

        var scenarioText = iTest is null
            ? $"{(isBackground ? "Background" : "Scenario")} {methodName}\r\n"
            : $"{(isBackground ? "Background" : "Scenario")} {iTest.DisplayName}\r\n";
        
        var stepString = new StringHandler(lexer ?? Lexers.EnUs);
        stepString.AppendLiteral($"{statusResult}{featuresText}{scenarioText}{steps}");

        output?.WriteLine(".");
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

    private static string? TryGetFeature(ITest? test)
    {
        if (test is null) return null;

        var attributes = test.TestCase.TestMethod.TestClass.Class.GetCustomAttributes(typeof(FeatureAttribute));
        var attribute = attributes.FirstOrDefault();

        var result = attribute?.GetNamedArgument<string>("Description");
        return result is null ? null : $"Feature {result}\r\n";
    }
}