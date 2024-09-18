using OutputEngine.Primitives;

namespace OutputEngine.Targets;

public class Terminal : CliOutput
{
    public Terminal(OutputContext outputContext)
        : base(outputContext)
    {  }

    public override void WriteLine()
    {
        Write(Environment.NewLine);
    }

    public override void Write(Section section, int indentCount = 0)
    {
        Write(section.Title);
        WriteLine(":");
        Write((Group)section, 1);
    }
}
