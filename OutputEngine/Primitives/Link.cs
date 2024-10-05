// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace OutputEngine.Primitives;

/// <summary>
/// 
/// </summary>
/// <param name="text"><inheritdoc/></param>
/// <param name="uri">The target of the link.</param>
/// <param name="whitespace"><inheritdoc/>/></param>
public class Link(string text, Uri uri, SurroundingWhitespace whitespace = SurroundingWhitespace.BeforeAndAfter)
    : InlineElement(text, Styles.CreateWithImplicit(InlineStyle.LinkText), whitespace)
{
    /// <summary>
    /// The target of the link.
    /// </summary>
    public Uri Uri { get; set; } = uri;

}
