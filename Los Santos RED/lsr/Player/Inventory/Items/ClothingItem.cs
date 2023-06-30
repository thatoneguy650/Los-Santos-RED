using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Player;
using System;
using System.Xml.Serialization;

[Serializable()]
public class ClothingItem : ModItem
{
    public ClothingItem()
    {

    }
    public ClothingItem(string name, string description) : base(name, description, ItemType.Valuables)
    {

    }
    public ClothingItem(string name) : base(name, ItemType.Valuables)
    {

    }
    public override bool UseItem(IActionable actionable, ISettingsProvideable settings, IEntityProvideable world, ICameraControllable cameraControllable, IIntoxicants intoxicants, ITimeControllable time)
    {
        //BinocularActivity activity = new BinocularActivity(actionable, settings, this);
        //if (activity.CanPerform(actionable))
        //{
        //    actionable.ActivityManager.StartUpperBodyActivity(activity);
        //    return true;
        //}
        return false;
    }
}

