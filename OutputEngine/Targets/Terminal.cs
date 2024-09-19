using OutputEngine.Primitives;

namespace OutputEngine.Targets;

public class Terminal : CliOutput
{
    public Terminal(OutputContext outputContext)
        : base(outputContext)
    {  
    OutputStyles = outputContext.OutputStyles ?? new OutputStyles();
    }

    public override void WriteLine()
    {
        Write(Environment.NewLine);
    }

    public override void Write(Section section, int indentCount = 0)
    {
        base.Write(section, indentCount);
        Write(Environment.NewLine);
    }
}
