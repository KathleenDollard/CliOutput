// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using OutputEngine.Primitives;

namespace CliOutput.Help;

public class HelpDescription : HelpSection
{
    public HelpDescription(CliCommand command) 
        : base("Description", command)
    {
        if (command.Description is not null)
        {
            Add(new Paragraph(command.Description));
        }
    }
}