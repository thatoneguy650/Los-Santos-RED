using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Player;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class SmokeItem : ConsumableItem
{
    public override bool CanConsume { get; set; } = true;
    public override string FullDescription(ISettingsProvideable Settings) => base.FullDescription(Settings) + $"~n~Requires: ~r~Lighter~s~";
    public SmokeItem()
    {
    }
    public SmokeItem(string name, ItemType itemType) : base(name, itemType)
    {

    }
    public SmokeItem(string name, string description, ItemType itemType) : base(name, description, itemType)
    {

    }
    public override bool UseItem(IActionable actionable, ISettingsProvideable settings, IEntityProvideable world, ICameraControllable cameraControllable, IIntoxicants intoxicants, ITimeControllable time)
    {
        SmokingActivity activity = new SmokingActivity(actionable, settings, this, intoxicants);
        if (activity.CanPerform(actionable))
        {



            ModItem li = actionable.Inventory.Get(typeof(LighterItem))?.ModItem;
            if (li == null)
            {
                Game.DisplayHelp($"Need a ~r~Lighter~s~ to use {Name}");
                return false;
            }
            actionable.Inventory.Use(li);



            base.UseItem(actionable, settings, world, cameraControllable, intoxicants, time);
            actionable.ActivityManager.StartUpperBodyActivity(activity);
            return true;
        }
        return false;
    }

}

