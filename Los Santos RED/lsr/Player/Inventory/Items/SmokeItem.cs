using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Player;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class SmokeItem : ConsumableItem
{
    public override bool CanConsume { get; set; } = true;
    public bool NeedsRollingPapers { get; set; } = false;
    public uint Duration { get; set; } = 120000;
    public override string FullDescription(ISettingsProvideable Settings) => base.FullDescription(Settings) + $"~n~Requires: ~r~Lighter~s~" + (NeedsRollingPapers ? " and ~r~Rolling Papers~s~" : "");
    public SmokeItem()
    {
    }
    public SmokeItem(string name, ItemType itemType) : base(name, itemType)
    {

    }
    public SmokeItem(string name, string description, ItemType itemType) : base(name, description, itemType)
    {

    }

    [OnDeserialized()]
    private void SetValuesOnDeserialized(StreamingContext context)
    {
        Duration = 120000;
    }

    public override bool UseItem(IActionable actionable, ISettingsProvideable settings, IEntityProvideable world, ICameraControllable cameraControllable, IIntoxicants intoxicants, ITimeControllable time)
    {
        LighterItem lighterItem = actionable.Inventory.ItemsList.Where(x => x.ModItem != null).Select(x => x.ModItem).OfType<LighterItem>().ToList().FirstOrDefault();
        if (lighterItem == null)
        {
            Game.DisplayHelp($"Need a ~r~Lighter~s~ to use {Name}");
            return false;
        }
        RollingPapersItem rollingPapersItem = !NeedsRollingPapers ? null : actionable.Inventory.ItemsList.Where(x => x.ModItem != null).Select(x => x.ModItem).OfType<RollingPapersItem>().ToList().FirstOrDefault();
        if (NeedsRollingPapers && rollingPapersItem == null)
        {
            Game.DisplayHelp($"Need a ~r~Rolling Paper~s~ to use {Name}");
            return false;
        }
        SmokingActivity activity = new SmokingActivity(actionable, settings, this, intoxicants);
        if (!activity.CanPerform(actionable))
        {
            return false;
        }
        actionable.Inventory.Use(lighterItem);
        actionable.Inventory.Use(rollingPapersItem);
        base.UseItem(actionable, settings, world, cameraControllable, intoxicants, time);
        actionable.ActivityManager.StartUpperBodyActivity(activity);
        return true;
    }

}

