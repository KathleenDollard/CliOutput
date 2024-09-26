////using System.Collections;

//namespace OutputEngine.Primitives;

//// I do not yet know how to do this - this should be a TextPart that
//// appears in one or more places, an ID, plus a paragraph for the actual note.
//// It will mess with PlainOutputWidth and wrapping.
//public class Footnote
//    : Paragraph
//{
//    public Footnote(TextStyle style, params TextPart[] parts)
//        : base(style, parts)
//    { }

//    public Footnote(params TextPart[] parts)
//        : base(parts)
//    { }

//    public Footnote(TextStyle style, params string[] parts)
//        : base(style, parts.Select(s => new TextPart(s)).ToArray())
//    { }

//    public Footnote(params string[] parts)
//        : base(parts.Select(s => new TextPart(s)).ToArray())
//    { }

//}