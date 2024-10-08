------Start------

##Description

.NET Builder
##Usage

dotnet build <PROJECT>[options]
##Examples


##Arguments

|||
|project|The project or solution file to operate on. If a file is not specified, the command will search the current directory for one.|

##Options

|||
|-c|The configuration to use for building the project. The default for most projects is 'Debug'.|
|-f|The target framework to build for. The target framework must also be specified in the project file.|
|-r|The target runtime to build for.|
|--no-restore|Do not restore the project before building.|
|--interactive|Allows the command to stop and wait for user input or action (for example to complete authentication).|
|-v|Set the MSBuild verbosity level. Allowed values are q[uiet], m[inimal], n[ormal], d[etailed], and diag[nostic].|
|-o|The output directory to place built artifacts in.|
|--self-contained|Publish the .NET runtime with your application so the runtime doesn't need to be installed on the target machine. The default is 'false.' However, when targeting .NET 7 or lower, the default is 'true' if a runtime identifier is specified.|
|--help|Show command line help.|
|--helpR|Show command line help.|

##Subcommands

|||
------End------
