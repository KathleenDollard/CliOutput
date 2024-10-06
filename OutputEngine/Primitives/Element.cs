// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace OutputEngine.Primitives;

/// <summary>
/// The base type for all primitive elements.
/// </summary>
public abstract class Element
{
    private Styles styles;

    /// <summary>
    /// Return the styles for this element.
    /// </summary>
    public Styles Styles 
        => styles;

    /// <summary>
    /// Add a style to the element. 
    /// </summary>
    /// <param name="style">The name of the style to add.</param>
    public void AddStyle(string style) 
        => styles.Add(style);

    /// <summary>
    /// Reset the styles for the element to only the implicit styles for the class of element.
    /// </summary>
    public void ResetStyles() 
        => styles.Reset();
}
