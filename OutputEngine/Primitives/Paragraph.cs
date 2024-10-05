// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace OutputEngine.Primitives;

/// <summary>
/// Represents a paragraph of text.
/// </summary>
public class Paragraph : TextContainer
{
    private readonly List<TextPart> parts = [];

    /// <summary>
    /// Creates a new paragraph with the given text parts.
    /// </summary>
    /// <param name="parts">A set of <see cref="TextPart"> instances, each of which may have a different style.</param>
    public Paragraph(params TextPart[] parts)
        : base(Styles.Empty, parts)
    { }

    /// <summary>
    /// Creates a new paragraph with the given text.
    /// </summary>
    /// <param name="text">The text that comprises the paragraph.</param>
    public Paragraph(string text)
        : base(Styles.Empty, text)
    { }

    /// <summary>
    /// Creates a new paragraph with no text.
    /// </summary>
    /// <remarks>
    /// This resolves an ambiguity.
    /// </remarks>
    public Paragraph()
        : base(Styles.Empty, Array.Empty<TextPart>())
    { }
}

