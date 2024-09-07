namespace IMT_Planner_Model;

public class Passives
{
    public int Id { get; set; } // Primary Key
    // Foreign key
    public int SuperManagerId { get; set; }

    // Navigation property
    public SuperManager SuperManager { get; set; }
    
    /// <summary>
    /// Will be true if MIF or CIF is set
    /// </summary>
    public bool HasMif { get; set; }
    public double MineIncomeFactor { get; set; }
    public bool HasCif { get; set; }
    public double? ContinentIncomeFactor { get; set; }
    
    

    /// <summary>
    /// Level costs of shaft are reduced
    /// </summary>
    public bool HasCostReduction { get; set; }
    public double? CostReduction { get; set; }
    /// <summary>
    /// Unlock costs for the next shaft is reduced
    /// </summary>
    public bool HasShaftUnlockReduction { get; set; }
    public double? ShaftUnlockReduction { get; set; }
    
    //Add more Passives if necessary
}