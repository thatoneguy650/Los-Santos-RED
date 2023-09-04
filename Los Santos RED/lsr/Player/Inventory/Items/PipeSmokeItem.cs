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
    public override string FullDescription(ISettingsProvideable Settings) => base.FullDescription(Settings) + $"~n~Requires: ~r~Lighter~s~ and ~r~Pipe~s~";
    public PipeSmokeItem()
    {
    }
    public PipeSmokeItem(string name, ItemType itemType) : base(name, itemType)
    {

    }
    public PipeSmokeItem(string name, string description, ItemType itemType) : base(name, description, itemType)
    {

    }
    public override bool UseItem(IActionable actionable, ISettingsProvideable settings, IEntityProvideable world, ICameraControllable cameraControllable, IIntoxicants intoxicants, ITimeControllable time)
    {
        LighterItem lighterItem = actionable.Inventory.ItemsList.Where(x => x.ModItem != null).Select(x => x.ModItem).OfType<LighterItem>().ToList().FirstOrDefault();
        if (lighterItem == null)
        {
            Game.DisplayHelp($"Need a ~r~Lighter~s~ to use {Name}");
            return false;
        }
        PipeItem pipeItem = actionable.Inventory.ItemsList.Where(x => x.ModItem != null).Select(x => x.ModItem).OfType<PipeItem>().ToList()?.Where(x=> x.PossibleDrugItems != null & x.PossibleDrugItems.Contains(Name)).FirstOrDefault();
        if (pipeItem == null)
        {
            Game.DisplayHelp($"Need a ~r~Pipe~s~ to use {Name}");
            return false;
        }
        PipeSmokingActivity activity = new PipeSmokingActivity(actionable, settings, pipeItem, lighterItem, this, intoxicants);
        if (!activity.CanPerform(actionable))
        {
            return false;
        }
        actionable.Inventory.Use(lighterItem);
        base.UseItem(actionable, settings, world, cameraControllable, intoxicants, time);
        actionable.ActivityManager.StartUpperBodyActivity(activity);
        return true;
    }

}

