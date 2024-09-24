// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using OutputEngine.Primitives;

namespace CliOutput.Help;

// # Scenarios for extended help

// ## More help
//
// We have information beyond the description that can contribute to help,
// such as choices and ranges. Possible ways to incorporate this. Much more
// information could be added, and for other reasons in help we are likely 
// to have reasonable markup support, so the CLI could be a single souce of 
// truth for simple to moderate CLIs.
// 
// Additional help could be provided by a different syntax:
//
// ```
// dotnet build -hv
// dotnet build -hm
// dotnet build -hvvvv
// dotnet build -h with-condition
// dotnet build -h options
// dotnet build -h no-description
// ```
//
// An alternative approach
//
// ## Targeted help
//
// Sometimes help




//// -s is --search
//// dotnet new blazorwasm -h -s auth
//// dotnet new blazorwasm -h --details pwa
//// Or instead of details, build decent URLs automatically from root. Build Website as well



public class HelpLayout : Layout
{
    public CliCommand Command { get; }

    private HelpLayout(CliCommand command, IEnumerable<HelpSection> sections)
        : base(sections)
    {
        Command = command;
    }

    public static HelpLayout Create(CliCommand command)
    {
        HelpLayout layout = new(command,
                [
                    new HelpDescription(command),
                    new HelpUsage(command)
                ]);

        // TODO: Figure out how examples are stored and how to display code blocks
        // new HelpExamples(command),

        if (command.Arguments.Count > 0)
        {
            layout.Sections.Add(new HelpArguments(command));
        }
        if (command.Options.Count > 0)
        {
            layout.Sections.Add(new HelpOptions(command));
        }
        if (command.SubCommands.Any())
        {
            layout.Sections.Add(new HelpSubcommands(command));
        }
        return layout;
    }
}
