namespace OutputEngine.Primitives;

public class CodeBlock : BlockElement
{
    public CodeBlock() : base()
    { }
    public string? Code { get; }
    public string? Language { get; }
}
