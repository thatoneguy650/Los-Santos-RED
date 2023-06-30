using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Player;
using Rage;
using System;
using System.Xml.Serialization;

[Serializable()]
public class PliersItem : ModItem
{
    public PliersItem()
    {

    }
    public PliersItem(string name, string description) : base(name, description, ItemType.Equipment)
    {

    }
    public PliersItem(string name) : base(name, ItemType.Equipment)
    {

    }
    public override bool UseItem(IActionable actionable, ISettingsProvideable settings, IEntityProvideable world, ICameraControllable cameraControllable, IIntoxicants intoxicants, ITimeControllable time)
    {
        PliersActivity activity = new PliersActivity(actionable, settings, this);
        if (activity.CanPerform(actionable))
        {
            actionable.ActivityManager.StartUpperBodyActivity(activity);
            return true;
        }
        return false;
    }
}

