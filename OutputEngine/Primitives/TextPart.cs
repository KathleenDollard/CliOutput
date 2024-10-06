// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace OutputEngine.Primitives;

// TODO: Remove and use InLineElement (remove abstract)
public class TextPart : InlineElement
{
    public TextPart(string text, string? style = null, SurroundingWhitespace whitespace = SurroundingWhitespace.BeforeAndAfter) 
        : base(text,  whitespace)
    {
        if (style is not null)
        {
            AddStyle(style);
        }
    }

    public static implicit operator string(TextPart textPart) => textPart.Text;
}
