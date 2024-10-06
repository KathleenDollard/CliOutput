// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

// Based on `OneOrMany` at https://github.com/dotnet/roslyn and simplified to skip LINQ semantics

using System.Diagnostics;
using StyleTuple = (string? One, System.Collections.Generic.List<string>? Many);

// TODO: Consider making this and element immutable or additive only and at that point remove implicit styles here, and combine somewhere with user styles
// TODO: Consider making style available on all elements

namespace OutputEngine;

/// <summary>
/// Represents not style, a single style or several styles.
/// </summary>
/// <remarks>
/// Used because elements will usually have no styles or a single style but sometimes might contain multiple.
/// </remarks>
[DebuggerDisplay("{GetDebuggerDisplay(),nq}")]
[DebuggerTypeProxy(typeof(DebuggerProxy))]
public struct Styles
{
    public static readonly Styles Empty = new();
    private StyleTuple styles;

    /// <summary>
    /// Creates a new instance of <see cref="Styles"/> with the specified <paramref name="styles"/>.
    /// </summary>
    /// <param name="styles"></param>
    public Styles(params string[] styles) 
        => this.styles = styles.Length == 0 
                ? ((string? One, List<string>? Many))(null, null) 
                : AddRangeAndReturnTuple(styles);

    /// <summary>
    /// Gets the style at the specified <paramref name="index"/>. Generally used for calling code that prefers `for` to `foreach`.
    /// </summary>
    /// <param name="index">The position of the style to return.</param>
    /// <returns>The style at the specific index.</returns>
    /// <exception cref="IndexOutOfRangeException">Thrown if the index is less than zero or greater than or equal to the length of the Styles.</exception>
    public readonly string? this[int index]
    {
        get
        {
            return HasOneItem
                ? index != 0
                    ? throw new IndexOutOfRangeException()
                    : styles.One
                : styles.Many is null
                    ? null
                    : index >= styles.Many.Count
                        ? null
                        : (styles.Many?[index]);
        }
    }

    /// <summary>
    /// Gets the number of styles in the collection.
    /// </summary>
    public readonly int Count
        => HasOneItem
                ? 1
                : styles.Many is null
                    ? 0
                    : styles.Many.Count;

    /// <remarks>
    /// Used only by <see cref="Element"/>. This overload can be removed when the Add overload that takes IEnumerable is made into a params array
    /// </remarks>
    internal void Add(string item)
        => this.styles = Contains(item)
                        ? styles
                        : AddAndReturnTuple(item);

    /// <remarks>
    /// TOO: mhutch: Is there a way around the following issue? If we allow floating Styles, it is inconvenient that users can't add to them. I do not care a lot about this as I think it can be solved later.
    /// Used only by <see cref="Element"/>. Do not make public, as users are likely to stub their toes that the struct results in a copy returned from Element.Property.
    /// </remarks>
    internal void Add(IEnumerable<string> items)
        => styles = NewOnly(items) switch
        {
            null or [] => styles,
            [var item] => AddAndReturnTuple(item),
            var many => AddRangeAndReturnTuple(many)
        };

    /// <summary>
    /// This is used for testing to ensure Lists are not incorrectly created.
    /// </summary>
    /// <returns></returns>
    // TODO: Figure out why Internals Visible To is not working for this project from CliOutput.Tests
    internal StyleTuple GetTupleForTesting() 
        => this.styles;

    internal void Clear() 
        => styles = (null, null);

    private readonly bool HasOneItem
        => styles.One is not null;

    private readonly StyleTuple AddAndReturnTuple(string? item)
        => styles switch
        {
            (null, null) => (item, null),
            (var one, null) => (null, [one, item]),
            (null, var many) => (null, [.. many, item]),
            _ => throw new InvalidOperationException()
        };

    private readonly StyleTuple AddRangeAndReturnTuple(IEnumerable<string>? many)
        => many is null || !many.Any()
            ? styles
            : styles switch
                {
                    (null, null) => (null, [.. many]),
                    (var one, null) => (null, [one, .. many]),
                    (null, var existing) => (null, [.. existing, .. many]),
                    _ => throw new InvalidOperationException()
                };

    private readonly List<string> NewOnly(IEnumerable<string> items)
    {
        var me = this;
        return items.Where(item => !me.Contains(item)).ToList();
    }

    private readonly bool Contains(string item)
        => HasOneItem
            ? item == styles.One
            : styles.Many is not null && styles.Many.Contains(item);

    public readonly Enumerator GetEnumerator()
        => new(this);

    public struct Enumerator(Styles collection)
    {
        private readonly Styles _collection = collection;
        private int _index = -1;

        public bool MoveNext()
        {
            _index++;
            return _index < _collection.Count;
        }

        public readonly string? Current
            => _collection[_index];
    }

    private readonly string?[] ToArray()
      => HasOneItem
          ? [styles.One]
          : styles.Many is null
              ? Array.Empty<string?>()
              : styles.Many.ToArray();

    private sealed class DebuggerProxy(Styles instance)
    {
        private readonly Styles _instance = instance;

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public string?[] Items => _instance.ToArray();
    }

    private readonly string GetDebuggerDisplay()
        => "Count = " + Count;
}
