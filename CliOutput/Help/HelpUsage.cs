// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using OutputEngine;
using OutputEngine.Primitives;

namespace CliOutput.Help;

public class HelpUsage : HelpSection
{
    public HelpUsage(CliCommand command)
        : base("Usage", command)
    {
        Add(GetHelpUse());
    }

    public Paragraph GetHelpUse()
    {
        Paragraph ret = new();
        AddHelpUsageLine(Command, ret);
        return ret;

        static void AddHelpUsageLine(CliCommand command, Paragraph paragraph)
        {
            paragraph.AddRange(command.Ancestors.Reverse().Select(x => new TextPart(x.Name, TextStyle.SlightlyImportant)));
            paragraph.Add(new TextPart(command.Name));
            if (command.Arguments.Any())
            {
                paragraph.AddRange(command.Arguments.Select(arg => UsageFromArg(arg)));
            }
            if (command.SubCommands.Any())
            {
                paragraph.Add(new TextPart("[command]", TextStyle.SlightlyImportant));
            }
            if (command.Options.Count > 0)
            {
                paragraph.Add(new TextPart("[options]", TextStyle.SlightlyImportant));
            }

            static TextPart UsageFromArg(CliArgument arg)
               => new($"<{arg.DisplayName}>");
        }

    }
}