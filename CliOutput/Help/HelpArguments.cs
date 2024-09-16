using OutputEngine.Primitives;

namespace CliOutput.Help;

public class HelpArguments : HelpSection
{
    public HelpArguments(CliCommand command) : base("Arguments",command)
    {
        var table = GetTable();
        foreach (var arg in Command.Arguments)
        {
            table.AddRow([arg.Name, arg.Description]);
        }
        Add( table);
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