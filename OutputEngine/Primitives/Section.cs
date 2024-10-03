// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace OutputEngine.Primitives;

public class Section : BlockContainer
{
    public Section(string? heading, params BlockElement[] elements)
        : base(Styles.CreateWithImplicit(BlockStyle.Section))
    {
        if (heading != null)
        {
            Heading = new Header(heading);
        }
    }

    // TODO: Replace Paragraph type in this case with Title, then rename to Heading. Supplies the implicit style and semantics.
    public Header? Heading { get; }
}