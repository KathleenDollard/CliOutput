// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace OutputEngine.Primitives;

public class Section : Group
{
    public Section(string heading)
    {
        Heading = new Paragraph(heading)
        {
            Appearance = Appearance.SectionHeading,
            NoNewLineAfter = true
        };
    }

    public Paragraph Heading { get; }


}