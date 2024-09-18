namespace OutputEngine;

public abstract class OutputStyles
{
    protected Dictionary<string, (string? open, string? close)> Styles
    { get; set; } = new()
    {
        ["Important"] = ("[Bold]", "[Bold]"),
        ["Error"] = ("[Important][Red]", "[Important][Red]"),
        ["Code"] = ("'", "'"),
        ["BulletedItem"] = ("-", null),
    };
    protected string DocumentOpen { get; set; } = string.Empty;
    protected string DocumentClose { get; set; } = string.Empty;
}

