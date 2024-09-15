//using System.Collections;

//namespace CliOutput.Primitives;

///// <summary>
///// Represents a table row.
///// </summary>
//public sealed class TableRow : IEnumerable<TextGroup>
//{
//    private readonly List<TextGroup> _items;

//    internal bool IsHeader { get; }
//    internal bool IsFooter { get; }

//    /// <summary>
//    /// Initializes a new instance of the <see cref="TableRow"/> class.
//    /// </summary>
//    /// <param name="items">The row items.</param>
//    public TableRow(IEnumerable<TextGroup> items)
//        : this(items, false, false)
//    {
//    }
//    private TableRow(IEnumerable<TextGroup> items, bool isHeader, bool isFooter)
//    {
//        _items = new List<TextGroup>(items ?? []);

//        IsHeader = isHeader;
//        IsFooter = isFooter;
//    }

//    /// <summary>
//    /// Gets a row item at the specified table column index.
//    /// </summary>
//    /// <param name="index">The table column index.</param>
//    /// <returns>The row item at the specified table column index.</returns>
//    public TextGroup this[int index]
//    {
//        get => _items[index];
//    }

//    internal static TableRow Header(IEnumerable<TextGroup> items)
//    {
//        return new TableRow(items, true, false);
//    }

//    internal static TableRow Footer(IEnumerable<TextGroup> items)
//    {
//        return new TableRow(items, false, true);
//    }

//    internal void Add(TextGroup item)
//    {
//        ArgumentNullException.ThrowIfNull(item);

//        _items.Add(item);
//    }

//    /// <inheritdoc/>
//    public IEnumerator<TextGroup> GetEnumerator()
//    {
//        return _items.GetEnumerator();
//    }

//    /// <inheritdoc/>
//    IEnumerator IEnumerable.GetEnumerator()
//    {
//        return GetEnumerator();
//    }
//}
