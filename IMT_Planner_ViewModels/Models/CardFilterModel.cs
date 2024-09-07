using System.Linq.Expressions;
using IMT_Planner_Model;
using LinqKit;

namespace IMT_Planner_ViewModels.Models;

public class CardFilterModel
{
    private int? _levelMin;
    private int? _levelMax;
    private int? _rankRangeMin;
    private int? _rankRangeMax;
    private int? _rank;
    private ICollection<Areas> _area;
    private ICollection<Rarity> _rarity;
    private ICollection<SuperManagerElement>? _elements;
    private bool _promoted;
    private bool? _hasIncomeFactor;
    private double? _passiveMultiplier;
    private bool? _hasCostReduction;
    private double? _costReductionValue;
    private bool? _hasShaftUnlockReduction;
    private double? _shaftUnlockReduction;

    public int? LevelMin
    {
        get => _levelMin;
        set => _levelMin = value;
    }

    public int? LevelMax
    {
        get => _levelMax;
        set => _levelMax = value;
    }

    public int? RankRangeMin
    {
        get => _rankRangeMin;
        set => _rankRangeMin = value;
    }

    public int? RankRangeMax
    {
        get => _rankRangeMax;
        set => _rankRangeMax = value;
    }
    
    public ICollection<Areas> Area
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

    public ICollection<Rarity> Rarity
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

    public ICollection<SuperManagerElement>? Elements
    {
        get => _elements;
        set
        {
            if (!Equals(_elements, value))
            {
                _elements = value;
            }
        }
    }

    public bool Promoted
    {
        get => _promoted;
        set => _promoted = value;
    }

    public bool? HasIncomeFactor
    {
        get => _hasIncomeFactor;
        set => _hasIncomeFactor = value;
    }

    public double? PassiveMultiplier
    {
        get => _passiveMultiplier;
        set => _passiveMultiplier = value;
    }
    public bool? HasCostReduction
    {
        get => _hasCostReduction;
        set => _hasCostReduction = value;
    }

    public double? CRValue
    {
        get => _costReductionValue;
        set => _costReductionValue= value;
    }
    
    
    public bool? HasShaftUnlockReduction
    {
        get => _hasShaftUnlockReduction;
        set => _hasShaftUnlockReduction = value;
    }

    public double? ShaftUnlockReduction
    {
        get => _shaftUnlockReduction;
        set => _shaftUnlockReduction= value;
    }
    public Expression<Func<SuperManager, bool>> GetExpression()
    {
        var elementNames = Elements?.Select(e => e.Element.Name).ToList();
        var effectiveness = Elements?.Select(e => e.EffectivenessType).ToList();

        var predicate = PredicateBuilder.New<SuperManager>(true);
        if (LevelMin.HasValue)
            predicate = predicate.And(sm => sm.Level >= LevelMin);
        if (LevelMax.HasValue)
            predicate = predicate.And(sm => sm.Level <= LevelMax);
        if (RankRangeMin.HasValue)
            predicate = predicate.And(sm => sm.Rank != null && sm.Rank.CurrentRank >= RankRangeMin);
        if (RankRangeMax.HasValue)
            predicate = predicate.And(sm => sm.Rank != null && sm.Rank.CurrentRank <= RankRangeMax);
        if (Elements != null)
        {
            predicate = predicate.And(manager => manager.SuperManagerElements != null
                                                 && manager.SuperManagerElements
                                                     .Any(x => elementNames.Contains(x.Element.Name)
                                                               && effectiveness.Contains(x.EffectivenessType)
                                                               && x.RankRequirement <= manager.Rank.CurrentRank));
        }

        if (HasIncomeFactor ?? false)
        {
            predicate = predicate.And(sm => sm.Passives.Any(p => p.Name.Abbreviation == "MIF" || p.Name.Abbreviation == "CIF"));
        }

        if (PassiveMultiplier.HasValue && HasIncomeFactor != null && HasIncomeFactor.Value)
        {
            predicate = predicate.And(sm => sm.Passives.Any(p => p.AttributeValue >= PassiveMultiplier));
        }
        
        if (HasCostReduction ?? false)
        {
            predicate = predicate.And(sm => sm.Passives.Any(p => p.Name.Abbreviation == "CR"));
        }

        if (CRValue.HasValue && HasCostReduction != null && HasCostReduction.Value)
        {
            predicate = predicate.And(sm => sm.Passives.Any(p => p.AttributeValue>= CRValue));
        }
        if (HasShaftUnlockReduction ?? false)
        {
            predicate = predicate.And(sm => sm.Passives.Any(p => p.Name.Abbreviation == "SUCR"));
        }

        if (ShaftUnlockReduction.HasValue && HasShaftUnlockReduction != null && HasShaftUnlockReduction.Value)
        {
            predicate = predicate.And(sm => sm.Passives.Any(p => p.AttributeValue >= ShaftUnlockReduction));
        }
        

        return predicate;
    }
}