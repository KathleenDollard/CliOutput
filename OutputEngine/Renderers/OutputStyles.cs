// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace OutputEngine.Renderers;

public abstract class OutputStyles
{
    protected Dictionary<string, (string? open, string? close)> StyleCodes { get; set; } = [];

    public OutputStyles()
    {
        AddStyle(InlineStyle.Normal, Normal);
        AddStyle(InlineStyle.Important, Important);
        AddStyle(InlineStyle.SlightlyImportant, SlightlyImportant);
        AddStyle(InlineStyle.CodeInline, CodeInline);
        AddStyle(InlineStyle.Optional, Optional);
        AddStyle(InlineStyle.Argument, Argument);
        AddStyle(InlineStyle.LinkText, LinkText);

        AddStyle(BlockStyle.SectionHeading, SectionHeading);
        AddStyle(BlockStyle.CodeBlock, CodeBlock);
        AddStyle(BlockStyle.Quote, Quote);
        AddStyle(BlockStyle.Heading1, Heading1);
        AddStyle(BlockStyle.Heading2, Heading2);
        AddStyle(BlockStyle.Heading3, Heading3);
        AddStyle(BlockStyle.Error, Error);
        AddStyle(BlockStyle.Warning, Warning);
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
        StyleCodes[name] = style;
    }

    protected void AddStyle(string name, string? open, string? close)
    {
        StyleCodes[name] = (open, close);
    }

    public virtual (string open, string close) GetStyleCodes(Styles? styles)
    {
        if (styles is null)
        {
            return ("", "");
        }
        (string? open, string? close) = (null, null);
        foreach (var style in styles)
        {
            if (style is null)
            {
                continue;
            }
            if (StyleCodes.TryGetValue(style, out var codes))
            {
                open = AppendIfNotNull(open, codes.open);
                close = PrependIfNotNull(close, codes.close);
            }
        }

        return (open ?? "", close ?? "");

    }

    protected static string? AppendIfNotNull(string? currentCode, string? newCode)
        => newCode is null
            ? currentCode  // whether or not it is null
            : currentCode is null
                ? newCode
                : $"{currentCode}{newCode}";

    protected static string? PrependIfNotNull(string? currentCode, string? newCode)
        => newCode is null
            ? currentCode  // whether or not it is null
            : currentCode is null
                ? newCode
                : $"{newCode}{currentCode}";

    protected string DocumentOpen { get; set; } = string.Empty;
    protected string DocumentClose { get; set; } = string.Empty;
}

