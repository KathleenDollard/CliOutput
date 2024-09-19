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
            table.AddRow([AliasesAndName(option), option.Description ?? string.Empty]);
        }
        Add(table);

        static string AliasesAndName(CliOption option)
        {
            var aliases = option.Aliases is not null && option.Aliases.Any()
                ? string.Concat(option.Aliases.Select(a=>$"{a} , "))
                : string.Empty;
           
            return $"{aliases}{option.Name}";
        }
    }

    private Table GetTable()
    {
        TableColumn[] columns = [
            new("Name", TableColumnKind.Mandatory, minWidth: 25),
            new("Description", TableColumnKind.Mandatory)
        ];
        var table = new Table(columns);
        return table;
    }
}
