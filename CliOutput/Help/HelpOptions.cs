//using System.Collections;

using OutputEngine.Primitives;

namespace CliOutput.Help;

public class HelpOptions : HelpSection
{
    public HelpOptions(CliCommand command) 
        : base("Options", command)
    {
        var table = GetTable();
        foreach (var option in Command.Options)
        {
            table.AddRow([option.Name, option.Description ?? string.Empty]);
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
