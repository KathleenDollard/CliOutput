// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Spectre.Console;
using SpectreStyle = Spectre.Console.Style;

namespace OutputEngine.Renderers;

public class SpectreStyles : OutputStyles
{
    protected override (string? open, string? close) Important => ("[bold]", "[/]");
    protected override (string? open, string? close) CodeInline => ("[bold]", "[/]");
    protected override (string? open, string? close) Error => ("[bold red]", "[/]");
    protected override (string? open, string? close) Warning => ("[bold olive]", "[/]");

    protected override (string? open, string? close) SlightlyImportant => (null, null);
    protected override (string? open, string? close) LinkText => (null, null);
    protected override (string? open, string? close) SectionHeading => (null, null);
    protected override (string? open, string? close) CodeBlock => (null, null);
    protected override (string? open, string? close) Quote => (null, null);
    protected override (string? open, string? close) Heading1 => (null, null);
    protected override (string? open, string? close) Heading2 => (null, null);
    protected override (string? open, string? close) Heading3 => (null, null);


    private readonly Dictionary<string, SpectreStyle?> spectreStyles = new()
    {
        ["Important"] = new SpectreStyle(decoration: Decoration.Bold),
        ["SectionTitle"] = new SpectreStyle(foreground: Color.Olive, decoration: Decoration.Bold),
        ["InlineCode"] = new SpectreStyle(decoration: Decoration.Bold),
        ["Error"] = new SpectreStyle(foreground: Color.Red, decoration: Decoration.Bold),
        ["Warning"] = new SpectreStyle(foreground: Color.Olive, decoration: Decoration.Bold)
    };

    internal SpectreStyle? GetSpectreStyle(string style)
    {
        if (spectreStyles.TryGetValue(style, out var spectreStyle))
        {
            return spectreStyle;
        }
        return null;
    }
}

