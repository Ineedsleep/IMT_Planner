namespace IMT_Planner_Model;

public class Passive
{
    public int Id { get; set; } // Primary Key
    public int SuperManagerId { get; set; } // Foreign key
    // Foreign key to PassiveAttributeName
    public int PassiveAttributeNameId { get; set; }
    public double? AttributeValue { get; set; } // Value of the attribute
    public int RankRequirement { get; set; } // Rank requirement for the attribute

    // Navigation property
    public SuperManager SuperManager { get; set; }
    
    // Navigation property to PassiveAttributeName
    public PassiveAttributeName Name { get; set; }
}

public class PassiveAttributeName
{
    public int Id { get; set; } // Primary Key
    public string Abbreviation { get; set; }
    public string Description { get; set; }

    // Navigation property for reverse relationship (not required, but helpful)
    public ICollection<Passive> PassiveAttributes { get; set; }
}