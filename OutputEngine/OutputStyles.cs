using System.Security;

namespace OutputEngine;

public class OutputStyles
{
    protected Dictionary<string, (string? open, string? close)> Styles
    { get; set; } = new();


    public OutputStyles()
    {
        AddStyle("Important", Important);
        AddStyle("SectionTitle", SectionTitle);
        AddStyle("InlineCode", InlineCode);
        AddStyle("AngleBrackets", ("<", ">"));
        AddStyle("SquareBrackets", ("[", "]"));
        AddStyle("SquareAndAngleBrackets", ("[<", ">]"));
    }

    protected virtual (string? open, string? close) Important => (null, null);
    protected virtual (string? open, string? close) SectionTitle => (null, ":");
    protected virtual (string? open, string? close) InlineCode => (null, null);

    protected void AddStyle(string name, (string?, string?) style)
    {
        Styles[name] = style;
    }

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

