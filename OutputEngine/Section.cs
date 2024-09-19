using OutputEngine.Primitives;

namespace OutputEngine;

public class Section : Group
{
    public TextPart Title { get; }

    public Section(string title)
    {
        Title = new TextPart(title, ParagraphAppearance.SectionTitle);
    }

    //public void WriteSectionHead(Section section)
    //    => WriteLine(section.Title + ":");

}