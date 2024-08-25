using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using Verse;

namespace IceIsSlippery;

[StaticConstructorOnStartup]
public static class IceIsSlippery
{
    private static readonly HashSet<TerrainDef> iceTerrainDefs = [TerrainDefOf.Ice];

    private static readonly Dictionary<Map, IceWatcher> iceWatchers = new Dictionary<Map, IceWatcher>();

    static IceIsSlippery()
    {
        var naturesPrettySweetIce = DefDatabase<TerrainDef>.GetNamedSilentFail("TKKN_Ice");
        if (naturesPrettySweetIce != null)
        {
            iceTerrainDefs.Add(naturesPrettySweetIce);
        }

        var stonesAndTerrainsIce = DefDatabase<TerrainDef>.GetNamedSilentFail("IceDeep");
        if (stonesAndTerrainsIce != null)
        {
            iceTerrainDefs.Add(stonesAndTerrainsIce);
        }

        var rimedivalIce = DefDatabase<TerrainDef>.GetNamedSilentFail("Ice_WaterShallow");
        if (rimedivalIce != null)
        {
            iceTerrainDefs.Add(rimedivalIce);
            iceTerrainDefs.Add(TerrainDef.Named("Ice_WaterDeep"));
            iceTerrainDefs.Add(TerrainDef.Named("Ice_Marsh"));
        }

        var permafrostIce = DefDatabase<TerrainDef>.GetNamedSilentFail("IceST");
        if (permafrostIce != null)
        {
            iceTerrainDefs.Add(permafrostIce);
            iceTerrainDefs.Add(TerrainDef.Named("IceS"));
            iceTerrainDefs.Add(TerrainDef.Named("IceDT"));
            iceTerrainDefs.Add(TerrainDef.Named("IceD"));
            iceTerrainDefs.Add(TerrainDef.Named("IceSMT"));
            iceTerrainDefs.Add(TerrainDef.Named("IceSM"));
            iceTerrainDefs.Add(TerrainDef.Named("IceDMT"));
            iceTerrainDefs.Add(TerrainDef.Named("IceDM"));
            iceTerrainDefs.Add(TerrainDef.Named("IceMarshT"));
            iceTerrainDefs.Add(TerrainDef.Named("IceMarsh"));
        }

        var waterFreezesIce = DefDatabase<TerrainDef>.GetNamedSilentFail("WF_LakeIceThin");
        if (waterFreezesIce != null)
        {
            iceTerrainDefs.Add(waterFreezesIce);
            iceTerrainDefs.Add(TerrainDef.Named("WF_LakeIce"));
            iceTerrainDefs.Add(TerrainDef.Named("WF_LakeIceThick"));
            iceTerrainDefs.Add(TerrainDef.Named("WF_MarshIceThin"));
            iceTerrainDefs.Add(TerrainDef.Named("WF_MarshIce"));
            iceTerrainDefs.Add(TerrainDef.Named("WF_RiverIceThin"));
            iceTerrainDefs.Add(TerrainDef.Named("WF_RiverIce"));
            iceTerrainDefs.Add(TerrainDef.Named("WF_RiverIceThick"));
        }

        new Harmony("Mlie.IceIsSlippery").PatchAll(Assembly.GetExecutingAssembly());
    }

    public static IceWatcher GetIceWatcher(this Map map)
    {
        if (iceWatchers.TryGetValue(map, out var iceWatcher))
        {
            return iceWatcher;
        }

        iceWatcher = map.GetComponent<IceWatcher>();
        iceWatchers[map] = iceWatcher;

        return iceWatcher;
    }

    public static bool IsIce(this TerrainDef terrain)
    {
        return iceTerrainDefs.Contains(terrain);
    }

    public static bool CanHaveHediff(this Pawn pawn)
    {
        if (!pawn.Spawned)
        {
            return false;
        }

        if (pawn.Dead)
        {
            return false;
        }

        if (!IceIsSlipperyMod.instance.Settings.Humanoids && pawn.RaceProps.Humanlike)
        {
            return false;
        }

        if (!IceIsSlipperyMod.instance.Settings.Animals && pawn.RaceProps.Animal)
        {
            return false;
        }

        if (!IceIsSlipperyMod.instance.Settings.Mechanoids && pawn.RaceProps.IsMechanoid)
        {
            return false;
        }

        if (!IceIsSlipperyMod.instance.Settings.Entities && pawn.RaceProps.IsAnomalyEntity)
        {
            return false;
        }

        return true;
    }

    public static void VerifyHediff(this Pawn pawn, bool shouldHave)
    {
        if (shouldHave)
        {
            if (pawn.health.hediffSet.HasHediff(SlipperyDefOf.Hediff_OnIce))
            {
                return;
            }

            pawn.health.AddHediff(SlipperyDefOf.Hediff_OnIce);
            return;
        }

        if (!pawn.health.hediffSet.HasHediff(SlipperyDefOf.Hediff_OnIce))
        {
            return;
        }

        pawn.health.RemoveHediff(pawn.health.hediffSet.GetFirstHediffOfDef(SlipperyDefOf.Hediff_OnIce));
    }
}