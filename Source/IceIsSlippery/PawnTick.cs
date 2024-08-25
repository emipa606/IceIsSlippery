using HarmonyLib;
using Verse;

namespace IceIsSlippery;

[HarmonyPatch(typeof(Pawn), nameof(Pawn.Tick))]
public static class PawnTick
{
    public static void Postfix(Pawn __instance)
    {
        if (!__instance.IsHashIntervalTick(60))
        {
            return;
        }

        if (!__instance.CanHaveHediff())
        {
            __instance.VerifyHediff(false);
            return;
        }

        var mapComponent = __instance.Map.GetIceWatcher();
        if (mapComponent == null)
        {
            __instance.VerifyHediff(false);
            return;
        }

        __instance.VerifyHediff(mapComponent.IsIce(__instance));
    }
}