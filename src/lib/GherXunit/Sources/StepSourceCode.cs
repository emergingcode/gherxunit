namespace GherXunit.Sources;

internal struct StepSourceCode
{
    public const string SOURCE =
        """
        #nullable enable
        using System.Reflection;
        using System.Text;
        
        namespace GherXunit.Annotations;
        
        /// <summary>
        /// Async Methods for Gherkin Steps
        /// Each step starts with Given, When, Then, And, or But.
        /// Cucumber executes each step in a scenario one at a time, in the sequence you’ve written them in.
        /// When Cucumber tries to execute a step, it looks for a matching step definition to execute.
        /// Keywords are not taken into account when looking for a step definition.
        /// This means you cannot have a Given, When, Then, And or But step with the same text as another step.
        /// See the <see href="https://cucumber.io/docs/gherkin/reference#steps">cucumber.io</see> documentation
        /// or <see href="https://dannorth.net/introducing-bdd/">introducing-bdd</see> page for more information.
        /// </summary>
        internal static partial class GherXunitSteps
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
        internal static partial class GherXunitSteps
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
        internal static partial class GherXunitSteps
        {
            /// <summary>
            /// Just output the steps in the scenario.
            /// </summary>
            /// <param name="feature">Feature</param>
            /// <param name="steps">Steps executed</param>
            public static void NonExecutable(this IGherXunit feature, string? steps = null)
            {
                if (steps is not null) feature.Output.WriteLine(steps.Highlights());
            }
        
            /// <summary>
            /// Just output the steps in the scenario.
            /// </summary>
            /// <param name="feature">Feature</param>
            /// <param name="steps">Steps executed</param>    
            public static Task NonExecutableAsync(this IGherXunit feature, string? steps = null)
            {
                if (steps is not null) feature.Output.WriteLine(steps.Highlights());
                return Task.CompletedTask;
            }
            
            private static void Execute(this IGherXunit feature, MethodInfo? method, string steps, params object?[] param)
            {
                try
                {
                    method?.Invoke(feature, param);
                    feature.Output.WriteLine(steps.Highlights());
                }
                catch (Exception)
                {
                    feature.Output.WriteLine(steps.Highlights(true));
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
        
                    feature.Output.WriteLine(steps.Highlights());
                }
                catch (Exception)
                {
                    feature.Output.WriteLine(steps.Highlights(true));
                    throw;
                }
            }
        
            private static string Highlights(this string steps, bool isException = false)
            {
                var color = isException ? "\u001b[31m" : "\u001b[38;5;82m";
        
                var replacements = new Dictionary<string, string>
                {
                    { "Given", $"{color}GIVEN\u001b[0m" },
                    { "When", $"{color}WHEN\u001b[0m" },
                    { "Then", $"{color}THEN\u001b[0m" },
                    { "And", $"{color}AND\u001b[0m" },
                    { "<<", $"\u001b[35m" },
                    { ">>", $"\u001b[0m" }
                };
        
                var sb = new StringBuilder(steps);
                foreach (var replacement in replacements)
                {
                    sb.Replace(replacement.Key, replacement.Value);
                }
        
                return sb.ToString();
            }
        }
        """;
}