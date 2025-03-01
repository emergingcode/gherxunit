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
    
    private async Task Step01()
    {
        var result = await PostToBlog("Dr. Bill", "Expensive Therapy");
        result.Should().Be("Your article was published.");
    }
    
    private async Task Step02()
    {
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
                PostToBlog("Dr. Bill", "Greg's anti-tax rants"));
    }

    private async Task Step03()
    {
        var result = await PostToBlog("Greg", "Expensive Therapy");
        result.Should().Be("Your article was published.");
    }

    private async Task<string> PostToBlog(string owner, string blog)
    {
        if (!Context.OwnersAndBlogs.TryGetValue(owner, out string? value) || value != blog)
            throw new InvalidOperationException("Hey! That's not your blog!");
        
        await Task.Yield();
        return "Your article was published.";
    }
}