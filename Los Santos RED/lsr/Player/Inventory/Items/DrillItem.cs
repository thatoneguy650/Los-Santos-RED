using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Player;
using Rage;
using System;
using System.Xml.Serialization;

[Serializable()]
public class DrillItem : ModItem
{
    public uint MinSafeDrillTime { get; set; } = 9000;
    public uint MaxSafeDrillTime { get; set; } = 18000;
    public DrillItem()
    {

    }
    public DrillItem(string name, string description) : base(name, description, ItemType.Equipment)
    {

    }
    public DrillItem(string name) : base(name, ItemType.Equipment)
    {

    }
    public override bool UseItem(IActionable actionable, ISettingsProvideable settings, IEntityProvideable world, ICameraControllable cameraControllable, IIntoxicants intoxicants, ITimeControllable time)
    {
        DrillActivity activity = new DrillActivity(actionable, settings, this);
        if (activity.CanPerform(actionable))
        {
            actionable.ActivityManager.StartUpperBodyActivity(activity);
            return true;
        }
        return false;
    }
}

