// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace OutputEngine;

public abstract class OutputStyles
{
    protected Dictionary<string, (string? open, string? close)> Styles { get; set; } = [];

    public OutputStyles()
    {
        AddStyle(TextStyle.Normal, Normal);
        AddStyle(TextStyle.Important, Important);
        AddStyle(TextStyle.SlightlyImportant, SlightlyImportant);
        AddStyle(TextStyle.CodeInline, CodeInline);
        AddStyle(TextStyle.Optional, Optional);
        AddStyle(TextStyle.Argument, Argument);
        AddStyle(TextStyle.LinkText, LinkText);


        AddStyle(ParagraphStyle.SectionHeading, SectionHeading);
        AddStyle(ParagraphStyle.CodeBlock, CodeBlock);
        AddStyle(ParagraphStyle.Quote, Quote);
        AddStyle(ParagraphStyle.Heading1, Heading1);
        AddStyle(ParagraphStyle.Heading2, Heading2);
        AddStyle(ParagraphStyle.Heading3, Heading3);
        AddStyle(ParagraphStyle.Error, Error);
        AddStyle(ParagraphStyle.Warning, Warning);
    }

    protected virtual (string? open, string? close) Normal { get; }
    protected abstract (string? open, string? close) Important { get; }
    protected abstract (string? open, string? close) SlightlyImportant { get; }
    protected abstract (string? open, string? close) CodeInline { get; }
    protected virtual (string? open, string? close) Argument => ("<", ">");
    protected virtual (string? open, string? close) Optional => ("[", "]");
    protected abstract (string? open, string? close) LinkText { get; }

    protected abstract (string? open, string? close) SectionHeading { get; }
    protected abstract (string? open, string? close) CodeBlock { get; }
    protected abstract (string? open, string? close) Quote { get; }
    protected abstract (string? open, string? close) Heading1 { get; }
    protected abstract (string? open, string? close) Heading2 { get; }
    protected abstract (string? open, string? close) Heading3 { get; }
    protected abstract (string? open, string? close) Error { get; }
    protected abstract (string? open, string? close) Warning { get; }

    protected void AddStyle(string name, (string?, string?) style)
    {
        Styles[name] = style;
    }

    protected void AddStyle(string name, string? open, string? close)
    {
        Styles[name] = (open, close);
    }

    public (string? open, string? close) GetStyle(string? name)
        => name is null
            ? (null, null)
            : Styles.TryGetValue(name, out var codes)
                ? (codes.open, codes.close)
                : (null, null);

    protected string DocumentOpen { get; set; } = string.Empty;
    protected string DocumentClose { get; set; } = string.Empty;
}

