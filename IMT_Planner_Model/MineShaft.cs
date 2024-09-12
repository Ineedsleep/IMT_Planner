using System.Net.Security;

namespace IMT_Planner_Model;

public class MineShaft
{
    public int ShaftNumber { get; set; }
    public int Level { get; set; } = 0;
    public double OpeningCost { get; set; } = 0;
    public double MaxCost { get; set; } = 0;
    public Element? Element { get; set; }
    public SuperManager? AssignedSM { get; set; }
}