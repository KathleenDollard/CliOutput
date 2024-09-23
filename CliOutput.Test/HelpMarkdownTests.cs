using FluentAssertions;
using CliOutput.Help;
using OutputEngine.Renderers;
using OutputEngine;

namespace CliOutput.Test;

public class HelpMarkdownTests
{
    [Fact]
    public void Outputs_description()
    {
        var command = new CliCommand("Hello", description: "World");
        var help = HelpLayout.Create(command);
        var description = help.Sections.OfType<HelpDescription>().First();
        var renderer = new MarkdownRenderer(new OutputContext(true));

        renderer.RenderSection(description);

        var result = renderer.GetBuffer();
        result.Should()
            .Be($"## Description{Environment.NewLine}{Environment.NewLine}World{Environment.NewLine}{Environment.NewLine}");
    }

    [Fact]
    public void Outputs_usage()
    {
        var command = new CliCommand("Hello", description: "World");
        var help = HelpLayout.Create(command);
        var usage = help.Sections.OfType<HelpUsage>().First();
        var renderer = new MarkdownRenderer(new OutputContext(true));

        renderer.RenderSection(usage);

        var result = renderer.GetBuffer();
        result.Should()
            .Be($"## Usage{Environment.NewLine}{Environment.NewLine}Hello{Environment.NewLine}{Environment.NewLine}");
    }

    [Fact]
    public void Outputs_usage_with_subcommands()
    {
        var command = new CliCommand("Hello", description: "World");
        var subcommand1 = new CliCommand("Welcome", description: "Welcome to the beach!");
        var subcommand2 = new CliCommand("Brrr", description: "Let's go skiing!");
        command.AddSubCommand(subcommand1);
        command.AddSubCommand(subcommand2);
        var help = HelpLayout.Create(command);
        var usage = help.Sections.OfType<HelpUsage>().First();
        var renderer = new MarkdownRenderer(new OutputContext(true));

        renderer.RenderSection(usage);

        var result = renderer.GetBuffer();
        result.Should()
            .Be($"## Usage{Environment.NewLine}{Environment.NewLine}Hello [command]{Environment.NewLine}{Environment.NewLine}");
    }

    [Fact]
    public void Outputs_usage_with_parents()
    {
        var command = new CliCommand("Hello", description: "World");
        var subcommand1 = new CliCommand("Welcome", description: "Welcome to the beach!");
        var subcommand2 = new CliCommand("Brrr", description: "Let's go skiing!");
        command.AddSubCommand(subcommand1);
        subcommand1.AddSubCommand(subcommand2);
        var help = HelpLayout.Create(subcommand2);
        var usage = help.Sections.OfType<HelpUsage>().First();
        var renderer = new MarkdownRenderer(new OutputContext(true));

        renderer.RenderSection(usage);

        var result = renderer.GetBuffer();
        result.Should()
            .Be($"## Usage{Environment.NewLine}{Environment.NewLine}Hello Welcome Brrr{Environment.NewLine}{Environment.NewLine}");
    }

    [Fact]
    public void Outputs_usage_with_arguments()
    {
        var command = new CliCommand("Hello", description: "World");
        command.Arguments.Add(new CliArgument("Morning", description: "It is the morning."));
        command.Arguments.Add(new CliArgument("Evening", description: "Now, it is the evening"));
        var help = HelpLayout.Create(command);
        var usage = help.Sections.OfType<HelpUsage>().First();
        var renderer = new MarkdownRenderer(new OutputContext(true));

        renderer.RenderSection(usage);

        var result = renderer.GetBuffer();
        result.Should()
            .Be($"## Usage{Environment.NewLine}{Environment.NewLine}Hello \\<MORNING\\> \\<EVENING\\>{Environment.NewLine}{Environment.NewLine}");
    }

    [Fact]
    public void Outputs_usage_with_options()
    {
        var command = new CliCommand("Hello", description: "World");
        command.Options.Add(new CliOption("Morning", description: "It is the morning."));
        command.Options.Add(new CliOption("Evening", description: "Now, it is the evening"));
        var help = HelpLayout.Create(command);
        var usage = help.Sections.OfType<HelpUsage>().First();
        var renderer = new MarkdownRenderer(new OutputContext(true));

        renderer.RenderSection(usage);

        var result = renderer.GetBuffer();
        result.Should()
            .Be($"## Usage{Environment.NewLine}{Environment.NewLine}Hello [options]{Environment.NewLine}{Environment.NewLine}");
    }

    [Fact]
    public void Outputs_arguments()
    {
        var command = new CliCommand("Hello", description: "World");
        command.Arguments.Add(new CliArgument("Morning", description: "It is the morning."));
        command.Arguments.Add(new CliArgument("VerEarlyEvening", description: "Now, it is the evening"));
        var help = HelpLayout.Create(command);
        var arguments = help.Sections.OfType<HelpArguments>().First();
        var renderer = new MarkdownRenderer(new OutputContext(true));

        renderer.RenderSection(arguments);

        var result = renderer.GetBuffer();
        // TODO: More fully test this output, possibly via Approvals testing
        result.Should()
            .StartWith($"## Arguments{Environment.NewLine}{Environment.NewLine}|||");
    }

    [Fact]
    public void Outputs_arguments_with_long_description()
    {
        var command = new CliCommand("Hello", description: "World");
        command.Arguments.Add(new CliArgument("EarlyEvening", description: "Now, it is the evening, with a very, very, very, very, very,  very, very,  very, very,  very, very,  very, very,  very, very,  very, very,  very, very,  very, very, long description"));
        command.Arguments.Add(new CliArgument("Morning", description: "It is the morning."));
        var help = HelpLayout.Create(command);
        var arguments = help.Sections.OfType<HelpArguments>().First();
        var renderer = new MarkdownRenderer(new OutputContext(true));

        renderer.RenderSection(arguments);

        var result = renderer.GetBuffer();
        // TODO: More fully test this output, possibly via Approvals testing
        result.Should()
            .StartWith($"## Arguments{Environment.NewLine}{Environment.NewLine}|||");
    }

    [Fact]
    public void Outputs_options()
    {
        var command = new CliCommand("Hello", description: "World");
        command.Options.Add(new CliOption("Morning", description: "It is the morning."));
        command.Options.Add(new CliOption("VerEarlyEvening", description: "Now, it is the evening"));
        var help = HelpLayout.Create(command);
        var arguments = help.Sections.OfType<HelpOptions>().First();
        var renderer = new MarkdownRenderer(new OutputContext(true));

        renderer.RenderSection(arguments);

        var result = renderer.GetBuffer();
        // TODO: More fully test this output, possibly via Approvals testing
        result.Should()
            .StartWith($"## Options{Environment.NewLine}{Environment.NewLine}|||");
    }

    [Fact]
    public void Outputs_subcommands()
    {
        var command = new CliCommand("Hello", description: "World");
        command.AddSubCommand(new CliCommand("Morning", description: "It is the morning."));
        command.AddSubCommand(new CliCommand("VerEarlyEvening", description: "Now, it is the evening"));
        var help = HelpLayout.Create(command);
        var arguments = help.Sections.OfType<HelpSubcommands>().First();
        var renderer = new MarkdownRenderer(new OutputContext(true));

        renderer.RenderSection(arguments);

        var result = renderer.GetBuffer();
        // TODO: More fully test this output, possibly via Approvals testing
        result.Should()
            .StartWith($"## Subcommands{Environment.NewLine}{Environment.NewLine}|||");
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
        var renderer = new MarkdownRenderer(new OutputContext(true));

        renderer.RenderLayout(help);

        var result = renderer.GetBuffer();
        // TODO: More fully test this output, possibly via Approvals testing
        result.Should()
            .StartWith($"## Description{Environment.NewLine}{Environment.NewLine}");
    }
}