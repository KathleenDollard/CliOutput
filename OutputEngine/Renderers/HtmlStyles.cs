// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

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
    protected override (string? open, string? close) CodeInline => (null, null);
    protected override (string? open, string? close) Error => (null, null);
    protected override (string? open, string? close) Warning => (null, null);

    protected override (string? open, string? close) SlightlyImportant  => (null, null);
    protected override (string? open, string? close) LinkText => (null, null);
    protected override (string? open, string? close) SectionHeading => (null, null);
    protected override (string? open, string? close) CodeBlock => (null, null);
    protected override (string? open, string? close) Quote => (null, null);
    protected override (string? open, string? close) Heading1 => (null, null);
    protected override (string? open, string? close) Heading2 => (null, null);
    protected override (string? open, string? close) Heading3 => (null, null);
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

