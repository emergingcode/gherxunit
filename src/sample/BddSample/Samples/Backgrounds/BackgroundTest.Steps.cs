using FluentAssertions;
using GherXunit.Annotations;
using Xunit.Abstractions;

namespace BddTests.Samples.Backgrounds;

public partial class BackgroundTest : IGherXunitBackground<BackgroundContext>
{
    public ITestOutputHelper Output { get; }
    private BackgroundContext Context { get; }

    public BackgroundTest(ITestOutputHelper output, BackgroundContext context)
    {
        Output = output;
        Context = context;
        Output.WriteLine("CONTEXT {0}", Context.ContextId);
        Output.WriteLine("");
        Setup();
        Output.WriteLine("");
    }
    
    private async Task DrBillPostToBlogStep()
    {
        var result = await PostToBlog("Dr. Bill", "Expensive Therapy");
        result.Should().Be("Your article was published.");
    }
    
    private async Task DrBillPostToBlogFailStep()
    {
        var result = await PostToBlog("Dr. Bill", "Greg's anti-tax rants");
        result.Should().Be("Hey! That's not your blog!");
    }

    private async Task GregPostToBlogStep()
    {
        var result = await PostToBlog("Greg", "Greg's anti-tax rants");
        result.Should().Be("Your article was published.");
    }

    private async Task<string> PostToBlog(string owner, string blog)
    {
        if (!Context.OwnersAndBlogs.TryGetValue(owner, out string? value) || value != blog)
            return "Hey! That's not your blog!";
        
        await Task.Yield();
        return "Your article was published.";
    }
}