using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class FoodItem : ConsumableItem
{
    public override bool CanConsume { get; set; } = true;
    public FoodItem()
    {
    }
    public FoodItem(string name, ItemType itemType) : base(name, itemType)
    {

    }
    public FoodItem(string name, string description, ItemType itemType) : base(name, description, itemType)
    {

    }
    public override bool UseItem(IActionable actionable, ISettingsProvideable settings, IEntityProvideable world, ICameraControllable cameraControllable, IIntoxicants intoxicants)
    {
        EatingActivity activity = new EatingActivity(actionable, settings, this, intoxicants);
        if (activity.CanPerform(actionable))
        {
            base.UseItem(actionable, settings, world, cameraControllable, intoxicants);
            actionable.ActivityManager.StartUpperBodyActivity(activity);
            return true;
        }
        return false;
    }
    public override string PurchaseMenuDescription(ISettingsProvideable settings)
    {
        string description = "";
        if (ChangesHealth && !settings.SettingsManager.NeedsSettings.ApplyNeeds)
        {
            description += $"~n~{HealthChangeDescription}";
        }
        if (ChangesNeeds && settings.SettingsManager.NeedsSettings.ApplyNeeds)
        {
            description += $"~n~{NeedChangeDescription}";
        }
        if (ConsumeOnPurchase)
        {
            description += $"~n~~r~Dine-In Only~s~";
        }
        return description;
    }

}

