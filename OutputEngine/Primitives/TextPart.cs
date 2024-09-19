namespace OutputEngine.Primitives;
// TODO: "Inline" might be a good part of this name
public struct TextPart
{
    public static implicit operator string(TextPart textPart) => textPart.Text;

    public Uri? Link { get; set; }

    public string Text { get; }
    public Whitespace Whitespace { get; }
    public string? Appearance { get; }

    public TextPart(string text, string? appearance = null, Whitespace whitespace = Whitespace.BeforeAndAfter)
    {
        Text = text;
        Appearance = appearance;
        Whitespace = whitespace;
    }

    //public void AddFootnote(Footnote footnote)
    //{
    //    this.Footnote ??= new List<Footnote>();
    //    this.Footnote.Add(footnote);
    //}
}
