using System;
using System.ComponentModel;

namespace OutputEngine.Targets;

public class MarkdownStyles : OutputStyles
{
    public MarkdownStyles()
    {
        // TODO: Code blocks will present special challenges because they
        //       involve multiple paragraphs, unless we redefine code, like
        //       we do tables.
        // TODO: Nested lists will also be a challenge.
        // TODO: Horizontal rules, definition list, code blocks and images may be a variation on Element
        // TODO: Links and footnotes may be a variation on TextPart

        // Several items are inherited from OutputStyles
        // Paragraph styles
        AddStyle("NormalParagraph", null, null);
        AddStyle("Heading1", "#", null);
        AddStyle("Heading2", "##", null);
        AddStyle("Heading3", "###", null);
        AddStyle("Heading4", "####", null);
        AddStyle("Heading5", "#####", null);
        AddStyle("Heading6", "######", null);
        AddStyle("BlockQuote", ">", null);
        AddStyle("BlockQuoteDoubled", ">>", null);
        AddStyle("BlockQuoteTripled", ">>>", null);
        AddStyle("NumberedItem", "1", null);
        AddStyle("TaskItemUnchecked", "[ ]", null);
        AddStyle("TaskItemChecked", "[x]", null);
        AddStyle("Important", "**", "**");
        AddStyle("Error", "**<span style = 'color: Red ;'>", "</span>**");
        AddStyle("Code", "'", "'");
        AddStyle("BulletedItem", "-", null);


        // TextPart styles
        AddStyle("NormalText", null, null);
        AddStyle("LessImportant", null, null);
        AddStyle("Code", "`", "`");
        AddStyle("Black", "<span style = 'color: Black ;'>", "</span>");
        AddStyle("Red", "<span style = 'color: Red ;'>", "</span>");
        AddStyle("Green", "<span style = 'color: Green ;'>", "</span>");
        AddStyle("Yellow", "<span style = 'color: Yellow ;'>", "</span>");
        AddStyle("Blue", "<span style = 'color: Blue ;'>", "</span>");
        AddStyle("Magenta", "<span style = 'color: Magenta ;'>", "</span>");
        AddStyle("Cyan", "<span style = 'color: Cyan ;'>", "</span>");
        AddStyle("White", "<span style = 'color: White ;'>", "</span>");
        AddStyle("Default", "<span style = 'color: Default ;'>", "</span>");
        AddStyle("Italic", "_", "_");
        AddStyle("Bold", "**", "**");
        AddStyle("Strikethrough", "~~", "~~");
        AddStyle("Subscript", "~", "~");
        AddStyle("Superscript", "^", "^");
        AddStyle("Highlight", "==", "==");


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

    protected override (string? open, string? close) Important => ("**", "**");
    protected override (string? open, string? close) InlineCode => ("`", "`");
    protected override (string? open, string? close) Error => ("**<span style = 'color: Red ;'>", "</span>**");
    protected override (string? open, string? close) Warning => ("**<span style = 'color: Yellow ;'>", "</span>**");
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

