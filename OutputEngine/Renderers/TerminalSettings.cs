// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text;

namespace OutputEngine.Renderers;

public class TerminalSettings : RendererSettings
{
    public TerminalSettings(OutputStyles? outputStyles, WriterForTests redirectingWriter, int indentSize, int width, Encoding encoding)
        : base(redirectingWriter)
    {
        IndentSize = indentSize;
        Width = width;
        OutputStyles = outputStyles;
        Encoding = encoding;
    }

    public OutputStyles? OutputStyles { get; }
    public Encoding Encoding { get; }
    public int IndentSize { get;  }
    public int Width { get;  }
}
