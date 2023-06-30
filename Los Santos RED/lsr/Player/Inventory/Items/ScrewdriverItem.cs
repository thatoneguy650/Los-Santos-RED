using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Player;
using Rage;
using System;
using System.Xml.Serialization;

[Serializable()]
public class ScrewdriverItem : ModItem
{
    public ScrewdriverItem()
    {

    }
    public ScrewdriverItem(string name, string description) : base(name, description, ItemType.Equipment)
    {

    }
    public ScrewdriverItem(string name) : base(name, ItemType.Equipment)
    {

    }
    public override bool UseItem(IActionable actionable, ISettingsProvideable settings, IEntityProvideable world, ICameraControllable cameraControllable, IIntoxicants intoxicants, ITimeControllable time)
    {
        ScrewdriverActivity activity = new ScrewdriverActivity(actionable, settings, this);
        if (activity.CanPerform(actionable))
        {
            actionable.ActivityManager.StartUpperBodyActivity(activity);
            return true;
        }
        return false;
    }
}

