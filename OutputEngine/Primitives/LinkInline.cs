// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.


namespace OutputEngine.Primitives;
// TODO: "Inline" might be a good part of this name
public class LinkInline(string text, string link)
    : InlineElement
{
    public string Link { get; } = link;

    public string Text { get; } = text;
}
