// Only Help works, and only the build command. 
// TODO: Switch this to use Powderhouse when we move back into the main repo
// TODO: Make a more complete example

using CliOutput;
using CliOutput.Help;
using OutputEngine;
using OutputEngine.Targets;

var rootCommand = new CliCommand("dotnet", "The base of the .NET CLI");
var buildCommand = new CliCommand("build", description: ".NET Builder");
buildCommand.Arguments.Add(new CliArgument("project", description: "The project or solution file to operate on. If a file is not specified, the command will search the current directory for one."));
buildCommand.Options.Add(new CliOption("--configuration", ["-c"], description: "The configuration to use for building the project. The default for most projects is 'Debug'."));
buildCommand.Options.Add(new CliOption("--framework",["-f"], description: "The target framework to build for. The target framework must also be specified in the project file."));
buildCommand.Options.Add(new CliOption("--runtime",["-r"], description: "The target runtime to build for."));
buildCommand.Options.Add(new CliOption("--no-restore", description: "Do not restore the project before building."));
buildCommand.Options.Add(new CliOption("--interactive", description: "Allows the command to stop and wait for user input or action (for example to complete authentication)."));
buildCommand.Options.Add(new CliOption("--verbosity",["-v"], description: "Set the MSBuild verbosity level. Allowed values are q[uiet], m[inimal], n[ormal], d[etailed], and diag[nostic]."));
buildCommand.Options.Add(new CliOption("--output",["-o"], description: "The output directory to place built artifacts in."));
buildCommand.Options.Add(new CliOption("--self-contained", description: "Publish the .NET runtime with your application so the runtime doesn't need to be installed on the target machine. The default is 'false.' However, when targeting .NET 7 or lower, the default is 'true' if a runtime identifier is specified."));
buildCommand.Options.Add(new CliOption("--help", ["-?", "-h"], "Show command line help."));
buildCommand.Options.Add(new CliOption("--helpR", ["-h"], "Show command line help."));

// buildCommand.Arguments.Add(new CliOption("--version-suffix", "Set the value of the $(VersionSuffix) property to use when building the project."));
// buildCommand.Arguments.Add(new CliOption("--debug", ""));
// buildCommand.Arguments.Add(new CliOption("--artifacts-path", "The artifacts path. All output from the project, including build, publish, and pack output, will go in subfolders under the specified path."));
// buildCommand.Arguments.Add(new CliOption("--no-incremental", "Do not use incremental building."));
// buildCommand.Arguments.Add(new CliOption("--no-dependencies", "Do not build project-to-project references and only build the specified project."));
// buildCommand.Arguments.Add(new CliOption("--nologo", "Do not display the startup banner or the copyright message."));
// buildCommand.Arguments.Add(new CliOption("--no-self-contained", "Publish your application as a framework dependent application. A compatible .NET runtime must be installed on the target machine to run your application."));
// buildCommand.Arguments.Add(new CliOption("-a", "--arch", "The target architecture."));
// buildCommand.Arguments.Add(new CliOption("--os", "The target operating system."));
// buildCommand.Arguments.Add(new CliOption("--disable-build-servers", "Force the command to ignore any persistent build servers."));

rootCommand.AddSubCommand(buildCommand);

// To make testing easier, currently only cares about the last item passed, mimicking a --helpR switch, sort of
// The last item specifies the output format temporarily.
//
// The future command line would be something like this, but we need to get comfy with having arguments to help:
//   dotnet build --help-r markdown (John)
//   dotnet build --help-r html     (Buyaa)
//   dotnet build --help-r rich     (Viktor)
//   dotnet build -h                (default to plain text)

// For this prototype, you can just enter

var help = HelpLayout.Create(buildCommand);
var outputContext = new OutputContext();
var renderer = args.LastOrDefault() switch
{
    "markdown" => (OutputEngine.Targets.CliOutput)new Markdown(outputContext),
    "html" => new Html(outputContext),
    "rich" => new RichTerminal(outputContext),
    _ => new Terminal(outputContext)
};
renderer.Write(help);
