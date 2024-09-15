//using CliOutput.Primitives;

//namespace CliOutput;

//public static class Cli
//{
//    private const int uxLevelEnvDefault = 4;

//    public static int Render(Table table,
//                             OutputTarget renderTo = OutputTarget.Unknown,
//                             int uxLevel = 0,
//                             string? outputFile = null,
//                             params string[] columns)
//    {
//        renderTo = renderTo == OutputTarget.Unknown
//                    ? GetRenderContext(outputFile)
//                    : renderTo;
//        uxLevel = uxLevel == 0 ? GetUxLevel() : uxLevel;
//        AdjustColumns(table, columns);
//        switch (renderTo)
//        {
//            case OutputTarget.Terminal:
//                return PlainHelpTerminal.Render( table);
//            case OutputTarget.Json:
//                var output = Json.Render( table);
//                return string.IsNullOrWhiteSpace(outputFile)
//                    ? PlainHelpTerminal.RenderPlainText( output)
//                    : OutputToFile(output, outputFile);
//            default:
//                throw new InvalidOperationException();
//        }
//    }

//    public static int Render(Help help,
//                             OutputTarget renderTo = OutputTarget.Unknown,
//                             int uxLevel = 0,
//                             string? outputFile = null)
//    {
//        renderTo = renderTo == OutputTarget.Unknown
//                                    ? GetRenderContext(outputFile)
//                                    : renderTo;
//        uxLevel = uxLevel == 0 ? GetUxLevel() : uxLevel;
//        switch (renderTo)
//        {
//            case OutputTarget.Terminal:
//                return PlainHelpTerminal.Render( help);
//            case OutputTarget.Json:
//                var output = Json.Render( help);
//                return string.IsNullOrWhiteSpace(outputFile)
//                    ? PlainHelpTerminal.RenderPlainText( output)
//                    : OutputToFile(output, outputFile);
//            default:
//                throw new InvalidOperationException();
//        }
//    }

//    private static int OutputToFile(string output, string outputFile)
//    {
//        throw new NotImplementedException();
//    }

//    private static void AdjustColumns(Table table, string[] columns)
//    {
//        if (columns is null || !columns.Any())
//        {
//            foreach (var tableColumn in table.Columns.Where(col => col.ColumnType != TableColumnType.Default
//                                                        && col.ColumnType != TableColumnType.Mandatory))
//            {
//                tableColumn.Hide = true;
//            }
//            return;
//        }
//        foreach (var tableColumn in table.Columns.Where(col => col.ColumnType != TableColumnType.Mandatory
//                                        && !columns.Contains(col.Header)))
//        {
//            tableColumn.Hide = true;
//        }
//    }

//    private static int GetUxLevel()
//    {
//        var uxLevelEnv = Environment.GetEnvironmentVariable("DOTNETCLI_UXLEVEL");
//        if (int.TryParse(uxLevelEnv, out var result))
//        {
//            return result;
//        }
//        return uxLevelEnvDefault;
//    }

//    private static OutputTarget GetRenderContext(string? renderToFile)
//    {
//        return string.IsNullOrWhiteSpace(renderToFile)
//                ? OutputTarget.Terminal
//                : RenderContextFromFileName(renderToFile);

//        static OutputTarget RenderContextFromFileName(string renderToFile)
//        {
//            var ext = Path.GetExtension(renderToFile);
//            return ext.Equals("JSON", StringComparison.OrdinalIgnoreCase)
//                      ? OutputTarget.Json
//                      : OutputTarget.Terminal; // just output what is displayed
//        }
//    }
//}
