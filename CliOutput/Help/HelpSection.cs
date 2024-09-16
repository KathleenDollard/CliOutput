using OutputEngine;

namespace CliOutput;

public abstract class HelpSection : Section
{
    protected HelpSection(string title, CliCommand command)
        : base(title) 
        => Command = command;

    public CliCommand Command { get; }
}
