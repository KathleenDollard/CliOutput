// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections;
using System.Text;

namespace OutputEngine.Primitives;

/// <summary>
/// The base class for things that hold <see cref="TextPart"/>s, that is things that contain text that can be individually formatted by the CLI author."/>
/// </summary>
public abstract class TextContainer : BlockElement, IEnumerable<TextPart>
{
    private readonly List<TextPart> parts = [];

    /// <summary>
    /// Creates a new text container with the given text parts.
    /// </summary>
    /// <param name="implicitStyles"><inheritdoc/>/></param>
    /// <param name="parts">The individual <see cref="TextPart"/>s held in the <see cref="TextContainer"/></param>
    protected TextContainer(Styles implicitStyles, params TextPart[] parts)
        : base(implicitStyles)
    {
        this.parts.AddRange(parts);
    }

    /// <summary>
    /// Creates a new text container with the given text.
    /// </summary>
    /// <param name="implicitStyles"><inheritdoc/>/></param>
    /// <param name="text">The text to hold.</param>
    protected TextContainer(Styles implicitStyles, string text)
        : this(implicitStyles, new TextPart(text))
    { }

    /// <summary>
    /// A new line does not appear after this container when this is true. Not all renderers respect this value.
    /// </summary>
    // TODO: Move this to paragraph. Can we do without this because it does not make sense in HTML - at least change name to NoPaddingBelow. This would be better to handle via a style.
    public bool NoNewLineAfter { get; set; }

    // TODO: Make these 2 into helper methods, possibly part of the wrapping code.
    // TODO: Document the rest of this file.
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

    public void Add(IEnumerable<TextPart> parts)
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

