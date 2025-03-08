namespace GherXunit.Annotations;

/// <summary>
/// The source code of the methods that are used to execute the Gherkin steps.
/// This works as a superset of the Xunit methods and it is used to create the custom methods
/// </summary>
public struct GherXunitMethods
{
    internal const string SOURCE =
        """
        #nullable enable
        using System.Reflection;
        using Xunit.Abstractions;
        using Xunit.Sdk;

        namespace GherXunit.Annotations;

        public static class GherXunitSteps
        {
            // Async methods
            public static async Task BackgroundAsync(this IGherXunitStep scenario, Delegate refer, string steps) => await ExecuteAscync(scenario, refer.Method, steps, true);
            public static async Task ExecuteAscync(this IGherXunitStep scenario, string steps) => await ExecuteAscync(scenario, null, steps, false);
            public static async Task ExecuteAscync(this IGherXunitStep scenario, Delegate refer, string steps) => await ExecuteAscync(scenario, refer.Method, steps);
            public static async Task ExecuteAscync(this IGherXunitStep scenario, Delegate refer, object[] param, string steps) => await ExecuteAscync(scenario, refer.Method, steps, false, param);
            public static async Task NonExecutableAsync(this IGherXunitStep scenario, string? steps = null) => await ExecuteAscync(scenario, null, steps, false);
        
            // Sync methods
            public static void Background(this IGherXunitStep scenario, Delegate refer, string steps) => Execute(scenario, refer.Method, steps, true);
            public static void Execute(this IGherXunitStep scenario, string steps) => Execute(scenario, null, steps, false);
            public static void Execute(this IGherXunitStep scenario, Delegate refer, string steps) => Execute(scenario, refer.Method, steps);
            public static void Execute(this IGherXunitStep scenario, Delegate refer, object[] param, string steps) => Execute(scenario, refer.Method, steps, false, param);
            public static void NonExecutable(this IGherXunitStep feature, string? steps) => Execute(feature, null, steps, false, []);
        
            // Private methods
            private static void Execute(this IGherXunitStep scenario, MethodInfo? method, string? steps,
                bool isBackground = false, params object?[] param)
            {
                try
                {
                    method?.Invoke(scenario, param);
                    scenario.Write(method?.Name, steps, false, isBackground);
                }
                catch (Exception)
                {
                    scenario.Write(method?.Name, steps, true, isBackground);
                    throw;
                }
            }
        
            private static async Task ExecuteAscync(this IGherXunitStep scenario, MethodInfo? method, string? steps,
                bool isBackground = false, params object?[] param)
            {
                try
                {
                    var task = method is null ? Task.CompletedTask : (Task)method.Invoke(scenario, param)!;
                    await task;
        
                    scenario.Write(method?.Name, steps, false, isBackground);
                }
                catch (Exception)
                {
                    scenario.Write(method?.Name, steps, true, isBackground);
                    throw;
                }
            }
        
            private static void Write(this IGherXunitStep scenario, string? methodName, string? steps, bool isException = false, bool isBackground = false)
            {
                if (steps is null) return;
        
                var iTest = GetTest(scenario, out var output);
                var featuresText = TryGetFeature(iTest);
                var statusResult = $"TEST RESULT: {(isException ? "\U0001F534 FAIL" : "\U0001F7E2 SUCCESS")}\r\n";
        
                var scenarioText = iTest is null
                    ? $"{(isBackground ? "Background" : "Scenario")} {methodName}\r\n"
                    : $"{(isBackground ? "Background" : "Scenario")} {iTest.DisplayName}\r\n";
        
                var stepString = new StepStringHandler();
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
        """;
}