using System.Runtime.CompilerServices;
using GherXunit.Annotations;
using Xunit.Abstractions;

namespace BddTests.Samples.Backgrounds;

public partial class BackgroundTest(ITestOutputHelper output) : IGherXunit, IAsyncLifetime
{
    public ITestOutputHelper Output { get; } = output;
    private BackgroundContext? Context { get; set; }

    private Task Setup()
    {
        Context = new BackgroundContext();
        return Task.CompletedTask;
    }
    
    private async Task DrBillPostToBlogStep()
    {
        var result = await PostToBlog("Dr. Bill", "Expensive Therapy");
        Assert.Equal("Your article was published.", result);
    }
    
    private async Task DrBillPostToBlogFailStep()
    {
        var result = await PostToBlog("Dr. Bill", "Greg's anti-tax rants");
        Assert.Equal("Hey! That's not your blog!", result);
    }

    private async Task GregPostToBlogStep()
    {
        var result = await PostToBlog("Greg", "Greg's anti-tax rants");
        Assert.Equal("Your article was published.", result);
    }

    private async Task<string> PostToBlog(string owner, string blog)
    {
        if (!Context!.OwnersAndBlogs.TryGetValue(owner, out string? value) || value != blog)
            return "Hey! That's not your blog!";
        
        await Task.Yield();
        return "Your article was published.";
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}