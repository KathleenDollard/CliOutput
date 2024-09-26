// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace OutputEngine.Renderers;

public class TerminalStyles : OutputStyles
{
    protected override (string? open, string? close) Important => (null, null);
    protected override (string? open, string? close) CodeInline => (null, null);
    protected override (string? open, string? close) Error => (null, null);
    protected override (string? open, string? close) Warning => (null, null);

    protected override (string? open, string? close) SlightlyImportant => (null, null);
    protected override (string? open, string? close) LinkText => (null, null);
    protected override (string? open, string? close) SectionHeading => (null, ":");
    protected override (string? open, string? close) CodeBlock => (null, null);
    protected override (string? open, string? close) Quote => (null, null);
    protected override (string? open, string? close) Heading1 => (null, null);
    protected override (string? open, string? close) Heading2 => (null, null);
    protected override (string? open, string? close) Heading3 => (null, null);
}

