// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

// Based on `OneOrMany` at https://github.com/dotnet/roslyn and simplified to skip LINQ semantics

using System.Diagnostics;
using StyleTuple = (string? One, System.Collections.Generic.List<string>? Many);

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
    private string[]? implicitStyles;
    private StyleTuple styles;

    /// <summary>
    /// Holds implicit styles so that they can be reset. Alternatively, implicit styles could be held
    /// in <see cref="Element"/>, a ResetStyles method placed on Element and the Reset method removed here.
    /// </summary>
    /// <param name="implicitStyles"></param>
    /// <returns></returns>
    internal static Styles CreateWithImplicit(params string[] implicitStyles)
    {
        var newStyles = new Styles
        {
            implicitStyles = implicitStyles
        };
        if (implicitStyles.Length > 0)
        {
            newStyles.AddRange(implicitStyles);
        }
        return newStyles;
    }

    public Styles(string item)
    {
        styles = (item, null);
    }

    public Styles(params string[] items)
    {
        ArgumentNullException.ThrowIfNull(items);

        styles = AddRangeAndReturnTuple(items);
    }

    private readonly bool HasOneItem
        => styles.One is not null;

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

    public readonly int Count
        => HasOneItem
                ? 1
                : styles.Many is null
                    ? 0
                    : styles.Many.Count;

    internal Styles Add(string item)
    {
        StyleTuple value = Contains(item)
                        ? styles
                        : AddAndReturnTuple(item);
        return value;
    }

    internal Styles Reset()
        => CreateWithImplicit(implicitStyles);

    internal void AddRange(IEnumerable<string> items)
        => styles = NewOnly(items) switch
        {
            null or [] => styles,
            [var item] => AddAndReturnTuple(item),
            var many => AddRangeAndReturnTuple(many)
        };

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

    public readonly bool Contains(string item)
        => HasOneItem
            ? item == styles.One
            : styles.Many is not null && styles.Many.Contains(item);

    private readonly string?[] ToArray()
        => HasOneItem
            ? [styles.One]
            : styles.Many is null
                ? Array.Empty<string?>()
                : styles.Many.ToArray();

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

    private sealed class DebuggerProxy(Styles instance)
    {
        private readonly Styles _instance = instance;

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public string?[] Items => _instance.ToArray();
    }

    private readonly string GetDebuggerDisplay()
        => "Count = " + Count;
}
