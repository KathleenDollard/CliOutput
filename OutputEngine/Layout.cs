namespace OutputEngine;

public abstract class Layout
{
    protected Layout(IEnumerable<Section> sections)
    {
        Sections = sections.ToList();
    }

    public List<Section> Sections { get; private set; } = [];

}
