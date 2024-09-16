using OutputEngine.Primitives;

namespace CliOutput.Help;

public class HelpDescription : HelpSection
{
    public HelpDescription(CliCommand command) 
        : base("Description", command)
    {
        if (command.Description is not null)
        {
            Add(new Paragraph(new TextPart(command.Description)));
        }
    }
}