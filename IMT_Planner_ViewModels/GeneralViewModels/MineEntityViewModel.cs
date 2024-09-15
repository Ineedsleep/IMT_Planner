using IMT_Planner_Model;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace IMT_Planner_ViewModels.GeneralViewModels;

public class MineEntityViewModel : ObservableObject
{
    private MineEntity _mineEntity;
    private ElementViewModel _element;
    
    public MineEntityViewModel( MineEntity entity)
    {
        MineEntity = entity;
    }
    
    public MineEntity MineEntity
    {
        get => _mineEntity;
        set
        {
            if (Equals(value, _mineEntity)) return;
            _mineEntity = value;
            OnPropertyChanged();
        }
    }

    public ElementViewModel Element
    {
        get => _element;
        set
        {
            if (value ==  _element) return;
            _element = value;
            MineEntity.Element = _element.Element;
            OnPropertyChanged();
        }
    }

    public int Number
    {
        get => MineEntity.EntityNumber;
        set
        {
            if (value ==  MineEntity.EntityNumber) return;
            MineEntity.EntityNumber = value;
            OnPropertyChanged();
        }
    }

    public Areas Area
    {
        get =>  MineEntity.Area;
        set
        {
            if (value ==  MineEntity.Area) return;
            MineEntity.Area = value;
            OnPropertyChanged();
        }
    }

    public SuperManager AssignedSuperManager
    {
        get =>  MineEntity.AssignedSM;
        set
        {
            if (Equals(value,  MineEntity.AssignedSM)) return;
            MineEntity.AssignedSM = value;
            OnPropertyChanged();
        }
    }

    public double MaxCost
    {
        get =>  MineEntity.MaxCost;
        set
        {
            if (value.Equals( MineEntity.MaxCost)) return;
            MineEntity.MaxCost = value;
            OnPropertyChanged();
        }
    }

    public double OpeningCost
    {
        get =>  MineEntity.OpeningCost;
        set
        {
            if (value.Equals( MineEntity.OpeningCost)) return;
            MineEntity.OpeningCost = value;
            OnPropertyChanged();
        }
    }
    
    public string? ImageSource => $@"../../../../Resources/Sprites/General/{Area.ToString()}.png";
}