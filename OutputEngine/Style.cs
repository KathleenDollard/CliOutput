// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using OutputEngine.Primitives;

namespace OutputEngine;

/// <summary>
/// Standard text styles for <see cref="Paragraph"/>, inspired by markdown - 
/// few will have meaning in plain text.
/// </summary>
public static class ParagraphStyle
{
    public const string SectionHeading = "SectionHeading";
    public const string CodeBlock = "CodeBlock";
    public const string Quote = "Quote";
    public const string Heading1 = "Heading1";
    public const string Heading2 = "Heading2";
    public const string Heading3 = "Heading3";
    public const string Error = "Error";
    public const string Warning = "Warning";
}

public static class TextStyle
{
    public const string Normal = "NormalParagraph";
    public const string Important = "Important";
    public const string SlightlyImportant = "LessImportant";
    public const string CodeInline = "CodeInline";
    public const string Argument = "Argument";
    public const string Optional = "Optional";
    public const string LinkText = "LinkText";
}

/// <summary>
/// Sets a specific style. 
/// </summary>
/// <remarks>
/// This can undermine semantic text style,
/// so should be used with caution.
/// </remarks>
public struct CustomTextStyle
{
    public string? CustomStyle { get; set; }
    public string? Color { get; set; }
}

//public static class CustomTextStyleStyle
//{
//    public const string Italic = "Italic";
//    public const string Bold = "Bold";
//    public const string Underline = "Underline";
//    public const string Strikethrough = "Strikethrough";
//    public const string Dim = "Dim";
//    public const string Subscript = "Subscript";
//    public const string Superscript = "Superscript";
//    public const string Highlight = "Highlight";
//}

//// TODO: Figure out color. (not in a separate file because not clear we need an enum)
///// <summary>
///// 
///// </summary>
///// <remarks>
///// Color is challenging because output may be in the ANSI set where "red" is a position in the 
///// theme, not an actual color, or a display that can handle RGB. Interesting discussion https://stackoverflow.com/questions/4842424/list-of-ansi-color-escape-sequences
///// </remarks>
//public enum Color
//{
//    Black,
//    Red,
//    Green,
//    Yellow,
//    Blue,
//    Magenta,
//    Cyan,
//    White,
//    Default
//}
