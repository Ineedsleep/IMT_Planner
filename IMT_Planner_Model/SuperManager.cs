using System.Net.Security;
using System.Collections.Generic;
namespace IMT_Planner_Model;


public class SuperManager
{
    public SuperManager(string? name, Rank? rank, int promoted
        ,Rarity rarity, Areas area)
    {
        Name = name;
        Rank = rank;
        Promoted = promoted;
        Rarity = rarity;
        Area = area;
    }
    public SuperManager(string? name, ICollection<SuperManagerElement>? superManagerElements)
    {
        Name = name;
        SuperManagerElements = superManagerElements;
    }
    public SuperManager(ICollection<SuperManagerElement>? superManagerElements)
    {
        SuperManagerElements = superManagerElements;
    }
    
    public SuperManager()
    {
    }

    public int SuperManagerId { get; set; }
    public ICollection<SuperManagerElement>? SuperManagerElements { get; set; }
    public string? Name { get; set; }
    public Rank? Rank { get; set; }
    public int Promoted { get; set; }
    public byte Level { get; set; }
    public int CurrentFragments { get; set; }
  //  public Equipment Equipment { get; set; }
    public ICollection<Passive> Passives { get; set; } = new List<Passive>();
    public Rarity Rarity { get; set; }
    public Areas Area { get; set; }
 
    public byte Priority { get; set; }
    /// <summary>
    /// If this is true the first initial 30 frags are collected and the sm can be crafted
    /// </summary>
    public bool Unlocked { get; set; }
    
}


