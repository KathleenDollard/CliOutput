namespace CliOutput.Primitives;

// TODO: Determine if using byte decreases the size of the struct
[Flags]
public enum Whitespace : byte 
{
    NeitherBeforeOrAfter = 0,
    Before = 0b0001,
    After = 0b0010,
    BeforeAndAfter = 0b0011,
}
