using System.Net.Security;
using System.Collections.Generic;
namespace IMT_ChronoPlanner_Model;


public class SuperManager
{
    public SuperManager(string name, byte rank, bool promoted, byte level, Rarity rarity, Areas area, List<Element> elements, Equipment equipment, double passiveMultiplier)
    {
        Name = name;
        Rank = rank;
        Promoted = promoted;
        Level = level;
        Rarity = rarity;
        Area = area;
        Elements = elements;
        Equipment = equipment;
        PassiveMultiplier = passiveMultiplier;
    }
    public SuperManager(string name)
    {
        Name = name;
    }
    public SuperManager()
    {
    }

    public string Name { get; set; }
    public byte Rank { get; set; }
    public bool Promoted { get; set; }
    public byte Level { get; set; }
    public List<Element> Elements { get; set; }
    public Equipment Equipment { get; set; }
    public double PassiveMultiplier { get; set; }
    public Rarity Rarity { get; set; }
    public Areas Area { get; set; }
    
    
}


