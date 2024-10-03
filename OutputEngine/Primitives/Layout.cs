// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace OutputEngine.Primitives;

// TODO: Probably replace this with root BlockContainer
public abstract class Layout
{
    protected Layout(IEnumerable<Section> sections, Header? title = null)
    {
        Sections = sections.ToList();
        Title = title;
    }

    public Header? Title { get;  }
    public List<Section> Sections { get; private set; } = [];

}
