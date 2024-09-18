namespace OutputEngine.Targets;

public abstract class GitHubMarkdownStyles : MarkdownStyles
{
    public GitHubMarkdownStyles()
    {
        // TODO: Code blocks will present special challenges because they
        //       involve multiple paragraphs, unless we redefine code, like
        //       we do tables.
        // TODO: Nested lists will also be a challenge.
        // TODO: Horizontal rules may be a variation on Element
        // TODO: Link, footnotes, code, and images may be a variation on TextPart
        Styles = new Dictionary<string, (string? open, string? close)>
        {
            ["Subscript"] = (null, null),
            ["Superscript"] = (null, null),
            ["Highlight"] = (null, null),
        };
    }
}
