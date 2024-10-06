// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace OutputEngine.Primitives;

public class CodeInline(string text)
    : InlineElement(text, SurroundingWhitespace.BeforeAndAfter)
{
    public string? Language { get; set; }
}
