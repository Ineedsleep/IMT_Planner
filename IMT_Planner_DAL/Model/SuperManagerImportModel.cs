using IMT_Planner_Model;

namespace IMT_Planner_DAL.Model;

public class SuperManagerImportModel
{
    public string SuperManagerName { get; set; }
    public Rarity Rarity { get; set; }
    public Areas SuperManagerArea { get; set; }
    public int CurrentRank { get; set; }
    public byte Level { get; set; }
    public int Promoted { get; set; }
    public string Group { get; set; }
    public string Elements { get; set; }
    public string Passives { get; set; }
    public bool Unlocked { get; set; }
}//Abbreviation1,10.5,2;Abbreviation2,null,3