// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using OutputEngine.Renderers;
using System.Text;

namespace OutputEngine;

public class OutputContext(bool shouldRedirect = false,
                     int width = 80,
                     int indentSize = 2,
                     CliWriter? writer = null)
{
    public bool ShouldRedirect { get; set; } = shouldRedirect;


    public OutputStyles? OutputStyles { get; set; } = null;
    public Encoding Encoding { get; set; } = Encoding.UTF8;
    public int IndentSize { get; set; } = indentSize;
    public CliWriter? Writer { get; } = writer;
    public int Width { get; set; } = width;
}
