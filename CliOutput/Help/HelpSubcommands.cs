//using System.Collections;

namespace CliOutput.Help;

public class HelpSubcommands(CliCommand command) 
    : HelpSection("Subcommands", command)
{
}