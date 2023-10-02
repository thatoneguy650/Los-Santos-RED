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
    public LighterItem(string name, string description) : base(name, description, ItemType.Equipment)
    {

    }
    public LighterItem(string name) : base(name, ItemType.Equipment)
    {

    }
    public override bool UseItem(IActionable actionable, ISettingsProvideable settings, IEntityProvideable world, ICameraControllable cameraControllable, IIntoxicants intoxicants, ITimeControllable time)
    {
        LighterActivity activity = new LighterActivity(actionable, this);
        if (activity.CanPerform(actionable))
        {
            actionable.ActivityManager.StartUpperBodyActivity(activity);
            return true;
        }
        return false;
    }
    //public override bool ConsumeItem(IActionable actionable, bool applyNeeds)
    //{
    //    return base.ConsumeItem(actionable, applyNeeds);
    //}
}

