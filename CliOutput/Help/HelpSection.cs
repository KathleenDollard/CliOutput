// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using OutputEngine.Primitives;

namespace CliOutput;

public abstract class HelpSection : Section
{
    protected HelpSection(string title, CliCommand command)
        : base(title) 
        => Command = command;

    public CliCommand Command { get; }
}
