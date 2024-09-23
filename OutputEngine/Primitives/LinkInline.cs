
namespace OutputEngine.Primitives;
// TODO: "Inline" might be a good part of this name
public class LinkInline(string text, string link)
    : InlineElement
{
    public string Link { get; } = link;

    public string Text { get; } = text;
}
