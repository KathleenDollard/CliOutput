namespace OutputEngine.Primitives;

public class CodeBlock : BlockElement
{
    public CodeBlock() : base(Styles.CreateWithImplicit(BlockStyle.CodeBlock))
    { }
    public string? Code { get; }
    public string? Language { get; }
}
