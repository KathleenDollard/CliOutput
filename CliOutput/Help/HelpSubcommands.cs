//using System.Collections;

using OutputEngine.Primitives;

namespace CliOutput.Help;

public class HelpSubcommands : HelpSection
{
    public HelpSubcommands(CliCommand command) 
        : base("Subcommands", command)
    {
        var table = GetTable();
        foreach (var subcommand in Command.SubCommands)
        {
            table.AddRow([subcommand.Name, subcommand.Description ?? string.Empty]);
        }
        Add(table);
    }

    private Table GetTable()
    {
        TableColumn[] columns = [
            new("Name", TableColumnKind.Mandatory),
            new("Description", TableColumnKind.Mandatory)
        ];
        var table = new Table(columns);
        return table;
    }
}