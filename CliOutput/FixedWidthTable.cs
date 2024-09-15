using CliOutput.Primitives;

namespace CliOutput;

public class FixedWidthTable
{
    public Table Table { get; }

    public FixedWidthTable(Table table)
    {
        Table = table;
    }

    private WorkingValues workingValues;

    public IEnumerable<string> LayoutTable(object width)
    {
        throw new NotImplementedException();
    }

    private struct WorkingColumnWidth
    {
        internal WorkingColumnWidth(TableColumn column, int position, Table table)
        {
            Column = column;
            // Set initial values
            TentativeWidth = column.MaxWidth;
            DesiredWidth = MaxColumnWidth(position, table, TentativeWidth);
            CurrentWidth = DesiredWidth;
        }


        internal static int MaxColumnWidth(int position, Table table, int trialWidth)
        {
            var temp = Math.Max(table.TableData.Max(row => row[position].PlainWidth(trialWidth)),
                    table.HeaderRow is null ? 0 : table.HeaderRow[position].PlainWidth(trialWidth));
            return table.FooterRow is null
                ? temp
                : Math.Max(temp, table.FooterRow[position].PlainWidth(trialWidth));
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

    public IEnumerable<int> GetAdjustedColumnWidths(int tableWidth)
    {
        var workingWidths = Table.Columns.Select((col, i) => new WorkingColumnWidth(col, i, Table)).ToArray();
        var totalDesired = workingWidths.Sum(col => col.DesiredWidth);

        return totalDesired < tableWidth
            ? WidenFlexibleColumns(workingWidths, tableWidth, totalDesired)
            : totalDesired > tableWidth
                ? ShrinkFlexibleColumns(workingWidths, tableWidth, totalDesired)
                : workingWidths.Select(x => x.CurrentWidth);
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
        var flexibleColumnCount = workingWidths.Length;
        for (int i = 0; i < workingWidths.Length; i++)
        {
            if (workingWidths[i].DesiredWidth <= Table.Columns[i].MinWidth)
            {
                workingWidths[i].FinalWidth = Table.Columns[i].MinWidth;
                workingWidths[i].CurrentWidth = Table.Columns[i].MinWidth;
                flexibleColumnCount--;
            }
        }
        if (flexibleColumnCount > 0)
        {
            // Iterate a few times here, because adding space will encounter new max width restrictions.
            var columnCount = Table.Columns.Count;
            for (int j = 0; j < columnCount; j++)
            {
                var lastFlexibleColumnCount = flexibleColumnCount;
                var totalCurrent = workingWidths.Sum(x => x.FinalWidth > 0 ? x.FinalWidth : x.DesiredWidth);
                var fairAddition = (tableWidth - totalCurrent) / flexibleColumnCount;
                for (int i = 0; i < workingWidths.Length; i++)
                {
                    if (workingWidths[i].FinalWidth == 0)
                    {
                        workingWidths[i].CurrentWidth = workingWidths[i].DesiredWidth + fairAddition;
                        if (workingWidths[i].DesiredWidth <= Table.Columns[i].MinWidth)
                        {
                            workingWidths[i].FinalWidth = Table.Columns[i].MinWidth;
                            workingWidths[i].CurrentWidth = Table.Columns[i].MinWidth;
                            flexibleColumnCount--;
                        }
                    }
                }
                if (flexibleColumnCount == 0 || flexibleColumnCount == lastFlexibleColumnCount)
                {
                    break;
                }
            }
        }
        return workingWidths.Select(x => x.CurrentWidth);
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
