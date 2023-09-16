using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Player;
using Rage;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;

[Serializable()]
public class RollingPapersItem : ModItem
{
    public List<string> PossibleDrugItems { get; set; } = new List<string>();
    public override string FullDescription(ISettingsProvideable Settings) => base.FullDescription(Settings) + $"~n~Requires: ~r~Lighter~s~ and ~r~{string.Join(",", PossibleDrugItems)}~s~";
    public RollingPapersItem()
    {

    }
    public RollingPapersItem(string name, string description) : base(name, description, ItemType.Paraphernalia)
    {

    }
    public RollingPapersItem(string name) : base(name, ItemType.Paraphernalia)
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
        if (PossibleDrugItems == null || !PossibleDrugItems.Any())
        {
            Game.DisplayHelp($"No Consumables found for {Name}");
            return false;
        }
        SmokeItem smokeItem = actionable.Inventory.ItemsList.Where(x => x.ModItem != null).Select(x => x.ModItem).OfType<SmokeItem>().ToList()?.Where(x => PossibleDrugItems.Contains(x.Name)).FirstOrDefault();
        if (smokeItem == null)
        {
            Game.DisplayHelp($"No Consumables found for {Name}");
            return false;
        }
        SmokingActivity activity = new SmokingActivity(actionable, settings, smokeItem, intoxicants);
        if (!activity.CanPerform(actionable))
        {
            return false;
        }
        actionable.Inventory.Use(lighterItem);
        actionable.Inventory.Use(smokeItem);
        base.UseItem(actionable, settings, world, cameraControllable, intoxicants, time);
        actionable.ActivityManager.StartUpperBodyActivity(activity);
        return true;
    }
}

