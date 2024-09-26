// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace OutputEngine.Primitives;

public abstract class Layout
{
    protected Layout(IEnumerable<Section> sections, Title? title = null)
    {
        Sections = sections.ToList();
        Title = title;
    }

    public Title? Title { get;  }
    public List<Section> Sections { get; private set; } = [];

}
