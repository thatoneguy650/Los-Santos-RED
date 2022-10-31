using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Player;
using System;
using System.Xml.Serialization;

[Serializable()]
public class ShovelItem : ModItem
{

    public ShovelItem()
    {
        ToolType = ToolTypes.Shovel;
    }
    public ShovelItem(string name, string description) : base(name, description, ItemType.Tools)
    {
        ToolType = ToolTypes.Shovel;
    }
    public ShovelItem(string name) : base(name, ItemType.Tools)
    {
        ToolType = ToolTypes.Shovel;
    }
    public override bool UseItem(IActionable actionable, ISettingsProvideable settings, IEntityProvideable world, ICameraControllable cameraControllable, IIntoxicants intoxicants)
    {
        EntryPoint.WriteToConsole("I AM IN SHOVEL ACTIVITY!!!!!!!!!!");
        if (actionable.IsOnFoot && !actionable.ActivityManager.IsResting && actionable.ActivityManager.CanUseItemsBase)
        {
            actionable.ActivityManager.StartLowerBodyActivity(new ShovelActivity(actionable,this, settings, cameraControllable));
            return true;
        }
        return false;
    }
}

