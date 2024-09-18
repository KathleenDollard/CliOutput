using System;
using System.ComponentModel;

namespace OutputEngine.Targets;

public abstract class MarkdownStyles : OutputStyles
{
    public MarkdownStyles()
    {
        // TODO: Code blocks will present special challenges because they
        //       involve multiple paragraphs, unless we redefine code, like
        //       we do tables.
        // TODO: Nested lists will also be a challenge.
        // TODO: Horizontal rules, definition list, code blocks and images may be a variation on Element
        // TODO: Links and footnotes may be a variation on TextPart
        Styles = new Dictionary<string, (string? open, string? close)>
        {
            // Several items are inherited from OutputStyles
            // Paragraph styles
            ["NormalParagraph"] = (null, null),
            ["Heading1"] = ("#", null),
            ["Heading2"] = ("##", null),
            ["Heading3"] = ("###", null),
            ["Heading4"] = ("####", null),
            ["Heading5"] = ("#####", null),
            ["Heading6"] = ("######", null),
            ["BlockQuote"] = (">", null),
            ["BlockQuoteDoubled"] = (">>", null),
            ["BlockQuoteTripled"] = (">>>", null),
            ["NumberedItem"] = ("1", null),
            ["TaskItemUnchecked"] = ("[ ]", null),
            ["TaskItemChecked"] = ("[x]", null),

            // TextPart styles
            ["NormalText"] = (null, null),
            ["LessImportant"] = (null, null),
            ["Code"] = ("`", "`"),
            ["Black"] = ("<span style = 'color: Black ;'>", "</span>"),
            ["Red"] = ("<span style = 'color: Red ;'>", "</span>"),
            ["Green"] = ("<span style = 'color: Green ;'>", "</span>"),
            ["Yellow"] = ("<span style = 'color: Yellow ;'>", "</span>"),
            ["Blue"] = ("<span style = 'color: Blue ;'>", "</span>"),
            ["Magenta"] = ("<span style = 'color: Magenta ;'>", "</span>"),
            ["Cyan"] = ("<span style = 'color: Cyan ;'>", "</span>"),
            ["White"] = ("<span style = 'color: White ;'>", "</span>"),
            ["Default"] = ("<span style = 'color: Default ;'>", "</span>"),
            ["Italic"] = ("_", "_"),
            ["Bold"] = ("**", "**"),
            ["Strikethrough"] = ("~~", "~~"),
            ["Subscript"] = ("~", "~"),
            ["Superscript"] = ("^", "^"),
            ["Highlight"] = ("==", "=="),
        };

        DocumentOpen = """
                       <style>
                           "Black { color: Black }
                           "Red { color: Red }
                           "Green { color: Green }
                           "Yellow { color: Yellow }
                           "Blue { color: Blue }
                           "Magenta { color: Magenta }
                           "Cyan { color: Cyan }
                           "White { color: White }
                       </style>
                       """;
        DocumentClose = string.Empty;
    }
}

// Reference: 
//{ "bold", ("**", "**") },
//{ "italic", ("*", "*") },
//{ "underline", ("__", "__") },
//{ "strikethrough", ("~~", "~~") },
//{ "code", ("`", "`") },
//{ "link", ("[", "]") },
//{ "image", ("![", "]") },
//{ "quote", ("> ", "") },
//{ "list", ("", "") },
//{ "listItem", ("* ", "") },
//{ "table", ("", "") },
//{ "tableRow", ("", "") },
//{ "tableCell", ("", "") },
//{ "heading1", ("# ", "") },
//{ "heading2", ("## ", "") },
//{ "heading3", ("### ", "") },
//{ "heading4", ("#### ", "") },
//{ "heading5", ("##### ", "") },
//{ "heading6", ("###### ", "") },
//{ "horizontalRule", ("---", "") },
//{ "lineBreak", ("", "  ") },
//{ "paragraph", ("", "") },
//{ "text", ("", "") }

