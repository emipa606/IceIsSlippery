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
    public static IceIsSlipperyMod Instance;

    private static string currentVersion;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="content"></param>
    public IceIsSlipperyMod(ModContentPack content) : base(content)
    {
        Instance = this;
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
        var listingStandard = new Listing_Standard();
        listingStandard.Begin(rect);
        listingStandard.Label("IIS.AppliesTo".Translate());
        listingStandard.Gap();
        listingStandard.CheckboxLabeled("IIS.Humanoids".Translate(), ref Settings.Humanoids);
        listingStandard.CheckboxLabeled("IIS.Animals".Translate(), ref Settings.Animals);
        listingStandard.CheckboxLabeled("IIS.Mechanoids".Translate(), ref Settings.Mechanoids);
        listingStandard.CheckboxLabeled("IIS.Colonists".Translate(), ref Settings.Colonists);
        listingStandard.CheckboxLabeled("IIS.Neutrals".Translate(), ref Settings.Neutrals);
        listingStandard.CheckboxLabeled("IIS.Enemies".Translate(), ref Settings.Enemies);
        if (ModsConfig.AnomalyActive)
        {
            listingStandard.CheckboxLabeled("IIS.Entities".Translate(), ref Settings.Entities);
        }
        else
        {
            Settings.Entities = false;
        }

        listingStandard.GapLine();
        listingStandard.CheckboxLabeled("IIS.CanFall".Translate(), ref Settings.CanFall);

        listingStandard.Gap();
        if (Settings.CanFall)
        {
            Settings.RiskOfFalling =
                listingStandard.SliderLabeled("IIS.RiskOfFalling".Translate(getRiskString(Settings.RiskOfFalling)),
                    Settings.RiskOfFalling, 0.0001f, 0.001f,
                    tooltip: "IIS.RiskOfFallingTT".Translate(Settings.RiskOfFalling.ToStringPercent()));
        }

        if (listingStandard.ButtonText("IIS.Reset".Translate(), widthPct: 0.25f))
        {
            Settings.Reset();
        }

        if (currentVersion != null)
        {
            listingStandard.Gap();
            GUI.contentColor = Color.gray;
            listingStandard.Label("IIS.CurrentModVersion".Translate(currentVersion));
            GUI.contentColor = Color.white;
        }

        listingStandard.End();
    }

    private static string getRiskString(float risk)
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