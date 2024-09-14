using System.DirectoryServices.ActiveDirectory;
using System.Linq.Expressions;
using IMT_Planner_Model;
using IMT_Planner_ViewModels.GeneralViewModels;
using LinqKit;

namespace IMT_Planner_ViewModels.Models;

public class CardFilterModel
{
    private Areas? _area;
    private Rarity? _rarity;
    private string? _effectiveness;
    
    public int? LevelMin { get; set; }

    public int? LevelMax { get; set; }

    public int? RankRangeMin { get; set; }

    public int? RankRangeMax { get; set; }

    public Areas? Area
    {
        get => _area;
        set
        {
            if (!Equals(_area, value))
            {
                _area = value;
            }
        }
    }

    public Rarity? Rarity
    {
        get => _rarity;
        set
        {
            if (!Equals(_rarity, value))
            {
                _rarity = value;
            }
        }
    }

    public int? Promoted { get; set; }

    public bool? HasIncomeFactor { get; set; }

    public double? PassiveMultiplier { get; set; }

    public bool? HasCostReduction { get; set; }

    public double? CRValue { get; set; }


    public bool? HasShaftUnlockReduction { get; set; }

    public double? ShaftUnlockReduction { get; set; }

    public Expression<Func<SuperManager, bool>> GetExpression()
    {
        var elementNames = SelectedElement?.ElementName; //Elements?.Select(e => e.Element.Name).ToList();
        var selectedTags =    Tags?.Where(obj => obj.Active)
            .Select(obj => obj.Name)
            .ToList(); //Elements?.Select(e => e.Element.Name).ToList();

        var predicate = PredicateBuilder.New<SuperManager>(true);
        if (LevelMin.HasValue)
            predicate = predicate.And(sm => sm.Level >= LevelMin);
        if (LevelMax.HasValue)
            predicate = predicate.And(sm => sm.Level <= LevelMax);
        if (RankRangeMin.HasValue)
            predicate = predicate.And(sm => sm.Rank != null && sm.Rank.CurrentRank >= RankRangeMin);
        if (RankRangeMax.HasValue)
            predicate = predicate.And(sm => sm.Rank != null && sm.Rank.CurrentRank <= RankRangeMax);
        if (Area.HasValue)
            predicate = predicate.And(sm => sm.Area == Area.Value);
        if (Rarity.HasValue)
            predicate = predicate.And(sm => sm.Rarity == Rarity.Value);
        if (SelectedElement != null && Effectiveness != null)
        {
            predicate = predicate.And(manager => manager.SuperManagerElements != null
                                                 && manager.SuperManagerElements
                                                     .Any(x => elementNames.Contains(x.Element.Name)
                                                               && Effectiveness.Contains(x.EffectivenessType)
                                                               && x.RankRequirement <= manager.Rank.CurrentRank));
        }
        
        if (Tags != null && Tags.Select(x => x.Active == true).Any())
        {
            predicate = predicate.And(sm => sm.Tags
                .Split(";",StringSplitOptions.RemoveEmptyEntries)
                .Any(tag => selectedTags != null && selectedTags.Contains(tag)));
        }
        if (HasIncomeFactor ?? false)
        {
            predicate = predicate.And(sm => sm.Passives.Any(p =>
                (p.Name.Id == 1 && p.PromoRequirement <= sm.Promoted) ||
                (p.Name.Id == 2 && p.PromoRequirement <= sm.Promoted)));
        }

        if (PassiveMultiplier.HasValue && HasIncomeFactor != null && HasIncomeFactor.Value)
        {
            predicate = predicate.And(sm => sm.Passives.Any(p => p.AttributeValue >= PassiveMultiplier));
        }

        if (HasCostReduction ?? false)
        {
            predicate = predicate.And(sm => sm.Passives.Any(p => p.Name.Id == 3 && p.PromoRequirement <= sm.Promoted));
        }

        if (CRValue.HasValue && HasCostReduction != null && HasCostReduction.Value)
        {
            predicate = predicate.And(sm => sm.Passives.Any(p => p.AttributeValue >= CRValue));
        }

        if (HasShaftUnlockReduction ?? false)
        {
            predicate = predicate.And(sm => sm.Passives.Any(p => p.Name.Id == 4 && p.PromoRequirement <= sm.Promoted));
        }

        if (ShaftUnlockReduction.HasValue && HasShaftUnlockReduction != null && HasShaftUnlockReduction.Value)
        {
            predicate = predicate.And(sm => sm.Passives.Any(p => p.AttributeValue >= ShaftUnlockReduction));
        }


        return predicate;
    }


    public IEnumerable<Rarity> Rarities
    {
        get { return Enum.GetValues(typeof(Rarity)).Cast<Rarity>(); }
    }

    public IEnumerable<Areas> Areas => Enum.GetValues(typeof(Areas)).Cast<Areas>();

    public IEnumerable<ElementViewModel?> ElementBase { get; set; }
    public ElementViewModel? SelectedElement { get; set; }

    public string? Effectiveness
    {
        get => _effectiveness;
        set => _effectiveness = value == string.Empty ? null : value;
    }

    public List<FilterTag>? Tags { get; set; } = new();

    public class FilterTag
    {
        public bool Active { get; set; }
        public string? Name { get; set; }
    }
}