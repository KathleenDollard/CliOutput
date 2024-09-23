namespace OutputEngine.Primitives;

/// <summary>
/// A table that can be rendered.
/// </summary>
public sealed class Table(IReadOnlyList<TableColumn> columns) 
    : BlockElement
{
    public IEnumerable<Paragraph?> GetHeaderRow()
    {
       foreach(var column in Columns)
        {
            yield return column.Header;
        }
    }

    /// <summary>
    /// Gets the table columns. This is set via the constructor and 
    /// cannot be changed, however, columns may be shown or hidden
    /// </summary>
    public IReadOnlyList<TableColumn> Columns { get; } = columns;

    /// <summary>
    /// Gets the table data.
    /// </summary>
    public List<Paragraph[]> TableData { get; } = [];

    /// <summary>
    /// Gets or sets the table title.
    /// </summary>
    public string? Title { get; set; }
    public bool DisplayClosingBar { get; set; }
    public bool IncludeHeaders { get; set; }

    public void AddRow(params Paragraph[] row)
    {
        if (row.Length != Columns.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(row), "The number of row items must match the number of table columns.");
        }
        TableData.Add(row);
    }

    public void AddRow(params string[] row)
    {
        var rowWithTextParts = row.Select(s => new Paragraph(new TextPart(s))).ToArray();
        AddRow(rowWithTextParts);
    }
}
