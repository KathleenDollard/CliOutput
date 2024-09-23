using System;
using System.ComponentModel;

namespace OutputEngine.Renderers;

public class TerminalStyles : OutputStyles
{
    protected override (string? open, string? close) Important => ("**", "**");
    protected override (string? open, string? close) InlineCode => ("`", "`");
    protected override (string? open, string? close) Error => ("**<span style = 'color: Red ;'>", "</span>**");
    protected override (string? open, string? close) Warning => ("**<span style = 'color: Yellow ;'>", "</span>**");
}

