namespace BddTests.Samples.Backgrounds;

public class BackgroundContext
{
    public string ContextId { get; } = Guid.NewGuid().ToString();
    public Dictionary<string, string> OwnersAndBlogs { get; } = new();
    
    public BackgroundContext()
    {
        OwnersAndBlogs.Add("Greg", "Greg's anti-tax rants");
        OwnersAndBlogs.Add("Dr. Bill", "Expensive Therapy");
    }    
}