// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text;

namespace OutputEngine.Renderers;

/// <summary>
/// Base constructor for writers
/// </summary>
/// <param name="outputContext"></param>
/// <remarks>
/// Implementing classes should set an appropriate default 
/// OutputStyles if it is null.
/// </remarks>
public class CliWriter(OutputContext outputContext) : TextWriter
{
    public override Encoding Encoding { get; } = outputContext.Encoding;
    public bool Redirecting { get; } = outputContext.ShouldRedirect;

    private readonly StringBuilder buffer = new();
    public virtual string? GetBuffer() 
        => Redirecting
            ? buffer.ToString()
            : null;
    public virtual void ClearBuffer() => buffer.Clear();

    public virtual void WriteLine<T>(T? output)
    {
        if (output is not null)
        {
            Write(output);
        }
        // This needs to call the parameterless form because
        // a line break is not be Environment.NewLine in HTML
        // or markdown
        WriteLine();
    }
    public override void WriteLine()
    {
        Write(Environment.NewLine);
    }

    public override void Write(string? text)
    {
        if (Redirecting)
        {
            buffer.Append(text);
        }
        else
        {
            Console.Write(text);
        }
    }

}
