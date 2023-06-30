using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Player;
using System;
using System.Xml.Serialization;

[Serializable()]
public class BinocularsItem : ModItem
{
    public bool CanSearch { get; set; } = true;

    public float MaxFOV { get; set; } = 50f;
    public float MidFOV { get; set; } = 35f;
    public float MinFOV { get; set; } = 10f;
    public bool HasNightVision { get; set; } = true;
    public bool HasThermalVision { get; set; } = true;

    public BinocularsItem()
    {

    }
    public BinocularsItem(string name, string description) : base(name, description, ItemType.Equipment)
    {

    }
    public BinocularsItem(string name) : base(name, ItemType.Equipment)
    {

    }
    public override bool UseItem(IActionable actionable, ISettingsProvideable settings, IEntityProvideable world, ICameraControllable cameraControllable, IIntoxicants intoxicants, ITimeControllable time)
    {
        BinocularActivity activity = new BinocularActivity(actionable, settings, this);
        if (activity.CanPerform(actionable))
        {
            actionable.ActivityManager.StartUpperBodyActivity(activity);
            return true;
        }
        return false;
    }
}

