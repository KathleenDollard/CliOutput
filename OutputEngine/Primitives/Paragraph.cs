// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections;
using System.Text;

namespace OutputEngine.Primitives;

// TODO: Create a common base class for things that hold TextPart
public class Paragraph : TextContainer
{
    private readonly List<TextPart> parts = [];

    public Paragraph(params TextPart[] parts)
        : base(Styles.Empty, parts)
    { }

    public Paragraph(params string[] parts)
        : base(Styles.Empty, parts)
    { }

    public Paragraph()
        : base(Styles.Empty, Array.Empty<TextPart>())
    { }
}

