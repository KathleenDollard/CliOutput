namespace OutputEngine.Primitives;

/// <summary>
/// Includes information on the appearance, including both font variations like bold and color. 
/// </summary>
public struct TextAppearance
{
    public bool Italic { get; set; }
    public bool Bold { get; set; }
    public bool Underline { get; set; }
    public bool Strikethrough { get; set; }
    public bool Dim { get; set; }
    public Color Color { get; set; }

    public static TextAppearance Normal => new();
    public static TextAppearance LessImportant => new() { Dim = true };
    public static TextAppearance Important => new() { Bold = true };
    public static TextAppearance Error => new() { Color = Color.Red };

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
