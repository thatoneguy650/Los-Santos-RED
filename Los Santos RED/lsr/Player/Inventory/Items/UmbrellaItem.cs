using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Player;
using System;
using System.Xml.Serialization;

[Serializable()]
public class UmbrellaItem : ModItem
{   
    public UmbrellaItem()
    {

    }
    public UmbrellaItem(string name, string description) : base(name, description, ItemType.Equipment)
    {

    }
    public UmbrellaItem(string name) : base(name, ItemType.Equipment)
    {

    }
    public override bool UseItem(IActionable actionable, ISettingsProvideable settings, IEntityProvideable world, ICameraControllable cameraControllable, IIntoxicants intoxicants, ITimeControllable time)
    {
        UmbrellaActivity activity = new UmbrellaActivity(actionable, this);
        if (activity.CanPerform(actionable))
        {
            actionable.ActivityManager.StartUpperBodyActivity(activity);
            return true;
        }
        return false;
    }
}

