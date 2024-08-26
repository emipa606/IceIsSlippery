using System.Collections.Generic;
using System.Linq;
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
        foreach (var terrainDef in DefDatabase<TerrainDef>.AllDefsListForReading)
        {
            if (terrainDef.HasModExtension<SlipperyTerrain_ModExtension>())
            {
                iceTerrainDefs.Add(terrainDef);
            }
        }

        Log.Message(
            $"[IceIsSlippery]: Set {iceTerrainDefs.Count} terrains as slippery.\n{string.Join("\n", iceTerrainDefs.Select(def => def.LabelCap))}");
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