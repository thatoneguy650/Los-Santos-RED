using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Player;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class PipeSmokeItem : ConsumableItem
{
    public override bool CanConsume { get; set; } = true;
    public override string FullDescription(ISettingsProvideable Settings) => base.FullDescription(Settings) + $"~n~Requires: ~r~Lighter~s~";
    public PipeSmokeItem()
    {
    }
    public PipeSmokeItem(string name, ItemType itemType) : base(name, itemType)
    {

    }
    public PipeSmokeItem(string name, string description, ItemType itemType) : base(name, description, itemType)
    {

    }
    public override bool UseItem(IActionable actionable, ISettingsProvideable settings, IEntityProvideable world, ICameraControllable cameraControllable, IIntoxicants intoxicants)
    {
        EntryPoint.WriteToConsole("I AM IN PipeSmokeItem ACTIVITY!!!!!!!!!!");
        if (!actionable.ActivityManager.IsResting && actionable.ActivityManager.CanUseItemsBase)
        {
            ModItem li = actionable.Inventory.Get(typeof(LighterItem))?.ModItem;
            if (li != null)
            {
                base.UseItem(actionable, settings, world, cameraControllable, intoxicants);
                actionable.ActivityManager.StartUpperBodyActivity(new PipeSmokingActivity(actionable, settings, this, intoxicants));
                return true;
            }
            else
            {
                Game.DisplayHelp($"Need a ~r~Lighter~s~ to use {Name}");
            }
        }
        return false;
    }

}

