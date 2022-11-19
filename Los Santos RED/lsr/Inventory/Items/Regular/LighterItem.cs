using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Player;
using Rage;
using System;
using System.Xml.Serialization;

[Serializable()]
public class LighterItem : ModItem
{
    public LighterItem()
    {

    }
    public LighterItem(string name, string description) : base(name, description, ItemType.Tools)
    {

    }
    public LighterItem(string name) : base(name, ItemType.Tools)
    {

    }
    public override bool UseItem(IActionable actionable, ISettingsProvideable settings, IEntityProvideable world, ICameraControllable cameraControllable, IIntoxicants intoxicants)
    {
        EntryPoint.WriteToConsole("I AM IN Lighter ACTIVITY!!!!!!!!!!");
        if (actionable.IsOnFoot && !actionable.ActivityManager.IsLayingDown && actionable.ActivityManager.CanUseItemsBase)
        {
            actionable.ActivityManager.StartUpperBodyActivity(new LighterActivity(actionable, this));
            return true;
        }

        return false;
    }
    public override bool ConsumeItem(IActionable actionable, bool applyNeeds)
    {
        return base.ConsumeItem(actionable, applyNeeds);
    }
}

