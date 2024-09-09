using Verse;

namespace IceIsSlippery;

public class Hediff_OnIce : HediffWithComps
{
    public override void Tick()
    {
        base.Tick();

        Severity -= 0.001f;

        if (Severity > 0.5f)
        {
            return;
        }

        Severity = 0.5f;

        if (Rand.Chance(IceIsSlipperyMod.instance.Settings.RiskOfFalling))
        {
            Severity = IceIsSlipperyMod.instance.Settings.CanFall ? 1.1f : 0.99f;
        }
    }
}