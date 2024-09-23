using FluentAssertions;
using CliOutput.Help;
using OutputEngine.Renderers;
using OutputEngine;

namespace CliOutput.Test;

public class HelpTerminalTests
{
    [Fact]
    public void Outputs_description()
    {
        var command = new CliCommand("Hello", "World");
        var help = HelpLayout.Create(command);
        var description = help.Sections.OfType<HelpDescription>().First();
        var renderer = new TerminalRenderer(new OutputContext(true));

        renderer.RenderSection(description);

        var result = renderer.GetBuffer();
        result.Should()
            .Be($"Description:{Environment.NewLine}  World{Environment.NewLine}{Environment.NewLine}");
    }

    [Fact]
    public void Outputs_usage()
    {
        var command = new CliCommand("Hello", "World");
        var help = HelpLayout.Create(command);
        var usage = help.Sections.OfType<HelpUsage>().First();
        var renderer = new TerminalRenderer(new OutputContext(true));

        renderer.RenderSection(usage);

        var result = renderer.GetBuffer();
        result.Should()
            .Be($"Usage:{Environment.NewLine}  Hello{Environment.NewLine}{Environment.NewLine}");
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
        var renderer = new TerminalRenderer(new OutputContext(true));

        renderer.RenderSection(usage);

        var result = renderer.GetBuffer();
        result.Should()
            .Be($"Usage:{Environment.NewLine}  Hello [command]{Environment.NewLine}{Environment.NewLine}");
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
        var renderer = new TerminalRenderer(new OutputContext(true));

        renderer.RenderSection(usage);

        var result = renderer.GetBuffer();
        result.Should()
            .Be($"Usage:{Environment.NewLine}  Hello Welcome Brrr{Environment.NewLine}{Environment.NewLine}");
    }

    [Fact]
    public void Outputs_usage_with_arguments()
    {
        var command = new CliCommand("Hello", "World");
        command.Arguments.Add(new CliArgument("Morning", "It is the morning."));
        command.Arguments.Add(new CliArgument("Evening", "Now, it is the evening"));
        var help = HelpLayout.Create(command);
        var usage = help.Sections.OfType<HelpUsage>().First();
        var renderer = new TerminalRenderer(new OutputContext(true));

        renderer.RenderSection(usage);

        var result = renderer.GetBuffer();
        result.Should()
            .Be($"Usage:{Environment.NewLine}  Hello <MORNING> <EVENING>{Environment.NewLine}{Environment.NewLine}");
    }

    [Fact]
    public void Outputs_usage_with_options()
    {
        var command = new CliCommand("Hello", "World");
        command.Options.Add(new CliOption("Morning", description: "It is the morning."));
        command.Options.Add(new CliOption("Evening", description: "Now, it is the evening"));
        var help = HelpLayout.Create(command);
        var usage = help.Sections.OfType<HelpUsage>().First();
        var renderer = new TerminalRenderer(new OutputContext(true));

        renderer.RenderSection(usage);

        var result = renderer.GetBuffer();
        result.Should()
            .Be($"Usage:{Environment.NewLine}  Hello [options]{Environment.NewLine}{Environment.NewLine}");
    }


    [Fact]
    public void Outputs_usage_with_arguments_options_and_subcommands()
    {
        var command = new CliCommand("build", description: ".NET Builder");
        command.Arguments.Add(new CliArgument("project", description: "The project or solution file to operate on. If a file is not specified, the command will search the current directory for one."));
        command.Options.Add(new CliOption("-c", description: "The configuration to use for building the project. The default for most projects is 'Debug'."));
        command.Options.Add(new CliOption("-f", description: "The target framework to build for. The target framework must also be specified in the project file."));
        command.Options.Add(new CliOption("-r", description: "The target runtime to build for."));
        var help = HelpLayout.Create(command);
        var usage = help.Sections.OfType<HelpUsage>().First();
        var renderer = new TerminalRenderer(new OutputContext(true));

        renderer.RenderSection(usage);

        var result = renderer.GetBuffer();
        result.Should()
            .Be($"Usage:{Environment.NewLine}  build <PROJECT> [options]{Environment.NewLine}{Environment.NewLine}");
    }

    [Fact]
    public void Outputs_arguments()
    {
        var command = new CliCommand("Hello", "World");
        command.Arguments.Add(new CliArgument("Morning", "It is the morning."));
        command.Arguments.Add(new CliArgument("VerEarlyEvening", "Now, it is the evening"));
        var help = HelpLayout.Create(command);
        var arguments = help.Sections.OfType<HelpArguments>().First();
        var renderer = new TerminalRenderer(new OutputContext(true));

        renderer.RenderSection(arguments);

        var result = renderer.GetBuffer();
        // TODO: More fully test this output, possibly via Approvals testing
        result.Should()
            .StartWith($"Arguments:{Environment.NewLine}  <MORNING> ");
    }

    [Fact]
    public void Outputs_arguments_with_long_description()
    {
        var command = new CliCommand("Hello", "World");
        command.Arguments.Add(new CliArgument("EarlyEvening", "Now, it is the evening, with a very, very, very, very, very,  very, very,  very, very,  very, very,  very, very,  very, very,  very, very,  very, very,  very, very, long description"));
        command.Arguments.Add(new CliArgument("Morning", "It is the morning."));
        var help = HelpLayout.Create(command);
        var arguments = help.Sections.OfType<HelpArguments>().First();
        var renderer = new TerminalRenderer(new OutputContext(true));

        renderer.RenderSection(arguments);

        var result = renderer.GetBuffer();
        // TODO: More fully test this output, possibly via Approvals testing
        result.Should()
            .StartWith($"Arguments:{Environment.NewLine}  ");
    }

    [Fact]
    public void Outputs_options()
    {
        var command = new CliCommand("Hello", "World");
        command.Options.Add(new CliOption("Morning", description: "It is the morning."));
        command.Options.Add(new CliOption("VerEarlyEvening", description: "Now, it is the evening"));
        var help = HelpLayout.Create(command);
        var options = help.Sections.OfType<HelpOptions>().First();
        var renderer = new TerminalRenderer(new OutputContext(true));

        renderer.RenderSection(options);

        var result = renderer.GetBuffer();
        // TODO: More fully test this output, possibly via Approvals testing
        result.Should()
            .StartWith($"Options:{Environment.NewLine}  Morning");
    }

    [Fact]
    public void Outputs_subcommands()
    {
        var command = new CliCommand("Hello", "World");
        command.AddSubCommand(new CliCommand("Morning", "It is the morning."));
        command.AddSubCommand(new CliCommand("VerEarlyEvening", "Now, it is the evening"));
        var help = HelpLayout.Create(command);
        var arguments = help.Sections.OfType<HelpSubcommands>().First();
        var renderer = new TerminalRenderer(new OutputContext(true));

        renderer.RenderSection(arguments);

        var result = renderer.GetBuffer();
        // TODO: More fully test this output, possibly via Approvals testing
        result.Should()
            .StartWith($"Subcommands:{Environment.NewLine}  ");
    }

    [Fact]
    public void Outputs_help()
    {
        var command = new CliCommand("Hello", "World");
        command.Arguments.Add(new CliArgument("Morning", "It is the morning."));
        command.Arguments.Add(new CliArgument("VeryEarlyEvening", "Now, it is the evening."));
        command.Options.Add(new CliOption("Red", description: "The color is read."));
        command.Options.Add(new CliOption("Blue", description: "The color is blue."));
        command.AddSubCommand(new CliCommand("One", description: "The first subcommand."));
        command.AddSubCommand(new CliCommand("Two", description: "The second subcommand"));
        var help = HelpLayout.Create(command);
        var renderer = new TerminalRenderer(new OutputContext(true));

        renderer.RenderLayout(help);

        var result = renderer.GetBuffer();
        // TODO: More fully test this output, possibly via Approvals testing
        result.Should()
            .StartWith($"Description:{Environment.NewLine}  ");
    }
}