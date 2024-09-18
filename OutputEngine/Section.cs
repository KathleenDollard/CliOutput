using OutputEngine.Primitives;

namespace OutputEngine;

public class Section : Group
{
    public TextPart Title { get; }

    public Section(string title)
    {
        Title = new TextPart(title, TextPartAppearance.Important);
    }

    //public void WriteSectionHead(Section section)
    //    => WriteLine(section.Title + ":");

}