// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using OutputEngine.Primitives;

namespace OutputEngine;

/// <summary>
/// Standard text styles for <see cref="Paragraph"/>, inspired by markdown - 
/// few will have meaning in plain text.
/// </summary>
public static class BlockStyle
{
    public const string SectionHeading = "SectionHeading";
    public const string Section = "Section";
    public const string Table = "Table";
    public const string CodeBlock = "CodeBlock";
    public const string Quote = "Quote";
    public const string Heading1 = "Heading1";
    public const string Heading2 = "Heading2";
    public const string Heading3 = "Heading3";
    public const string Error = "Error";
    public const string Warning = "Warning";
}

