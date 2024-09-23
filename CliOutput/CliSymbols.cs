// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace CliOutput;

public class CliSymbol(string name, string? description)
{
    public string Name { get; set; } = name;
    public string? Description { get; set; } = description;
}

public class CliCommand(string name, string? description)
    : CliSymbol(name, description)
{
    public List<CliArgument> Arguments { get; } = [];
    public List<CliOption> Options { get; } = [];
    private readonly List<CliCommand> subCommands = [];
    public IEnumerable<CliCommand> SubCommands => subCommands;
    public void AddSubCommand(CliCommand command)
    {
        command.Parent = this;
        subCommands.Add(command);
    }
    public CliCommand? Parent { get; private set; }
    public IEnumerable<CliCommand> Ancestors => GetAncestors(Parent);
    private static IEnumerable<CliCommand> GetAncestors(CliCommand? command)
        => command is null
            ? []
            : command.Parent is null
                ? [command]
                : [command, .. GetAncestors(command.Parent)];
}

public class CliOption(string name, string[]? aliases = null, string? description = null)
    : CliSymbol(name, description)
{
    public string[]? Aliases { get; } = aliases;
}

public class CliArgument(string name, string? description)
    : CliSymbol(name, description)
{
    public string DisplayName => Name.ToUpper();
}