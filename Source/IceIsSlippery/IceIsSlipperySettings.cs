using Verse;

namespace IceIsSlippery;

/// <summary>
///     Definition of the settings for the mod
/// </summary>
internal class IceIsSlipperySettings : ModSettings
{
    public bool Animals;
    public bool CanFall = true;
    public bool Colonists = true;
    public bool Enemies = true;
    public bool Entities;
    public bool Humanoids = true;
    public bool Mechanoids;
    public bool Neutrals = true;
    public float RiskOfFalling = 0.0005f;
    public bool Skating = true;

    /// <summary>
    ///     Saving and loading the values
    /// </summary>
    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref Animals, "Animals");
        Scribe_Values.Look(ref Mechanoids, "Mechanoids");
        Scribe_Values.Look(ref Entities, "Entities");
        Scribe_Values.Look(ref Humanoids, "Humanoids", true);
        Scribe_Values.Look(ref Skating, "Skating", true);
        Scribe_Values.Look(ref CanFall, "CanFall", true);
        Scribe_Values.Look(ref Enemies, "Enemies", true);
        Scribe_Values.Look(ref Neutrals, "Neutrals", true);
        Scribe_Values.Look(ref Colonists, "Colonists", true);
        Scribe_Values.Look(ref RiskOfFalling, "RiskOfFalling", 0.0005f);
    }

    public void Reset()
    {
        Animals = false;
        Entities = false;
        Humanoids = true;
        Mechanoids = false;
        Skating = true;
        CanFall = true;
        Neutrals = true;
        Enemies = true;
        Colonists = true;
        RiskOfFalling = 0.0005f;
    }
}