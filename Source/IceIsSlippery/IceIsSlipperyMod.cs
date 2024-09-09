using Mlie;
using UnityEngine;
using Verse;

namespace IceIsSlippery;

[StaticConstructorOnStartup]
internal class IceIsSlipperyMod : Mod
{
    /// <summary>
    ///     The instance of the settings to be read by the mod
    /// </summary>
    public static IceIsSlipperyMod instance;

    private static string currentVersion;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="content"></param>
    public IceIsSlipperyMod(ModContentPack content) : base(content)
    {
        instance = this;
        Settings = GetSettings<IceIsSlipperySettings>();
        currentVersion = VersionFromManifest.GetVersionFromModMetaData(content.ModMetaData);
    }

    /// <summary>
    ///     The instance-settings for the mod
    /// </summary>
    internal IceIsSlipperySettings Settings { get; }

    /// <summary>
    ///     The title for the mod-settings
    /// </summary>
    /// <returns></returns>
    public override string SettingsCategory()
    {
        return "Ice Is Slippery";
    }

    /// <summary>
    ///     The settings-window
    ///     For more info: https://rimworldwiki.com/wiki/Modding_Tutorials/ModSettings
    /// </summary>
    /// <param name="rect"></param>
    public override void DoSettingsWindowContents(Rect rect)
    {
        var listing_Standard = new Listing_Standard();
        listing_Standard.Begin(rect);
        listing_Standard.Label("IIS.AppliesTo".Translate());
        listing_Standard.Gap();
        listing_Standard.CheckboxLabeled("IIS.Humanoids".Translate(), ref Settings.Humanoids);
        listing_Standard.CheckboxLabeled("IIS.Animals".Translate(), ref Settings.Animals);
        listing_Standard.CheckboxLabeled("IIS.Mechanoids".Translate(), ref Settings.Mechanoids);
        if (ModsConfig.AnomalyActive)
        {
            listing_Standard.CheckboxLabeled("IIS.Entities".Translate(), ref Settings.Entities);
        }
        else
        {
            Settings.Entities = false;
        }

        listing_Standard.GapLine();
        listing_Standard.CheckboxLabeled("IIS.CanFall".Translate(), ref Settings.CanFall);

        listing_Standard.Gap();
        if (Settings.CanFall)
        {
            Settings.RiskOfFalling =
                listing_Standard.SliderLabeled("IIS.RiskOfFalling".Translate(GetRiskString(Settings.RiskOfFalling)),
                    Settings.RiskOfFalling, 0.0001f, 0.001f,
                    tooltip: "IIS.RiskOfFallingTT".Translate(Settings.RiskOfFalling.ToStringPercent()));
        }

        if (listing_Standard.ButtonText("IIS.Reset".Translate(), widthPct: 0.25f))
        {
            Settings.Reset();
        }

        if (currentVersion != null)
        {
            listing_Standard.Gap();
            GUI.contentColor = Color.gray;
            listing_Standard.Label("IIS.CurrentModVersion".Translate(currentVersion));
            GUI.contentColor = Color.white;
        }

        listing_Standard.End();
    }

    private static string GetRiskString(float risk)
    {
        switch (risk)
        {
            case < 0.00025f:
                return "IIS.RiskOfFalling.VeryLow".Translate();
            case < 0.0005f:
                return "IIS.RiskOfFalling.Low".Translate();
            case < 0.00075f:
                return "IIS.RiskOfFalling.Default".Translate();
            default:
                return "IIS.RiskOfFalling.High".Translate();
        }
    }
}