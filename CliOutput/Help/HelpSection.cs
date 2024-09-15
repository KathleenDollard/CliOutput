//using System.Collections;

namespace CliOutput.Help;

public class HelpSection(string title, CliCommand command)
{
    public CliCommand Command { get; } = command;
    public string Title { get; } = title;
}