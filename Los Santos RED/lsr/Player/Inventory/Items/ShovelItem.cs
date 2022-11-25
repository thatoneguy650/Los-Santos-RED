using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Player;
using System;
using System.Xml.Serialization;

[Serializable()]
public class ShovelItem : ModItem
{

    public ShovelItem()
    {

    }
    public ShovelItem(string name, string description) : base(name, description, ItemType.Tools)
    {

    }
    public ShovelItem(string name) : base(name, ItemType.Tools)
    {

    }
    public override bool UseItem(IActionable actionable, ISettingsProvideable settings, IEntityProvideable world, ICameraControllable cameraControllable, IIntoxicants intoxicants)
    {
        ShovelActivity activity = new ShovelActivity(actionable, settings, cameraControllable, this);
        if (activity.CanPerform(actionable))
        {
            actionable.ActivityManager.StartUpperBodyActivity(activity);
            return true;
        }
        return false;
    }
}

