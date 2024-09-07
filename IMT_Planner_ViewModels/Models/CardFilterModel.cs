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
    private bool? _hasPassiveMultiplier;
    private double? _passiveMultiplier;
    private bool? _hasCostReduction;
    private double? _costReductionValue;
    private bool? _hasShaftUnlockReduction;
    private double? _shaftUnlockReduction;

    public int? LevelMin
    {
        get => _levelMin;
        set
        {
            if (_levelMin != value)
            {
                _levelMin = value;
            }
        }
    }

    public int? LevelMax
    {
        get => _levelMax;
        set
        {
            if (_levelMax != value)
            {
                _levelMax = value;
            }
        }
    }

    public int? RankRangeMin
    {
        get => _rankRangeMin;
        set
        {
                _rankRangeMin = value;
        }
    }

    public int? RankRangeMax
    {
        get => _rankRangeMax;
        set
        {
                _rankRangeMax = value;
        }
    }

    public int? Rank
    {
        get => _rank;
        set
        {
            if (_rank != value)
            {
                _rank = value;
            }
        }
    }

    public ICollection<Areas> Area
    {
        get => _area;
        set
        {
            if (_area != value)
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
            if (_rarity != value)
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
            if (_elements != value)
            {
                _elements = value;
            }
        }
    }

    public bool Promoted
    {
        get => _promoted;
        set
        {
            if (_promoted != value)
            {
                _promoted = value;
            }
        }
    }

    public bool? HasPassiveMultiplier
    {
        get => _hasPassiveMultiplier;
        set
        {
            if (_hasPassiveMultiplier != value)
            {
                _hasPassiveMultiplier = value;
            }
        }
    }

    public double? PassiveMultiplier
    {
        get => _passiveMultiplier;
        set
        {
            if (_passiveMultiplier != value)
            {
                _passiveMultiplier = value;
            }
        }
    }
    public bool? HasCR
    {
        get => _hasCostReduction;
        set
        {
            if (_hasCostReduction != value)
            {
                _hasCostReduction = value;
            }
        }
    }

    public double? CRValue
    {
        get => _costReductionValue;
        set
        {
            if (_costReductionValue != value)
            {
                _costReductionValue= value;
            }
        }
    }
    
    
    public bool? HasShaftUnlockReduction
    {
        get => _hasShaftUnlockReduction;
        set
        {
            if (_hasShaftUnlockReduction != value)
            {
                _hasShaftUnlockReduction = value;
            }
        }
    }

    public double? ShaftUnlockReduction
    {
        get => _shaftUnlockReduction;
        set
        {
            if (_shaftUnlockReduction != value)
            {
                _shaftUnlockReduction= value;
            }
        }
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

        if (HasPassiveMultiplier ?? false)
        {
            predicate = predicate.And(sm => sm.Passives.HasCif || sm.Passives.HasMif);
        }

        if (PassiveMultiplier.HasValue)
        {
            predicate = predicate.And(sm => sm.Passives.ContinentIncomeFactor >= PassiveMultiplier || sm.Passives.MineIncomeFactor <= PassiveMultiplier);
        }
        
        if (HasCR ?? false)
        {
            predicate = predicate.And(sm => sm.Passives.HasCostReduction);
        }

        if (CRValue.HasValue)
        {
            predicate = predicate.And(sm => sm.Passives.CostReduction >= CRValue);
        }
        if (HasShaftUnlockReduction ?? false)
        {
            predicate = predicate.And(sm => sm.Passives.HasShaftUnlockReduction);
        }

        if (ShaftUnlockReduction.HasValue)
        {
            predicate = predicate.And(sm => sm.Passives.ShaftUnlockReduction >= ShaftUnlockReduction);
        }
        

        return predicate;
    }
}