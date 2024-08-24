using System.Net.Security;

namespace IMT_Planner_Model;

public class MineEntity
{

    public MineEntity(int shaftNumber, int level, double openingCost, double maxCost, Element element, SuperManager superManager)
    {
        ShaftNumber = shaftNumber;
        Level = level;
        OpeningCost = openingCost;
        MaxCost = maxCost;
        Element = element;
        AssignedSM = superManager;
    }
    
    public int ShaftNumber { get; set; }
    public int Level { get; set; }
    public double OpeningCost { get; set; }
    public double MaxCost { get; set; }
    public Element Element { get; set; }
    public SuperManager AssignedSM { get; set; }
    
    
}