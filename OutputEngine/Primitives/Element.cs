// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace OutputEngine.Primitives;

// TODO: Constructor for implicit types from elements like Link or InlineCode
public abstract class Element(Styles styles)
{
    public Styles Styles { get;  } = styles;

    public void AddStyle(string style)
    {
        Styles.Add(style);
    }

    public void ResetStyles()
    {
        Styles.Reset();
    }
}
