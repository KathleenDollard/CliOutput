using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CliOutput.Primitives;

public readonly struct TextPart(string text, TextAppearance? appearance = null,Whitespace whitespace = Whitespace.BeforeAndAfter)
{
    public static implicit operator string(TextPart textPart) => textPart.Text;
    public static explicit operator TextPart(string s) => new(s);

    public string Text { get; } = text;
    public Whitespace Whitespace { get; } = whitespace;
    public TextAppearance Appearance { get; } = appearance ?? new TextAppearance();

}
