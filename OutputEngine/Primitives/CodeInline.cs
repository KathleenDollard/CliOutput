// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace OutputEngine.Primitives;
// TODO: "Inline" might be a good part of this name
// TODO: Specify language
public class CodeInline(string text)
    : InlineElement(text, Styles.CreateWithImplicit(InlineStyle.CodeInline), SurroundingWhitespace.BeforeAndAfter)
{ }
