using System.Net.Security;
using System.Collections.Generic;
namespace IMT_ChronoPlanner_Model;


public class SuperManager
{
    public SuperManager(string name, byte rank, bool promoted, byte level, Rarity rarity, Areas area, List<Element> elements, Equipment equipment, double passiveMultiplier, ICollection<SuperManagerElement> superManagerElements)
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
        SuperManagerElements = superManagerElements;
    }
    public SuperManager(string name, ICollection<SuperManagerElement> superManagerElements)
    {
        Name = name;
        SuperManagerElements = superManagerElements;
    }
    public SuperManager(ICollection<SuperManagerElement> superManagerElements)
    {
        SuperManagerElements = superManagerElements;
    }
    
    public SuperManager(string name, byte rank, bool promoted, byte level, Rarity rarity, Areas area,
        List<Element> elements, Equipment equipment, double passiveMultiplier, byte priority, ICollection<SuperManagerElement> superManagerElements)
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
        Priority = priority;
        SuperManagerElements = superManagerElements;
    }

    public SuperManager()
    {
    }

    public int SuperManagerId { get; set; }
    // Other properties...

    // Navigation property
    public ICollection<SuperManagerElement> SuperManagerElements { get; set; }
    public string Name { get; set; }
    public byte Rank { get; set; }
    public bool Promoted { get; set; }
    public byte Level { get; set; }
    public List<Element> Elements { get; set; }
    public Equipment Equipment { get; set; }
    public double PassiveMultiplier { get; set; }
    public Rarity Rarity { get; set; }
    public Areas Area { get; set; }
    
    public byte Priority { get; set; }
    
}


