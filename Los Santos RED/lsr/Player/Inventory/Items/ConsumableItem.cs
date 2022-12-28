using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

public abstract class ConsumableItem : ModItem
{
    public ConsumableItem()
    {
    }
    public ConsumableItem(string name, ItemType itemType) : base(name, itemType)
    {

    }
    public ConsumableItem(string name, string description, ItemType itemType) : base(name, description, itemType)
    {

    }
    [XmlIgnore]
    public ConsumableRefresher ConsumableItemNeedGain { get; set; }
    public string IntoxicantName { get; set; } = "";
    public bool IsIntoxicating => IntoxicantName != "";
    public int HealthChangeAmount { get; set; }
    public float HungerChangeAmount { get; set; }
    public float ThirstChangeAmount { get; set; }
    public float SleepChangeAmount { get; set; }
    public bool AlwaysChangesHealth { get; set; } = false;
    public bool ChangesHealth => HealthChangeAmount != 0;
    public string HealthChangeDescription => HealthChangeAmount > 0 ? $"~g~+{HealthChangeAmount} ~s~HP" : $"~r~{HealthChangeAmount} ~s~HP";
    public string NeedChangeDescription => (ChangesHunger ? HungerChangeDescription + " " : "") + (ChangesThirst ? ThirstChangeDescription + " " : "") + (ChangesSleep ? SleepChangeDescription : "")   + (AlwaysChangesHealth && ChangesHealth ? HealthChangeDescription : "") .Trim();
    public bool ChangesNeeds => ChangesHunger || ChangesThirst || ChangesSleep || (ChangesHealth && AlwaysChangesHealth);
    public bool ChangesHunger => HungerChangeAmount != 0.0f;
    public string HungerChangeDescription => ChangesHunger ? $"{(HungerChangeAmount > 0.0f ? "~g~+" : "~r~") + HungerChangeAmount.ToString() + "~s~ Hunger"}" : "";
    public bool ChangesThirst => ThirstChangeAmount != 0.0f;
    public string ThirstChangeDescription => ChangesThirst ? $"{(ThirstChangeAmount > 0.0f ? "~g~+" : "~r~") + ThirstChangeAmount.ToString() + "~s~ Thirst"}" : "";
    public bool ChangesSleep => SleepChangeAmount != 0.0f;
    public string SleepChangeDescription => ChangesSleep ? $"{(SleepChangeAmount > 0.0f ? "~g~+" : "~r~") + SleepChangeAmount.ToString() + "~s~ Sleep"}" : "";
    public override bool UseItem(IActionable actionable, ISettingsProvideable settings, IEntityProvideable world, ICameraControllable cameraControllable, IIntoxicants intoxicants)
    {
        actionable.Inventory.Use(this);
        return true;
    }
    public override bool ConsumeItem(IActionable actionable, bool applyNeeds)
    {
        actionable.Inventory.Use(this);
        if (applyNeeds)
        {
            if (ChangesHunger)
            {
                actionable.HumanState.Hunger.Change(HungerChangeAmount, true);
            }
            if (ChangesSleep)
            {
                actionable.HumanState.Sleep.Change(SleepChangeAmount, true);
            }
            if (ChangesThirst)
            {
                actionable.HumanState.Thirst.Change(ThirstChangeAmount, true);
            }
            if(ChangesHealth && AlwaysChangesHealth)
            {
                actionable.HealthManager.ChangeHealth(HealthChangeAmount);
            }
        }
        else
        {
            if (ChangesHealth)
            {
                actionable.HealthManager.ChangeHealth(HealthChangeAmount);
            }
        }
        return true;
    }
    public override string GetExtendedDescription(ISettingsProvideable settings)
    {
        return (settings.SettingsManager.NeedsSettings.ApplyNeeds ? (ChangesNeeds ? $"~n~{NeedChangeDescription}" : "") : (ChangesHealth ? $"~n~{HealthChangeDescription}" : ""));
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
        return description;
    }
    public override string SellMenuDescription(ISettingsProvideable settings)
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
        return description;
    }
}

