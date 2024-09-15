namespace CliOutput.Primitives;

/// <summary>
/// A renderable table.
/// </summary>
public sealed class Table
{
    private IReadOnlyList<TextGroup>? headerRow;
    private IReadOnlyList<TextGroup>? footerRow;

    public Table(IReadOnlyList<TableColumn> columns)
    {
        Columns = columns;
    }

    public IReadOnlyList<TextGroup>? HeaderRow
    {
        get => headerRow; 
        set
        {
        if (value is { } &&  value.Count() != Columns.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(HeaderRow), "The number of header columns must match the number of table columns.");
        }
            headerRow = value;
        }
    }

    public IReadOnlyList<TextGroup>? FooterRow
    {
        get => footerRow;
        set
        {
            if (value is { } && value.Count() != Columns.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(FooterRow), "The number of footer columns must match the number of table columns.");
            }
            footerRow = value;
        }
    }


    /// <summary>
    /// Gets the table columns. This is set via the constructor and 
    /// cannot be changed, however, columns may be shown or hidden
    /// </summary>
    public IReadOnlyList<TableColumn> Columns { get; }

    /// <summary>
    /// Gets the table data.
    /// </summary>
    public List<TextGroup[]> TableData { get; } = new();

    /// <summary>
    /// Gets or sets the table title.
    /// </summary>
    public string? Title { get; set; }
    public bool DisplayClosingBar { get; set; }

    public void AddRow(params TextGroup[] row)
    {
        if (row.Count() != Columns.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(row), "The number of row items must match the number of table columns.");
        }
        TableData.Add(row);
    }

    public void AddRow(params string[] row)
    {
        if (row.Count() != Columns.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(row), "The number of row items must match the number of table columns.");
        }
        TableData.Add(row.Select(s=> (TextGroup)s).ToArray());
    }

}
