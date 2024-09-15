//using System.Collections;

namespace CliOutput.Help;

public class HelpOptions(CliCommand command) 
    : HelpSection("Options", command)
{
}