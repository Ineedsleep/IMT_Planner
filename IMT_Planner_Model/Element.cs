namespace IMT_Planner_Model;

public class Element
{
    public int ElementId { get; set; }
    public string Name { get; set; }

    // Navigation property
    public ICollection<SuperManagerElement> SuperManagerElements { get; set; }
}

public class SuperManagerElement
{
    public int SuperManagerId { get; set; }
    public SuperManager SuperManager { get; set; }
    public int ElementId { get; set; }
    public Element Element { get; set; }
    public int RankRequirement { get; set; }
    public string EffectivenessType { get; set; }
    
}