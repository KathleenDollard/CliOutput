//using System.Collections;

namespace CliOutput.Help;

public class HelpDescription(CliCommand command) 
    : HelpSection("Description", command)
{
}