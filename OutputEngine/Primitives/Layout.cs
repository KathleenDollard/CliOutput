using System.Reflection;

namespace OutputEngine.Primitives;

public abstract class Layout
{
    protected Layout(IEnumerable<Section> sections, Title? title = null)
    {
        Sections = sections.ToList();
        Title = title;
    }

    public Title? Title { get;  }
    public List<Section> Sections { get; private set; } = [];

}
