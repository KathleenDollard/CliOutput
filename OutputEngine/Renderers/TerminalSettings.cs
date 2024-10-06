// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text;

namespace OutputEngine.Renderers;

public class TerminalSettings(bool shouldRedirect, CliWriter writer) 
    : RendererSettings(shouldRedirect, writer)
{
    public OutputStyles? OutputStyles { get; set; } = null;
    public Encoding Encoding { get; set; } = Encoding.UTF8;
    public int IndentSize { get; set; } = indentSize;
    public int Width { get; set; } = width;
}
