using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Player;
using Rage;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;

[Serializable()]
public class BongItem : ModItem
{
    public List<string> PossibleDrugItems { get; set; } = new List<string>();
    public override string FullDescription(ISettingsProvideable Settings) => base.FullDescription(Settings) + $"~n~Requires: ~r~Lighter~s~ and ~r~{string.Join(",", PossibleDrugItems)}~s~";
    public BongItem()
    {

    }
    public BongItem(string name, string description) : base(name, description, ItemType.Paraphernalia)
    {

    }
    public BongItem(string name) : base(name, ItemType.Paraphernalia)
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
        ConsumableItem consumableItem = actionable.Inventory.ItemsList.Where(x=> x.ModItem != null).Select(x=> x.ModItem).OfType<ConsumableItem>().ToList()?.Where(x=> PossibleDrugItems.Contains(x.Name)).FirstOrDefault();
        if (consumableItem == null)
        {
            Game.DisplayHelp($"No Consumables found for {Name}");
            return false;
        }
        BongSmokingActivity activity = new BongSmokingActivity(actionable, settings, this, lighterItem, consumableItem, intoxicants);
        if (!activity.CanPerform(actionable))
        {
            return false;
        }
        actionable.Inventory.Use(lighterItem);
        actionable.Inventory.Use(consumableItem);
        base.UseItem(actionable, settings, world, cameraControllable, intoxicants, time);
        actionable.ActivityManager.StartUpperBodyActivity(activity);
        return true;
    }
}

