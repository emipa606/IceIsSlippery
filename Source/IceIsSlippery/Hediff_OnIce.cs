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

        Severity = 0.6f;

        if (!IceIsSlipperyMod.Instance.Settings.CanFall)
        {
            return;
        }

        if (Rand.Chance(IceIsSlipperyMod.Instance.Settings.RiskOfFalling))
        {
            Severity = 1.1f;
        }
    }
}