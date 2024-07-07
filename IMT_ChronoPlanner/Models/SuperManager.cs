using System.Net.Security;
using System.Collections.Generic;
using IMT_ChronoPlanner.Models;

namespace IMT_ChronoPlanner;


public class SuperManager
{
    public SuperManager(string name, byte rank, bool promoted, byte level, List<Element> elements, string equipment, double multiplier)
    {
        Name = name;
        Rank = rank;
        Promoted = promoted;
        Level = level;
        Elements = elements;
        Equipment = equipment;
        Multiplier = multiplier;
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
    public string Equipment { get; set; }
    
    public double Multiplier { get; set; }
}


