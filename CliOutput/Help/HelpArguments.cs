using CliOutput.Primitives;

namespace CliOutput.Help;

public class HelpArguments(CliCommand command)
    : HelpSection("Arguments", command)
{
    public Table GetHelp()
    {
        var table = GetTable();
        foreach (var arg in Command.Arguments)
        {
            table.AddRow([arg.Name, arg.Description]);
        }
        return table;
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