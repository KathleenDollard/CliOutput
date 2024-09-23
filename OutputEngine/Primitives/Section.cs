namespace OutputEngine.Primitives;

public class Section : Group
{
    public Section(string heading)
    {
        Heading = new Paragraph(heading)
        {
            Appearance = Appearance.SectionHeading,
            NoNewLineAfter = true
        };
    }

    public Paragraph Heading { get; }


}