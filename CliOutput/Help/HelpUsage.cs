//using System.Collections;

using CliOutput.Primitives;
using System.Collections.Generic;

namespace CliOutput.Help;

public class HelpUsage(CliCommand command)
    : HelpSection("Usage", command)
{
    public TextGroup GetHelpUse()
    {
        TextGroup ret = [];
        AddHelpUsageLine(Command, ret);
        return ret;

        static void AddHelpUsageLine(CliCommand command, TextGroup textGroup)
        {
            textGroup.AddRange(command.Ancestors.Reverse().Select(x => new TextPart(x.Name, TextAppearance.LessImportant)));
            textGroup.Add((TextPart)command.Name);
            textGroup.AddRange(command.Arguments.Select(arg => usageFromArg(arg)));
            if (command.Options.Count > 0)
            {
                textGroup.Add(new TextPart("[OPTIONS]", TextAppearance.LessImportant));
            }

            static TextPart usageFromArg(CliArgument arg)
               => new($"<{arg.DisplayName}>", whitespace: Whitespace.NeitherBeforeOrAfter);
        }

    }
}