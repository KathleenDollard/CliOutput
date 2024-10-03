// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections;
using System.Text;

namespace OutputEngine.Primitives;

// TODO: Create a common base class for things that hold TextPart
public abstract class TextContainer : BlockElement, IEnumerable<TextPart>
{
    private readonly List<TextPart> parts = [];

    public TextContainer(Styles styles, params TextPart[] parts)
        : base(styles)
    {
        this.parts.AddRange(parts);
    }

    public TextContainer(Styles styles, params string[] parts)
        : this(styles, parts.Select(s => new TextPart(s)).ToArray())
    { }

    public TextContainer(Styles styles)
        : this(styles, Array.Empty<TextPart>())
    { }

    public bool NoNewLineAfter { get; set; }

    public int PlainWidth(int trialWidth)
    {
        IEnumerable<string> strings = PlainOutput(trialWidth);
        return strings.Max(s => s.Length);
    }

    public IEnumerable<string> PlainOutput(int outputWidth = int.MaxValue)
    {
        var sb = new StringBuilder();
        var first = this.First();
        var last = this.Last();
        foreach (var part in this)
        {
            if (part != first && part.Whitespace.HasFlag(SurroundingWhitespace.Before))
            {
                sb.Append(' ');
            }
            sb.Append(part.Text);
            if (part != last && part.Whitespace.HasFlag(SurroundingWhitespace.After))
            {
                sb.Append(' ');
            }
        }

        return sb.ToString().Wrap(outputWidth);
    }

    public void Add(TextPart part)
        => this.parts.Add(part);

    public void AddRange(IEnumerable<TextPart> parts)
        => this.parts.AddRange(parts);

    public TextPart this[int index]
        => parts[index];

    public IEnumerable<TextPart> Slice(int start, int length)
        => parts[new Range(start, length)];

    public int Count()
        => parts.Count;

    public int Length
    => Count();

    public IEnumerator<TextPart> GetEnumerator()
        => ((IEnumerable<TextPart>)this.parts).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => ((IEnumerable)this.parts).GetEnumerator();

    public override string ToString()
    { return PlainOutput().JoinLines(); }
}

