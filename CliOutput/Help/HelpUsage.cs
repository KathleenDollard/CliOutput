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
            paragraph.AddRange(command.Ancestors.Reverse().Select(x => new TextPart(x.Name, TextAppearance.LessImportant)));
            paragraph.Add((TextPart)command.Name);
            paragraph.AddRange(command.Arguments.Select(arg => usageFromArg(arg)));
            if (command.SubCommands.Any())
            {
                paragraph.Add(new TextPart("[command]", TextAppearance.LessImportant));
            }
            if (command.Options.Count > 0)
            {
                paragraph.Add(new TextPart("[options]", TextAppearance.LessImportant));
            }

            static TextPart usageFromArg(CliArgument arg)
               => new($"<{arg.DisplayName}>", whitespace: Whitespace.NeitherBeforeOrAfter);
        }

    }
}