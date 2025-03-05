namespace GherXunit.Annotations;

/// <summary>
/// The source code of the methods that are used to execute the Gherkin steps.
/// This works as a superset of the Xunit methods and it is used to create the custom methods
/// </summary>
public struct StepSourceCode
{
    public const string SOURCE =
        """
        #nullable enable
        using System.Reflection;

        namespace GherXunit.Annotations;
        
        public static partial class GherXunitSteps
        {
            /// <summary>
            /// Execute the steps in the scenario asynchronously.
            /// </summary>
            /// <param name="feature">Feature</param>
            /// <param name="steps">Steps executed</param>
            public static async Task ExecuteAscync(this IGherXunit feature, string steps)
                => await ExecuteAscync(feature, null, steps, []);
        
            /// <summary>
            /// Execute the steps in the scenario asynchronously.
            /// </summary>
            /// <param name="feature">Feature</param>
            /// <param name="refer">The method to be executed</param>
            /// <param name="steps">Steps executed</param>
            public static async Task ExecuteAscync(this IGherXunit feature, Delegate refer, string steps)
                => await ExecuteAscync(feature, refer.Method, steps, []);
        
            /// <summary>
            /// Execute the steps in the scenario asynchronously.
            /// </summary>
            /// <param name="feature">Feature</param>
            /// <param name="refer">The method to be executed</param>
            /// <param name="param">Parameters</param>
            /// <param name="steps">Steps executed</param>
            public static async Task ExecuteAscync(this IGherXunit feature, Delegate refer, object[] param, string steps)
                => await ExecuteAscync(feature, refer.Method, steps, param);
        }

        /// <summary>
        /// # Sync Methods for Gherkin Steps
        /// See the <see href="https://cucumber.io/docs/gherkin/reference#steps">cucumber.io</see> documentation
        /// or <see href="https://dannorth.net/introducing-bdd/">introducing-bdd</see> page for more information.
        /// </summary>
        public static partial class GherXunitSteps
        {
            /// <summary>
            /// Execute the steps in the scenario synchronously.
            /// </summary>
            /// <param name="feature">Feature</param>
            /// <param name="steps">Steps executed</param>
            public static void Execute(this IGherXunit feature, string steps)
                => Execute(feature, null, steps, []);
        
            /// <summary>
            /// Execute the steps in the scenario synchronously.
            /// </summary>
            /// <param name="feature">Feature</param>
            /// <param name="refer">The method to be executed</param>
            /// <param name="steps">Steps executed</param>
            public static void Execute(this IGherXunit feature, Delegate refer, string steps)
                => Execute(feature, refer.Method, steps, []);
        
            /// <summary>
            /// Execute the steps in the scenario synchronously.
            /// </summary>
            /// <param name="feature">Feature</param>
            /// <param name="refer">The method to be executed</param>
            /// <param name="param">Parameters</param>
            /// <param name="steps">Steps executed</param>    
            public static void Execute(this IGherXunit feature, Delegate refer, object[] param, string steps)
                => Execute(feature, refer.Method, steps, param);
        }

        /// <summary>
        /// Base Methods for Gherkin Steps
        /// See the <see href="https://cucumber.io/docs/gherkin/reference#steps">cucumber.io</see> documentation
        /// or <see href="https://dannorth.net/introducing-bdd/">introducing-bdd</see> page for more information.
        /// </summary>
        public static partial class GherXunitSteps
        {
            /// <summary>
            /// Just output the steps in the scenario.
            /// </summary>
            /// <param name="feature">Feature</param>
            /// <param name="steps">Steps executed</param>
            public static void NonExecutable(this IGherXunit feature, string? steps = null)
            {
                if (steps is not null) feature.Write(steps);
            }
        
            /// <summary>
            /// Just output the steps in the scenario.
            /// </summary>
            /// <param name="feature">Feature</param>
            /// <param name="steps">Steps executed</param>    
            public static Task NonExecutableAsync(this IGherXunit feature, string? steps = null)
            {
                if (steps is not null) feature.Write(steps);
                return Task.CompletedTask;
            }
        
            private static void Execute(this IGherXunit feature, MethodInfo? method, string steps, params object?[] param)
            {
                try
                {
                    method?.Invoke(feature, param);
                    feature.Write(steps);
                }
                catch (Exception)
                {
                    feature.Write(steps, true);
                    throw;
                }
            }
        
            private static async Task ExecuteAscync(this IGherXunit feature, MethodInfo? method, string steps,
                params object?[] param)
            {
                try
                {
                    if (method is not null)
                    {
                        var task = (Task)method.Invoke(feature, param)!;
                        await task;
                    }
        
                    feature.Write(steps);
                }
                catch (Exception)
                {
                    feature.Write(steps, true);
                    throw;
                }
            }
        
            private static void Write(this IGherXunit feature, string steps, bool isException = false)
            {
                feature.Output.WriteLine(isException ? "🔴 TEST FAIL" : "🟢 TEST OK");
                feature.Output.WriteLine(steps);
            }
        }
        """;
}