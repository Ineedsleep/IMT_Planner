namespace IMT_Planner_Model;

public class Element
{
    public Element(string name)
    {
        Name = name;
    }

    public Element()
    {
    }

    public int ElementId { get; set; }
    public string Name { get; set; }

    // Navigation property
    public ICollection<SuperManagerElement> SuperManagerElements { get; set; }
}

public class SuperManagerElement
{
    public SuperManagerElement(SuperManager manager, Element element, string effectivenessType)
    {
        SuperManager = manager;
        Element = element;
        EffectivenessType = effectivenessType;
    }

    public int SuperManagerId { get; set; }
    public SuperManager SuperManager { get; set; }
    public int ElementId { get; set; }
    public Element Element { get; set; }

    public string EffectivenessType { get; set; }
}