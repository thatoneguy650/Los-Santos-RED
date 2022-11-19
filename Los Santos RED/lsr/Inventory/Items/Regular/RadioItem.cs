using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Player;
using Rage;
using System;
using System.Xml.Serialization;

[Serializable()]
public class RadioItem : ModItem
{
    public RadioItem()
    {

    }
    public RadioItem(string name, string description) : base(name, description, ItemType.Tools)
    {

    }
    public RadioItem(string name) : base(name, ItemType.Tools)
    {

    }
    public override bool UseItem(IActionable actionable, ISettingsProvideable settings, IEntityProvideable world, ICameraControllable cameraControllable, IIntoxicants intoxicants)
    {
        EntryPoint.WriteToConsole("I AM IN RadioItem ACTIVITY!!!!!!!!!!");
        if (actionable.IsOnFoot && !actionable.ActivityManager.IsLayingDown && actionable.ActivityManager.CanUseItemsBase)
        {
            actionable.ActivityManager.StartLowerBodyActivity(new RadioActivity(actionable,settings, this));
            return true;
        }
        return false;
    }
}

