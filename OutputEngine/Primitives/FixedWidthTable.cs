//using CellLine = OutputEngine.Primitives.Paragraph;
//using RowLine = System.Collections.Generic.List<OutputEngine.Primitives.Paragraph>;
using Row = System.Collections.Generic.List<System.Collections.Generic.List<string>>;

namespace OutputEngine.Primitives;

public class FixedWidthTable
{
    public Table Table { get; }

    public FixedWidthTable(Table table)
    {
        Table = table;
    }

    private WorkingValues workingValues;

    public IEnumerable<IEnumerable<string>> LayoutTable(int width, bool includeHeaders = false)
    {
        var columnWidths = GetAdjustedColumnWidths(width, includeHeaders).ToArray();
        var rows = GetRows(Table, columnWidths, includeHeaders);
        var tableLines = new List<IEnumerable<string>>();
        foreach (var row in rows)
        {
            var rowLines = new List<string>();
            foreach (var line in row)
            {
                rowLines.Add(string.Join(string.Empty, line));
            }
            tableLines.Add(rowLines);
        }

        return tableLines;

        static IEnumerable<Row> GetRows(Table table, int[] columnWidths, bool includeHeaders)
        {
            var returnRows = new List<Row>();
            if (includeHeaders)
            {
                var header = table.GetHeaderRow();
                var wrappedHeader = header.Select((cell, col)
                    => cell is null
                        ? []
                        : cell.PlainOutput(columnWidths[col]).ToArray());
                var headerRows = LineupRow(wrappedHeader, columnWidths);
                returnRows.AddRange(headerRows);
            }

            foreach (var row in table.TableData)
            {
                var wrappedRow = row.Select((cell, col)
                    => cell is null
                        ? []
                        : cell.PlainOutput(columnWidths[col]).ToArray());
                var dataRow = LineupRow(wrappedRow, columnWidths);
                returnRows.AddRange(dataRow);
            }

            return returnRows;
        }

        static Row LineupRow(IEnumerable<string[]> byColumn, int[] columnWidths)
        {
            var maxLineCount = byColumn.Max(x => x.Count());
            var returnRows = new Row();
            for (var i = 0; i < maxLineCount; i++)
            {
                var line = new List<string>();
                for (var j = 0; j < byColumn.Count(); j++)
                {
                    var cell = byColumn.ElementAt(j);
                    var cellLineText = i >= cell.Length
                        ? string.Empty
                        : cell switch
                        {
                            null => string.Empty,
                            _ => cell[i] ?? string.Empty
                        };
                    line.Add(cellLineText.PadRight(columnWidths[j]));
                }
                returnRows.Add(line);
            }
            return returnRows;
        }
    }

    private struct WorkingColumnWidth
    {
        internal WorkingColumnWidth(TableColumn column, int position, Table table, bool includeHeaders)
        {
            Column = column;
            // Set initial values
            TentativeWidth = column.MaxWidth;
            DesiredWidth = MaxColumnWidth(position, table, TentativeWidth, includeHeaders);
            CurrentWidth = DesiredWidth;
        }


        internal static int MaxColumnWidth(int position, Table table, int trialWidth, bool includeHeaders)
        {
            var headerWidth = !includeHeaders
                                ? 0
                                : HeaderWidth(table.GetHeaderRow()?.ElementAt(position), trialWidth);
            return Math.Max(table.TableData.Max(row => row[position].PlainWidth(trialWidth)),
                                headerWidth);

            static int HeaderWidth(Paragraph? header, int trialWidth)
                => header is null
                    ? 0
                    : header.PlainWidth(trialWidth);
        }

        internal TableColumn Column { get; }
        internal int DesiredWidth { get; set; }
        internal int FairWidth { get; set; }
        internal int TentativeWidth { get; set; }
        internal int CurrentWidth { get; set; }
        internal int FinalWidth { get; set; }
    }

    private struct WorkingValues
    {
        internal int TotalDesired { get; set; }
        internal int TotalFinal { get; set; }
        internal int FlexibleColumnCount { get; set; }
        internal int ExtraSpace { get; set; }
    }

    public IEnumerable<int> GetAdjustedColumnWidths(int tableWidth, bool includeHeaders)
    {
        var workingWidths = Table.Columns.Select((col, i) => new WorkingColumnWidth(col, i, Table, includeHeaders)).ToArray();
        var totalDesired = workingWidths.Sum(col => col.DesiredWidth);

        return totalDesired < tableWidth
            ? WidenFlexibleColumns(workingWidths, totalDesired, tableWidth)
            : totalDesired > tableWidth
                ? ShrinkFlexibleColumns(workingWidths, totalDesired, tableWidth)
                : workingWidths.Select(x => x.CurrentWidth).ToArray();
    }

    private IEnumerable<int> WidenFlexibleColumns(WorkingColumnWidth[] workingWidths, int totalDesired, int tableWidth)
    {
        // Determine inflexible columns
        var flexibleColumnCount = workingWidths.Length;
        for (int i = 0; i < workingWidths.Length; i++)
        {
            // Can't shrink it further
            if (workingWidths[i].CurrentWidth >= Table.Columns[i].MaxWidth)
            {
                workingWidths[i].FinalWidth = Table.Columns[i].MaxWidth;
                workingWidths[i].CurrentWidth = Table.Columns[i].MaxWidth;
                flexibleColumnCount--;
            }
        }
        var iterationCount = 0;
        // We should iterate, but protect from bouncing between two valid states. It seems like it would never
        // require more iterations than there are columns, which also keeps the iteration count low.
        while (flexibleColumnCount > 0 && iterationCount <= Table.Columns.Count)
        {
            iterationCount++;
            var totalCurrent = workingWidths.Sum(x => x.CurrentWidth);
            var fairAddition = (tableWidth - totalCurrent) / flexibleColumnCount;
            for (int i = 0; i < workingWidths.Length; i++)
            {
                if (workingWidths[i].FinalWidth == 0)
                {
                    workingWidths[i].CurrentWidth = workingWidths[i].DesiredWidth + fairAddition;
                    if (workingWidths[i].CurrentWidth >= Table.Columns[i].MaxWidth)
                    {
                        workingWidths[i].FinalWidth = Table.Columns[i].MaxWidth;
                        workingWidths[i].CurrentWidth = Table.Columns[i].MaxWidth;
                        flexibleColumnCount--;
                    }
                }
            }
            if (flexibleColumnCount == 0)
            {
                break;
            }
        }
        return workingWidths.Select(x => x.CurrentWidth);
    }


    //private IEnumerable<int> WidenFlexibleColumns(WorkingColumnWidth[] workingWidths, int totalDesired, int tableWidth)
    //{
    //    // Determine inflexible columns
    //    var flexibleColumnCount = workingWidths.Length;
    //    for (int i = 0; i < workingWidths.Length; i++)
    //    {
    //        if (workingWidths[i].DesiredWidth >= Columns[i].MaxWidth)
    //        {
    //            workingWidths[i].FinalWidth = Columns[i].MaxWidth;
    //            workingWidths[i].CurrentWidth = Columns[i].MaxWidth;
    //            flexibleColumnCount--;
    //        }
    //    }
    //    if (flexibleColumnCount > 0)
    //    {
    //        // Iterate a few times here, because adding space will encounter new max width restrictions.
    //        var columnCount = Columns.Count;
    //        for (int j = 0; j < columnCount; j++)
    //        {
    //            var lastFlexibleColumnCount = flexibleColumnCount;
    //            var totalCurrent = workingWidths.Sum(x => x.FinalWidth > 0 ? x.FinalWidth : x.DesiredWidth);
    //            var fairAddition = (tableWidth - totalCurrent) / flexibleColumnCount;
    //            for (int i = 0; i < workingWidths.Length; i++)
    //            {
    //                if (workingWidths[i].FinalWidth == 0)
    //                {
    //                    workingWidths[i].CurrentWidth = workingWidths[i].DesiredWidth + fairAddition;
    //                    if (workingWidths[i].CurrentWidth >= Columns[i].MaxWidth)
    //                    {
    //                        workingWidths[i].FinalWidth = Columns[i].MaxWidth;
    //                        workingWidths[i].CurrentWidth = Columns[i].MaxWidth;
    //                        flexibleColumnCount--;
    //                    }
    //                }
    //            }
    //            if (flexibleColumnCount == 0 || flexibleColumnCount == lastFlexibleColumnCount)
    //            {
    //                break;
    //            }
    //        }
    //    }
    //    return workingWidths.Select(x => x.CurrentWidth);
    //}

    private IEnumerable<int> ShrinkFlexibleColumns(WorkingColumnWidth[] workingWidths, int totalDesired, int tableWidth)
    {
        // Determine inflexible columns
        for (int i = 0; i < workingWidths.Length; i++)
        {
            if (workingWidths[i].DesiredWidth <= Table.Columns[i].MinWidth)
            {
                workingWidths[i].FinalWidth = Table.Columns[i].MinWidth;
                workingWidths[i].CurrentWidth = Table.Columns[i].MinWidth;
            }
        }
        var flexibleColumnCount = workingWidths.Count(x => x.FinalWidth == 0);
        if (flexibleColumnCount > 0)
        {
            // TODO: Iterate a few times here, because adding space will encounter new max width restrictions. Arbitrary number of iterations.
            for (int j = 0; j < 5; j++)
            {
                var lastFlexibleColumnCount = flexibleColumnCount;
                CalculateFairAddition(workingWidths, totalDesired, tableWidth, ref flexibleColumnCount);

                if (flexibleColumnCount == 0 || flexibleColumnCount == lastFlexibleColumnCount)
                {
                    break;
                }
            }
        }
        return workingWidths.Select(x => x.FinalWidth > 0 ? x.FinalWidth : x.FairWidth);
    }


    private void CalculateFairAddition(WorkingColumnWidth[] workingWidths, int totalDesired, int tableWidth, ref int flexibleColumnCount)
    {
        var trialAdjustment = SetMinMaxAndGetTrialAdjustment(workingWidths, Table, tableWidth, ref flexibleColumnCount);
        AdjustFlexibleColumns(workingWidths, trialAdjustment);

        static void AdjustFlexibleColumns(WorkingColumnWidth[] workingWidths, int trialAdjustment)
        {
            for (int i = 0; i < workingWidths.Length; i++)
            {
                if (workingWidths[i].FinalWidth == 0)
                {
                    workingWidths[i].FairWidth = workingWidths[i].DesiredWidth - trialAdjustment;
                    workingWidths[i].DesiredWidth = workingWidths[i].FairWidth;
                }
            }
        }

        static int SetMinMaxAndGetTrialAdjustment(WorkingColumnWidth[] workingWidths, Table table, int tableWidth, ref int flexibleColumnCount)
        {
            var trialAdjustment = GetTrialAdjustment(workingWidths, tableWidth, flexibleColumnCount);
            for (int i = 0; i < workingWidths.Length; i++)
            {
                if (workingWidths[i].FinalWidth > 0)
                {
                    continue;
                }
                workingWidths[i].FairWidth = workingWidths[i].DesiredWidth - trialAdjustment;
                if (workingWidths[i].FairWidth <= table.Columns[i].MinWidth)
                {
                    workingWidths[i].FinalWidth = table.Columns[i].MinWidth;
                    flexibleColumnCount--;
                    trialAdjustment = GetTrialAdjustment(workingWidths, tableWidth, flexibleColumnCount);
                }
            }
            return trialAdjustment;
        }

        static int GetTrialAdjustment(WorkingColumnWidth[] workingWidths, int tableWidth, int flexibleColumnCount)
            => flexibleColumnCount == 0
                    ? 0
                    : (workingWidths.Sum(x => x.FinalWidth > 0 ? x.FinalWidth : x.DesiredWidth) - tableWidth) / flexibleColumnCount;

    }

    //var workingValues = new WorkingValues();

    //TentativeCalculation(ref workingWidths, ref workingValues);
    //if (workingValues.FlexibleColumnCount == 0)
    //{
    //    return workingWidths.Select(x => x.FinalWidth);
    //}
    //if (workingValues.ExtraSpace == 0)


    //return workingWidths.Select(x => x.FinalWidth);

    private void SetDesiredWidth(ref WorkingColumnWidth[] workingWidths)
    {
        for (int i = 0; i < Table.Columns.Count; i++)
        {
            if (workingWidths[i].FinalWidth != 0)
            {
                workingValues.TotalFinal += workingWidths[i].FinalWidth;
                workingValues.FlexibleColumnCount--;
                continue;
            }
            var tentativeWidth = workingWidths[i].TentativeWidth;
            // Wrapping may result in this being smaller than the tentativeWidth
            var maxCurrent = Table.TableData.Max(row => row[i].PlainWidth(tentativeWidth));
            workingWidths[i].DesiredWidth = int.Max(maxCurrent, Table.Columns[i].MinWidth);
            workingWidths[i].DesiredWidth = int.Max(maxCurrent, Table.Columns[i].MinWidth);
            if (workingWidths[i].DesiredWidth <= Table.Columns[i].MinWidth)
            {
                workingWidths[i].FinalWidth = int.Max(maxCurrent, Table.Columns[i].MinWidth);
                workingValues.TotalFinal += workingWidths[i].FinalWidth;
                workingValues.FlexibleColumnCount--;
            }
            workingValues.ExtraSpace += tentativeWidth - maxCurrent;
            workingValues.TotalDesired += workingWidths[i].DesiredWidth;
        }
    }


    //private void TentativeCalculation(ref WorkingColumnWidth[] workingWidths, ref WorkingValues workingValues)
    //{
    //    for (int i = 0; i < Columns.Count; i++)
    //    {
    //        if (workingWidths[i].FinalWidth != 0)
    //        {
    //            workingValues.TotalFinal += workingWidths[i].FinalWidth;
    //            workingValues.FlexibleColumnCount--;
    //            continue;
    //        }
    //        var tentativeWidth = workingWidths[i].TentativeWidth;
    //        // Wrapping may result in this being smaller than the tentativeWidth
    //        var maxCurrent = Rows.Max(row => row[i].CalculateWidth(tentativeWidth));
    //        workingWidths[i].DesiredWidth = int.Max(maxCurrent, Columns[i].MinWidth);
    //        workingWidths[i].DesiredWidth = int.Max(maxCurrent, Columns[i].MinWidth);
    //        if (workingWidths[i].DesiredWidth <= Columns[i].MinWidth)
    //        {
    //            workingWidths[i].FinalWidth = int.Max(maxCurrent, Columns[i].MinWidth);
    //            workingValues.TotalFinal += workingWidths[i].FinalWidth;
    //            workingValues.FlexibleColumnCount--;
    //        }
    //        workingValues.ExtraSpace += tentativeWidth - maxCurrent;
    //        workingValues.TotalDesired += workingWidths[i].DesiredWidth;
    //    }
    //}
}
