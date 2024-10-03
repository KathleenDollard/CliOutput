// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace OutputEngine.Primitives;

// TODO: Pass style to base
public abstract class InlineElement(string text, Styles styles, SurroundingWhitespace whitespace)
    : Element(styles)
{
    public string Text { get; } = text;
    public SurroundingWhitespace Whitespace { get; } = whitespace;
}