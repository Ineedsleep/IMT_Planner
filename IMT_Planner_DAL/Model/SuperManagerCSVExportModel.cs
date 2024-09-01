using IMT_Planner_Model;

namespace IMT_Planner_DAL.Model;

public class SuperManagerCSVExportModel
{
    public string SuperManagerName { get; set; }
    public Rarity SuperManagerRarity { get; set; }
    public Areas SuperManagerArea { get; set; }
    public int CurrentRank { get; set; }
    public byte Level { get; set; }
    public bool Promoted { get; set; }
    public string Group { get; set; }
    public string Elements { get; set; }
    public Rarity Rarity { get; set; }
}