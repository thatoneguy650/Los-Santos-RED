using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Player;
using System;
using System.Xml.Serialization;

[Serializable()]
public class FlashlightItem : ModItem
{
    public bool LightFollowsCamera { get; set; } = true;
    public bool AllowPropRotation { get; set; } = true;
    public bool UseFakeEmissive { get; set; } = true;
    public float PitchMax { get; set; } = 25f;
    public float PitchMin { get; set; } = -25f;
    public float HeadingMax { get; set; } = 30f;
    public float HeadingMin { get; set; } = -30f;

    public float EmissiveDistance { get; set; } = 100f;
    public float EmissiveBrightness { get; set; } = 1.0f;
    public float EmissiveHardness { get; set; } = 0.0f;
    public float EmissiveRadius { get; set; } = 13.0f;
    public float EmissiveFallOff { get; set; } = 1.0f;

    public float FakeEmissiveDistance { get; set; } = 0.4f;
    public float FakeEmissiveBrightness { get; set; } = 5.0f;
    public float FakeEmissiveHardness { get; set; } = 0.0f;
    public float FakeEmissiveRadius { get; set; } = 100f;
    public float FakeEmissiveFallOff { get; set; } = 1.0f;

    public bool IsCellphone { get; set; } = false;
    public bool CanSearch { get; set; } = true;
    public FlashlightItem()
    {

    }
    public FlashlightItem(string name, string description) : base(name, description, ItemType.Equipment)
    {

    }
    public FlashlightItem(string name) : base(name, ItemType.Equipment)
    {

    }
    public override bool UseItem(IActionable actionable, ISettingsProvideable settings, IEntityProvideable world, ICameraControllable cameraControllable, IIntoxicants intoxicants, ITimeControllable time)
    {
        FlashlightActivity activity = new FlashlightActivity(actionable, settings, this);
        if (activity.CanPerform(actionable))
        {
            actionable.ActivityManager.StartUpperBodyActivity(activity);
            return true;
        }
        return false;
    }
}

