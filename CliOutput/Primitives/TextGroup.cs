using System.Runtime.CompilerServices;
using System.Text;

namespace CliOutput.Primitives;

public class TextGroup : List<TextPart>
{
    public static implicit operator TextGroup(TextPart textPart) => [textPart];
    public static implicit operator string(TextGroup textGroup) => textGroup.PlainOutput().JoinLines();
    public static explicit operator TextGroup(string s) => [new TextPart(s)];

    public bool IsParagraph { get; set; }

    public static TextGroup TextLine => new() { IsParagraph = true };

    public int PlainWidth(int trialWidth) => PlainOutput(trialWidth).Max(s=>s.Length);

    public IEnumerable<string> PlainOutput(int outputWidth = int.MaxValue)
    {
        var sb = new StringBuilder();
        var first = this.First();
        var last = this.Last();
        foreach (var part in this)
        {
            if (part != first && part.Whitespace.HasFlag(Whitespace.Before))
            {
                sb.Append(' ');
            }
            sb.Append(part.Text);
            if (part != last && part.Whitespace.HasFlag(Whitespace.After))
            {
                sb.Append(' ');
            }
        }

        return sb.ToString().Wrap(outputWidth);
    }

}

