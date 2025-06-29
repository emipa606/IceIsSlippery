using System.Collections.Generic;
using System.Linq;
using Verse;

namespace IceIsSlippery;

public class IceWatcher(Map map) : MapComponent(map)
{
    private List<IntVec3> iceCells = [];

    public override void FinalizeInit()
    {
        base.FinalizeInit();
        updateIceCells();
    }

    public override void MapComponentTick()
    {
        if (Find.TickManager.TicksGame % GenTicks.TickLongInterval != 0)
        {
            return;
        }

        updateIceCells();
    }

    public bool IsIce(Pawn pawn)
    {
        return pawn is { Spawned: true } && iceCells.Contains(pawn.Position);
    }

    private void updateIceCells()
    {
        iceCells = map.AllCells.Where(cell => cell.GetTerrain(map).IsIce()).ToList();
    }
}