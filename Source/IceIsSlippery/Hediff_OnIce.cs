using System;
using Verse;

namespace IceIsSlippery;

public class Hediff_OnIce : HediffWithComps
{
    public override void Tick()
    {
        base.Tick();

        Severity = Math.Max(0.5f, Severity - 0.001f);

        if (Severity > 0.5f)
        {
            if (Rand.Chance(0.005f))
            {
                Severity = 0.5f;
            }

            return;
        }

        if (Rand.Chance(IceIsSlipperyMod.instance.Settings.RiskOfFalling))
        {
            Severity = 1.25f;
        }
    }
}