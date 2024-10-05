// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace OutputEngine.Primitives;

/// <summary>
/// The base class for all inline primitive elements.
/// </summary>
/// <param name="text">The text for the inline element.</param>
/// <param name="implicitStyles">A style that is prefilled with the implicit type for the specific primitive element class.</param>
/// <param name="whitespace">Whether whitespace before and after the element is ensured.</param>
public abstract class InlineElement(string text, Styles implicitStyles, SurroundingWhitespace whitespace)
    : Element(implicitStyles)
{
    /// <summary>
    /// The text for the inline element.
    /// </summary>
    public string Text { get; } = text;

    /// <summary>
    /// The whitespace before and after the element.
    /// </summary>
    public SurroundingWhitespace Whitespace { get; } = whitespace;
}