// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Security;

namespace OutputEngine;

public abstract class OutputStyles
{
    protected Dictionary<string, (string? open, string? close)> Styles { get; set; } = [];


    public OutputStyles()
    {
        AddStyle("Important", Important);
        AddStyle("SectionTitle", SectionTitle);
        AddStyle("InlineCode", InlineCode);
        AddStyle("AngleBrackets", AngleBrackets);
        AddStyle("SquareBrackets", SquareBrackets);
        AddStyle("SquareAndAngleBrackets", SquareAndAngleBrackets);
        AddStyle("Error", Error);
        AddStyle("Warning", Warning);
    }

    protected abstract (string? open, string? close) InlineCode { get; }
    protected abstract (string? open, string? close) Important { get; }
    protected abstract (string? open, string? close) Error { get; }
    protected abstract (string? open, string? close) Warning { get; }
    protected virtual (string? open, string? close) SectionTitle => (null, ":");
    protected virtual (string? open, string? close) AngleBrackets => ("<", ">");
    protected virtual (string? open, string? close) SquareBrackets => ("[", "]");
    protected virtual (string? open, string? close) SquareAndAngleBrackets => ("[<", ">]");

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

