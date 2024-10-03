// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections;

namespace OutputEngine.Primitives;

public class BlockContainer : BlockElement, IEnumerable<BlockElement>
{
    protected BlockContainer(Styles styles) 
        : base(styles)
    { }

    public BlockContainer()
        : base(Styles.Empty)
    { }

    public List<BlockElement> Children { get; } = new();

    public void Add(BlockElement element) 
        => Children.Add(element);

    public IEnumerator<BlockElement> GetEnumerator() 
        => ((IEnumerable<BlockElement>)Children).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() 
        => ((IEnumerable)Children).GetEnumerator();
}
