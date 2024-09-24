// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Spectre.Console;

namespace OutputEngine.Renderers;

public class SpectreStyles : OutputStyles
{
    protected override (string? open, string? close) Important => ("[bold]", "[/]");
    protected override (string? open, string? close) InlineCode => ("[bold]", "[/]");
    protected override (string? open, string? close) Error => ("[bold red]", "[/]");
    protected override (string? open, string? close) Warning => ("[bold olive]", "[/]");

    private readonly Dictionary<string, Style?> spectreStyles = new()
    {
        ["Important"] = new Style(decoration: Decoration.Bold),
        ["SectionTitle"] = new Style(foreground: Color.Olive, decoration: Decoration.Bold),
        ["InlineCode"] = new Style(decoration: Decoration.Bold),
        ["Error"] = new Style(foreground: Color.Red, decoration: Decoration.Bold),
        ["Warning"] = new Style(foreground: Color.Olive, decoration: Decoration.Bold)
    };

    internal Style? GetSpectreStyle(string appearance)
    {
        if (spectreStyles.TryGetValue(appearance, out var style))
        {
            return style;
        }
        return null;
    }
}

