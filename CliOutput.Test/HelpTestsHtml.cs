using CliOutput.Help;
using FluentAssertions;
using OutputEngine;
using OutputEngine.Targets;

namespace CliOutput.Test;

public class HelpTestsHtml
{
    [Fact]
    public void Outputs_description()
    {
        var command = new CliCommand("Hello", description: "World");
        var help = HelpLayout.Create(command);
        var description = help.Sections.OfType<HelpDescription>().First();
        var writer = new Html(new OutputContext(true));

        writer.Write(description);

        var result = writer.GetBuffer();
        result.Should()
            .Be("<h2>Description</h2><p>&nbsp;&nbsp;World</p>");
    }

    [Fact]
    public void Outputs_usage()
    {
        var command = new CliCommand("Hello", "World");
        var help = HelpLayout.Create(command);
        var usage = help.Sections.OfType<HelpUsage>().First();
        var writer = new Html(new OutputContext(true));

        writer.Write(usage);

        var result = writer.GetBuffer();
        result.Should()
            .Be("<h2>Usage</h2><p>&nbsp;&nbsp;Hello</p>");
    }

    [Fact]
    public void Outputs_usage_with_subcommands()
    {
        var command = new CliCommand("Hello", "World");
        var subcommand1 = new CliCommand("Welcome", "Welcome to the beach!");
        var subcommand2 = new CliCommand("Brrr", "Let's go skiing!");
        command.AddSubCommand(subcommand1);
        command.AddSubCommand(subcommand2);
        var help = HelpLayout.Create(command);
        var usage = help.Sections.OfType<HelpUsage>().First();
        var writer = new Html(new OutputContext(true));

        writer.Write(usage);

        var result = writer.GetBuffer();
        result.Should()
            .Be("<h2>Usage</h2><p>&nbsp;&nbsp;Hello [command]</p>");
    }

    [Fact]
    public void Outputs_usage_with_parents()
    {
        var command = new CliCommand("Hello", "World");
        var subcommand1 = new CliCommand("Welcome", "Welcome to the beach!");
        var subcommand2 = new CliCommand("Brrr", "Let's go skiing!");
        command.AddSubCommand(subcommand1);
        subcommand1.AddSubCommand(subcommand2);
        var help = HelpLayout.Create(subcommand2);
        var usage = help.Sections.OfType<HelpUsage>().First();
        var writer = new Html(new OutputContext(true));

        writer.Write(usage);

        var result = writer.GetBuffer();
        result.Should()
            .Be("<h2>Usage</h2><p>&nbsp;&nbsp;Hello Welcome Brrr</p>");
    }

    [Fact]
    public void Outputs_usage_with_arguments()
    {
        var command = new CliCommand("Hello", description: "World");
        command.Arguments.Add(new CliArgument("Morning", description: "It is the morning."));
        command.Arguments.Add(new CliArgument("Evening", description: "Now, it is the evening"));
        var help = HelpLayout.Create(command);
        var usage = help.Sections.OfType<HelpUsage>().First();
        var writer = new Html(new OutputContext(true));

        writer.Write(usage);

        var result = writer.GetBuffer();
        result.Should()
            .Be("<h2>Usage</h2><p>&nbsp;&nbsp;Hello &lt;MORNING&gt; &lt;EVENING&gt;</p>");
    }

    [Fact]
    public void Outputs_usage_with_options()
    {
        var command = new CliCommand("Hello", description: "World");
        command.Options.Add(new CliOption("Morning", description: "It is the morning."));
        command.Options.Add(new CliOption("Evening", description: "Now, it is the evening"));
        var help = HelpLayout.Create(command);
        var usage = help.Sections.OfType<HelpUsage>().First();
        var writer = new Html(new OutputContext(true));

        writer.Write(usage);

        var result = writer.GetBuffer();
        result.Should()
            .Be("<h2>Usage</h2><p>&nbsp;&nbsp;Hello [options]</p>");
    }

    [Fact]
    public void Outputs_arguments()
    {
        var command = new CliCommand("Hello", "World");
        command.Arguments.Add(new CliArgument("Morning", "It is the morning."));
        command.Arguments.Add(new CliArgument("VerEarlyEvening", "Now, it is the evening"));
        var help = HelpLayout.Create(command);
        var arguments = help.Sections.OfType<HelpArguments>().First();
        var writer = new Html(new OutputContext(true));

        writer.Write(arguments);

        var result = writer.GetBuffer();
        // TODO: More fully test this output, possibly via Approvals testing
        result.Should()
            .StartWith("<h2>Arguments</h2><table><tr><td>&lt;MORNING&gt;");
    }

    [Fact]
    public void Outputs_arguments_with_long_description()
    {
        var command = new CliCommand("Hello", "World");
        command.Arguments.Add(new CliArgument("EarlyEvening", "Now, it is the evening, with a very, very, very, very, very,  very, very,  very, very,  very, very,  very, very,  very, very,  very, very,  very, very,  very, very, long description"));
        command.Arguments.Add(new CliArgument("Morning", "It is the morning."));
        var help = HelpLayout.Create(command);
        var arguments = help.Sections.OfType<HelpArguments>().First();
        var writer = new Html(new OutputContext(true));

        writer.Write(arguments);
        writer.Write(arguments);

        var result = writer.GetBuffer();
        // TODO: More fully test this output, possibly via Approvals testing
        result.Should()
            .StartWith("<h2>Arguments</h2><table><tr><td>&lt;EARLYEVENING&gt;");
    }

    [Fact]
    public void Outputs_options()
    {
        var command = new CliCommand("Hello", description: "World");
        command.Options.Add(new CliOption("Morning", description: "It is the morning."));
        command.Options.Add(new CliOption("VerEarlyEvening", description: "Now, it is the evening"));
        var help = HelpLayout.Create(command);
        var arguments = help.Sections.OfType<HelpOptions>().First();
        var writer = new Html(new OutputContext(true));

        writer.Write(arguments);

        var result = writer.GetBuffer();
        // TODO: More fully test this output, possibly via Approvals testing
        result.Should()
            .StartWith($"<h2>Options</h2><table><tr><td>Morning");
    }

    [Fact]
    public void Outputs_subcommands()
    {
        var command = new CliCommand("Hello", "World");
        command.AddSubCommand(new CliCommand("Morning", "It is the morning."));
        command.AddSubCommand(new CliCommand("VerEarlyEvening", "Now, it is the evening"));
        var help = HelpLayout.Create(command);
        var arguments = help.Sections.OfType<HelpSubcommands>().First();
        var writer = new Html(new OutputContext(true));

        writer.Write(arguments);

        var result = writer.GetBuffer();
        // TODO: More fully test this output, possibly via Approvals testing
        result.Should()
            .StartWith($"<h2>Subcommands</h2><table><tr><td>Morning");
    }

    [Fact]
    public void Outputs_help()
    {
        var command = new CliCommand("Hello", description: "World");
        command.Arguments.Add(new CliArgument("Morning", description: "It is the morning."));
        command.Arguments.Add(new CliArgument("VeryEarlyEvening", description: "Now, it is the evening."));
        command.Options.Add(new CliOption("Red", description: "The color is read."));
        command.Options.Add(new CliOption("Blue", description: "The color is blue."));
        command.AddSubCommand(new CliCommand("One", description: "The first subcommand."));
        command.AddSubCommand(new CliCommand("Two", description: "The second subcommand"));
        var help = HelpLayout.Create(command);
        var writer = new Html(new OutputContext(true));

        writer.Write(help);

        var result = writer.GetBuffer();
        // TODO: More fully test this output, possibly via Approvals testing
        result.Should()
            .StartWith($"<h2>Description</h2>");
    }
}
