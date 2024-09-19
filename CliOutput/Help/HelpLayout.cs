using OutputEngine;

namespace CliOutput.Help;

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
