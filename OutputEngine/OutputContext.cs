using System.Text;

namespace OutputEngine;

public class OutputContext
{
    public OutputContext(bool shouldRedirect = false,
                         int width = 80,
                         int indentSize = 2)
    {
        ShouldRedirect = shouldRedirect;
        Width = width;
        IndentSize = indentSize;
    }

    public bool ShouldRedirect { get; set; } = false;


    public OutputStyles? OutputStyles { get; set; } = null;
    public Encoding Encoding { get; set; } = Encoding.UTF8;
    public int IndentSize { get; set; }
    public int Width { get; set; }
}
