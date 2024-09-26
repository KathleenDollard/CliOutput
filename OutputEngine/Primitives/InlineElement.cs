// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace OutputEngine.Primitives;

public abstract class InlineElement(string text, string? style , Whitespace whitespace)
    : Element
{
    public string Text { get; } = text;
    public Whitespace Whitespace { get; } = whitespace;
    public string? Style { get; } = style;
}