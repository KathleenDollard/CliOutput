// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using OutputEngine.Primitives;
using System.Globalization;

namespace CliOutput.Help;

public class HelpArguments : HelpSection
{
    public HelpArguments(CliCommand command) : base("Arguments", command)
    {
        var table = GetTable();
        foreach (var arg in Command.Arguments)
        {
            table.AddRow([$"<{arg.Name.ToUpper(CultureInfo.InvariantCulture)}>", arg.Description ?? string.Empty]);
        }
        Add(table);
    }

    private static Table GetTable()
    {
        TableColumn[] columns = [
            new("Name", TableColumnKind.Mandatory,minWidth: 25),
            new("Description", TableColumnKind.Mandatory)
        ];
        var table = new Table(columns);
        return table;
    }
}