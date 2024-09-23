namespace OutputEngine.Primitives;
// TODO: "Inline" might be a good part of this name
public class TextPart(string text, string? appearance = null, Whitespace whitespace = Whitespace.BeforeAndAfter) 
    : InlineElement
{
    public static implicit operator string(TextPart textPart) => textPart.Text;

    public Uri? Link { get; set; }

    public string Text { get; } = text;
    public Whitespace Whitespace { get; } = whitespace;
    public string? Appearance { get; } = appearance;
}
