namespace OutputEngine.Primitives;

/// <summary>
/// Standard text appearances for <see cref="Paragraph"/>, inspired by markdown - 
/// few will have meaning in plain text.
/// </summary>
public static class ParagraphAppearance
{
    public const string Normal = "NormalParagraph";
    public const string Warning = "Warning";
    public const string Error = "Error";
    public const string Heading1 = "Heading1";
    public const string Heading2 = "Heading2";
    public const string Heading3 = "Heading3";
    public const string Heading4 = "Heading4";
    public const string Heading5 = "Heading5";
    public const string Heading6 = "Heading6";
    // Consider making block quotes elements
    public const string BlockQuote = "BlockQuote";
    public const string BlockQuoteDoubled = "BlockQuoteDoubled";
    public const string BlockQuoteTripled = "BlockQuoteTripled";
    public const string NumberedList = "NumberedItem";
    public const string BulletedList = "BulletedItem";
    public const string DefinitionList = "DefinitionList";
    public const string TaskItemUnchecked = "TaskItemUnchecked";
    public const string TaskItemChecked = "TaskItemChecked";
}

/// <summary>
/// Standard text appearances for <see cref="TextPart"/>, inspired by markdown - 
/// few will have meaning in plain text.
/// </summary>
public static class TextPartAppearance
{
    public const string Normal = "NormalText";
    public const string LessImportant = "LessImportant";
    public const string Important = "Important";
    public const string Code = "Code";
}

/// <summary>
/// Sets a specific style. 
/// </summary>
/// <remarks>
/// This can undermine semantic text appearance,
/// so should be used with caution.
/// </remarks>
public struct CustomTextAppearance
{
    public string CustomStyle { get; set; }
    public Color Color { get; set; }
}

public static class CustomTextAppearanceStyle
{
    public const string Italic = "Italic";
    public const string Bold = "Bold";
    public const string Underline = "Underline";
    public const string Strikethrough = "Strikethrough";
    public const string Dim = "Dim";
    public const string Subscript = "Subscript";
    public const string Superscript = "Superscript";
    public const string Highlight = "Highlight";
}

// TODO: Figure out color. (not in a separate file because not clear we need an enum)
/// <summary>
/// 
/// </summary>
/// <remarks>
/// Color is challenging because output may be in the ANSI set where "red" is a position in the 
/// theme, not an actual color, or a display that can handle RGB. Interesting discussion https://stackoverflow.com/questions/4842424/list-of-ansi-color-escape-sequences
/// </remarks>
public enum Color
{
    Black,
    Red,
    Green,
    Yellow,
    Blue,
    Magenta,
    Cyan,
    White,
    Default
}
