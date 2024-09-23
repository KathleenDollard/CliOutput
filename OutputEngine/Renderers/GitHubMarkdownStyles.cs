namespace OutputEngine.Renderers;

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
        AddStyle("Subscript",null, null);
        AddStyle("Superscript",null, null);
        AddStyle("Highlight", null, null);
    }
}
