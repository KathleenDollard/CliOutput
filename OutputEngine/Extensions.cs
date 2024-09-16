namespace OutputEngine;

public static class Extensions
{
    public static IEnumerable<string> Wrap(this string s, int outputWidth)
    {
        if (outputWidth <= 0)
        {
            throw new ArgumentException("Output width must be greater than 0", nameof(outputWidth));
        }
        List<string> lines = new();
        var position = 0;
        var lastBreakPosition = 0;
        while (position < s.Length)
        {
            var start = outputWidth;  // look for space after outputWidth first
            if (s.Length <= start + lastBreakPosition)
            {
                lines.Add(s.Substring(lastBreakPosition));
                break; // Breaks out of while loop
            }
            for (var i = start; i >= 0; i--)
            {
                var pos = lastBreakPosition + i;
                if (char.IsWhiteSpace(s[pos]))
                {
                    lines.Add(s.Substring(lastBreakPosition, pos - lastBreakPosition));
                    lastBreakPosition = pos + 1; // +1 here skips the ws char
                    break; // breaks out of for loop
                }
                if (i == 0) // We incremented to end and didn't find ws - so break at outputWidth
                {
                    lines.Add(s.Substring(lastBreakPosition, outputWidth));
                    lastBreakPosition = lastBreakPosition + outputWidth;
                    break; // breaks out of for loop
                }
            }
        }

        return lines;
    }

    public static string JoinLines(this IEnumerable<string> lines)
        => string.Join(Environment.NewLine, lines);

    //    public static Help ToHelpCommand(this Command command, bool canExecute = false)
    //        => new(command, CreateHelpCommand(command, canExecute: canExecute));

    //    private static HelpCommand CreateHelpCommand(Command command, HelpCommand? parent = null, bool canExecute = false)
    //    {
    //        var helpCommand = new HelpCommand(command.Name, command.Description)
    //        {
    //            ParentCommandNames = command.Parents.Select(x => x.Name).ToList(),
    //            CanExecute = canExecute
    //        };

    //        foreach (var argument in command.Arguments)
    //        {
    //            helpCommand.Arguments.Add(argument.Name, argument.Description);
    //        }
    //        foreach (var option in command.Options)
    //        {
    //            var newHelpOption = helpCommand.Options.Add(option.Aliases.First(), option.Description);
    //            foreach (var alias in option.Aliases.Skip(1))
    //            {
    //                newHelpOption.Aliases.Add(alias);
    //            }
    //        }
    //        foreach (var subCommand in command.Children.OfType<Command>())
    //        {
    //            var helpSubCommand = helpCommand.SubCommands.Add(subCommand.Aliases.First().Trim(), subCommand.Description);
    //            var newHelpCommand = CreateHelpCommand(subCommand, helpCommand);
    //            foreach (var alias in subCommand.Aliases.Skip(1))
    //            {
    //                newHelpCommand.Aliases.Add(alias.Trim());
    //            }
    //        }
    //        return helpCommand;
    //    }
}
