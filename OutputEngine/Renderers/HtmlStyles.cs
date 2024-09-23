using System;
using System.ComponentModel;

namespace OutputEngine.Renderers;

public class HtmlStyles : OutputStyles
{
    public HtmlStyles()
    {
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

    protected override (string? open, string? close) Important => (null, null);
    protected override (string? open, string? close) InlineCode => (null, null);
    protected override (string? open, string? close) Error => (null, null);
    protected override (string? open, string? close) Warning => (null, null);
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

