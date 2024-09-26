// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace OutputEngine.Primitives;
// TODO: "Inline" might be a good part of this name
public class CodeInline(string text, string? style = null, Whitespace whitespace = Whitespace.BeforeAndAfter) 
    : InlineElement(text, style, whitespace)
{
    public Uri? Link { get; set; }
}
