namespace IMT_ChronoPlanner.Models;

public class Mine
{
    public List<MineEntity> MineShafts { get; set; }
    public MineEntity Elevator { get; set; }
    public MineEntity Warehouse { get; set; }
}