namespace OutputEngine.Primitives;

/// <summary>
/// Represents a table column.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="TableColumn"/> class.
/// </remarks>
/// <param name="header">The <see cref="string"/> instance to use as the table column header.</param>
public sealed class TableColumn(Paragraph header,
                                TableColumnKind columnKind = TableColumnKind.Default,
                                TableColumnAlignment alignment = TableColumnAlignment.Left,
                                byte maxWidth = TableColumn.maxAllowedColumnWidth,
                                byte minWidth = 10)
{
    internal const int maxAllowedColumnWidth = byte.MaxValue;

    public TableColumn(string header,
                       TableColumnKind columnKind = TableColumnKind.Default,
                       TableColumnAlignment alignment = TableColumnAlignment.Left,
                       byte maxWidth = maxAllowedColumnWidth,
                       byte minWidth = 15)
        : this(new Paragraph { new TextPart(header) }, columnKind, alignment, maxWidth, minWidth)
    { }

    /// <summary>
    /// Gets the column header.
    /// </summary>
    public Paragraph? Header { get; } = header;

    /// <summary>
    /// Gets or sets the column footer.
    /// </summary>
    public Paragraph? Footer { get; set; }

    public TableColumnKind ColumnKind { get; set; } = columnKind;
    public TableColumnAlignment Alignment { get; set; } = alignment;
    public byte MaxWidth { get; set; } = maxWidth;
    public byte MinWidth { get; set; } = minWidth;
    public bool Hide { get; set; }
}
