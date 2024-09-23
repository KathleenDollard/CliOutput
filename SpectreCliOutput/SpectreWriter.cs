// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using OutputEngine;
using OutputEngine.Renderers;
using Spectre.Console;
using Spectre.Console.Advanced;
using Spectre.Console.Rendering;
using Spectre.Console.Testing;


namespace SpectreCliOutput;

public class SpectreWriter : CliWriter
{
    private readonly IAnsiConsole Console;
    public SpectreWriter(OutputContext outputContext) : base(outputContext)
    {
        Console = Redirecting
            ? new TestConsole()
                .Colors(ColorSystem.Standard)
                .EmitAnsiSequences()
            : AnsiConsole.Create(new AnsiConsoleSettings());
    }

    public override string? GetBuffer() 
        => Console is TestConsole testConsole
            ? testConsole.Output
            : null;

    public override void ClearBuffer()
    {
        if (this.Console is TestConsole testConsole)
        {
            testConsole.Clear();
        }
    }

    public override void Write(string? text)
    {
        if (text is not null)
        {
            Console.Markup(text);
        }
    }

    //public void Write(Markup markup)
    //{
    //    if (markup is not null)
    //    {
    //        var s = Console.ToAnsi(markup); 
    //        Console.Write(s);
    //    }
    //}

    //public void Write(Table table)
    //{
    //    if (table is not null)
    //    {
    //        var s = Console.ToAnsi(table);
    //        Console.Write(s);
    //    }
    //}

    public void Write(IRenderable toRender)
    {
        if (toRender is not null)
        {
            var s = Console.ToAnsi(toRender);
            Console.Write(s);
        }
    }

}
