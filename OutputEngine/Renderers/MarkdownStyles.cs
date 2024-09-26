// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace OutputEngine.Renderers;

public class MarkdownStyles : OutputStyles
{
    public MarkdownStyles()
    {
       //// TextPart styles
       // AddStyle("NormalText", null, null);
       // AddStyle("LessImportant", null, null);
       // AddStyle("Code", "`", "`");
       // AddStyle("Black", "<span style = 'color: Black ;'>", "</span>");
       // AddStyle("Red", "<span style = 'color: Red ;'>", "</span>");
       // AddStyle("Green", "<span style = 'color: Green ;'>", "</span>");
       // AddStyle("Yellow", "<span style = 'color: Yellow ;'>", "</span>");
       // AddStyle("Blue", "<span style = 'color: Blue ;'>", "</span>");
       // AddStyle("Magenta", "<span style = 'color: Magenta ;'>", "</span>");
       // AddStyle("Cyan", "<span style = 'color: Cyan ;'>", "</span>");
       // AddStyle("White", "<span style = 'color: White ;'>", "</span>");
       // AddStyle("Default", "<span style = 'color: Default ;'>", "</span>");
       // AddStyle("Italic", "_", "_");
       // AddStyle("Bold", "**", "**");
       // AddStyle("Strikethrough", "~~", "~~");
       // AddStyle("Subscript", "~", "~");
       // AddStyle("Superscript", "^", "^");
       // AddStyle("Highlight", "==", "==");


       // DocumentOpen = """
       //                <style>
       //                    "Black { color: Black }
       //                    "Red { color: Red }
       //                    "Green { color: Green }
       //                    "Yellow { color: Yellow }
       //                    "Blue { color: Blue }
       //                    "Magenta { color: Magenta }
       //                    "Cyan { color: Cyan }
       //                    "White { color: White }
       //                </style>
       //                """;
       // DocumentClose = string.Empty;
    }

    protected override (string? open, string? close) Important => ("**", "**");
    protected override (string? open, string? close) CodeInline => ("`", "`");
    protected override (string? open, string? close) Error => ("**<span style = 'color: Red ;'>", "</span>**");
    protected override (string? open, string? close) Warning => ("**<span style = 'color: Yellow ;'>", "</span>**");

    protected override (string? open, string? close) SlightlyImportant => ("_", "_");
    protected override (string? open, string? close) LinkText => (null, null);
    protected override (string? open, string? close) SectionHeading => ($"## ", ":");
    protected override (string? open, string? close) CodeBlock => ($"```{Environment.NewLine}", $"{Environment.NewLine}```");
    protected override (string? open, string? close) Quote => ($"> ", null);
    protected override (string? open, string? close) Heading1 => ($"# ", null);
    protected override (string? open, string? close) Heading2 => ($"## ", null);
    protected override (string? open, string? close) Heading3 => ($"### ", null);
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

