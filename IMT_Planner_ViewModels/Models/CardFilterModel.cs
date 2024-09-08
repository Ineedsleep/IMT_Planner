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
    private Areas? _area;
    private Rarity? _rarity;
    private int? _promoted;
    private bool? _hasIncomeFactor;
    private double? _passiveMultiplier;
    private bool? _hasCostReduction;
    private double? _costReductionValue;
    private bool? _hasShaftUnlockReduction;
    private double? _shaftUnlockReduction;
    private string? _effectiveness;

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

    public int? Promoted
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
        set => _costReductionValue = value;
    }


    public bool? HasShaftUnlockReduction
    {
        get => _hasShaftUnlockReduction;
        set => _hasShaftUnlockReduction = value;
    }

    public double? ShaftUnlockReduction
    {
        get => _shaftUnlockReduction;
        set => _shaftUnlockReduction = value;
    }

    public Expression<Func<SuperManager, bool>> GetExpression()
    {
        var elementNames = SelectedElement?.ElementName; //Elements?.Select(e => e.Element.Name).ToList();

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
}