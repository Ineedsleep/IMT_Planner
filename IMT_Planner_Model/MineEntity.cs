using System.Net.Security;

namespace IMT_Planner_Model;

public class MineEntity
{
    public int EntityNumber { get; set; }
    public Areas Area { get; set; }
    public int Level { get; set; } = 0;
    public double OpeningCost { get; set; } = 0;
    public double MaxCost { get; set; } = 0;
    public Element? Element { get; set; }
    public SuperManager? AssignedSM { get; set; }
    public int MaxLevel => GetMaxLevel();

    private int GetMaxLevel()
    {
        switch (Area)
        {
        case Areas.Elevator:
        case Areas.Warehouse:
            return 3500;
        case Areas.Mineshaft:
            return 800;
        default:
            return 800;
        }
    }
}