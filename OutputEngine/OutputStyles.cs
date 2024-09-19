using System.Security;

namespace OutputEngine;

public abstract class OutputStyles
{
    protected Dictionary<string, (string? open, string? close)> Styles
    { get; set; } = new()
    {
        //["Important"] = ("[Bold]", "[Bold]"),
        //["Error"] = ("[Important][Red]", "[Important][Red]"),
        //["Code"] = ("'", "'"),
        //["BulletedItem"] = ("-", null),
    };

    protected void AddStyle(string name, string? open, string? close)
    {
        Styles[name] = (open, close);
    }

    public (string? open, string? close) GetStyle(string? name)
        => name is null
            ? (null, null)
            : Styles.TryGetValue(name, out var codes)
                ? (codes.open, codes.close)
                : (null, null);

    protected string DocumentOpen { get; set; } = string.Empty;
    protected string DocumentClose { get; set; } = string.Empty;
}

