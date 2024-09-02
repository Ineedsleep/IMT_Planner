using System.Linq.Expressions;
using IMT_Planner_Model;
using LinqKit;

namespace IMT_Planner_ViewModels.Models;

public class CardFilterModel
{
    public (int? Min, int? Max) Level { get; set; }
    public (int? Min, int? Max) RankRange { get; set; }
    public int? Rank { get; set; }
   
    // Possible use Enum here
    public ICollection<Areas> Area { get; set; } 
    public ICollection<Rarity> Rarity { get; set; } 
    public ICollection<SuperManagerElement>? Elements { get; set; } 
   public bool Promoted { get; set; }
   public bool? HasPassiveMultiplier { get; set; }
    // More properties...
    
    public Expression<Func<SuperManager, bool>> GetExpression()
    {
        var elementNames = Elements?.Select(e => e.Element.Name).ToList();
        var effectiveness = Elements?.Select(e => e.EffectivenessType).ToList();
        
        var predicate = PredicateBuilder.New<SuperManager>(true);
        if (Level.Min.HasValue)
            predicate = predicate.And(sm => sm.Level >= Level.Min);
        if (Level.Max.HasValue)
            predicate = predicate.And(sm => sm.Level <= Level.Max);
        if (Rank.HasValue)
            predicate = predicate.And(sm => sm.Rank.CurrentRank == Rank.Value);
        if (RankRange.Min.HasValue)
            predicate = predicate.And(sm => sm.Rank.CurrentRank >= RankRange.Min);
        if (RankRange.Max.HasValue)
            predicate = predicate.And(sm => sm.Rank.CurrentRank <= RankRange.Max);
        if (Elements != null)
        {
            predicate = predicate.And(sm => sm.SuperManagerElements != null 
                                            && sm.SuperManagerElements.Any(e => 
                                                elementNames != null 
                                                && elementNames.Contains(e.Element.Name)
                                                && e.RankRequirement <= sm.Rank.CurrentRank)
                                            && sm.SuperManagerElements.Any(e => 
                                                effectiveness != null 
                                                && effectiveness.Contains(e.EffectivenessType)
                                                && e.RankRequirement <= sm.Rank.CurrentRank));
            
        }

        if (HasPassiveMultiplier.HasValue)
        {
            predicate = predicate.And(sm => sm.PassiveMultiplier > 1.00);
        }

        return predicate;
    }
}