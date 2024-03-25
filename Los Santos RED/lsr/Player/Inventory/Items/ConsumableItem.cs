using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
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
    public Intoxicant Intoxicant { get; set; }

    //[XmlIgnore]
    //public ConsumableRefresher ConsumableItemNeedGain { get; set; }
    public string IntoxicantName { get; set; } = "";
    public bool IsIntoxicating => IntoxicantName != "";
    public float IntoxicationPerInterval { get; set; } = 0.2f;
    public int HealthChangeAmount { get; set; }
    public int ArmorChangeAmount { get; set; }
    public float HungerChangeAmount { get; set; }
    public float ThirstChangeAmount { get; set; }
    public float SleepChangeAmount { get; set; }
    public bool AlwaysChangesHealth { get; set; } = false;
    public bool ChangesHealth => HealthChangeAmount != 0;
    public bool ChangesArmor => ArmorChangeAmount != 0;
    public string HealthChangeDescription => HealthChangeAmount > 0 ? $"~g~+{HealthChangeAmount} ~s~HP" : $"~r~{HealthChangeAmount} ~s~HP";

    public string ArmorChangeDescription => ArmorChangeAmount > 0 ? $"~g~+{ArmorChangeAmount} ~s~AP" : $"~r~{ArmorChangeAmount} ~s~AP";
    public string NeedChangeDescription => (ChangesHunger ? HungerChangeDescription + " " : "") + (ChangesThirst ? ThirstChangeDescription + " " : "") + (ChangesSleep ? SleepChangeDescription : "")   + (AlwaysChangesHealth && ChangesHealth ? HealthChangeDescription : "") .Trim();
    public bool ChangesNeeds => ChangesHunger || ChangesThirst || ChangesSleep || (ChangesHealth && AlwaysChangesHealth);
    public bool ChangesHunger => HungerChangeAmount != 0.0f;
    public string HungerChangeDescription => ChangesHunger ? $"{(HungerChangeAmount > 0.0f ? "~g~+" : "~r~") + HungerChangeAmount.ToString() + "~s~ Hunger"}" : "";
    public bool ChangesThirst => ThirstChangeAmount != 0.0f;
    public string ThirstChangeDescription => ChangesThirst ? $"{(ThirstChangeAmount > 0.0f ? "~g~+" : "~r~") + ThirstChangeAmount.ToString() + "~s~ Thirst"}" : "";
    public bool ChangesSleep => SleepChangeAmount != 0.0f;
    public string SleepChangeDescription => ChangesSleep ? $"{(SleepChangeAmount > 0.0f ? "~g~+" : "~r~") + SleepChangeAmount.ToString() + "~s~ Sleep"}" : "";

    public override void Setup(PhysicalItems physicalItems, IWeapons weapons, IIntoxicants intoxicants)
    {
        if(!string.IsNullOrEmpty(IntoxicantName))
        {
            Intoxicant = intoxicants.Get(IntoxicantName);
        }
        base.Setup(physicalItems, weapons, intoxicants);
    }

    public override bool UseItem(IActionable actionable, ISettingsProvideable settings, IEntityProvideable world, ICameraControllable cameraControllable, IIntoxicants intoxicants, ITimeControllable time)
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
        if(IsIntoxicating && Intoxicant != null)
        {
            actionable.Intoxication.Ingest(Intoxicant);
        }
        if(ChangesArmor)
        {
            actionable.ArmorManager.ChangeArmor(ArmorChangeAmount);
        }
        return true;
    }

    public override bool ConsumeItemSlow(IActionable actionable, bool applyNeeds, ISettingsProvideable settings)
    {
        actionable.Inventory.Use(this);
        ConsumableRefresher ConsumableItemNeedGain = new ConsumableRefresher(actionable, this, settings) { SpeedMultiplier = 5.0f };
        uint GameTimeStarted = Game.GameTime;
        while (ConsumableItemNeedGain != null && !ConsumableItemNeedGain.IsFinished)// && Game.GameTime - GameTimeStarted <= 20000)
        {
            ConsumableItemNeedGain.Update();
            GameFiber.Yield();
        }
        if (IsIntoxicating && Intoxicant != null)
        {
            actionable.Intoxication.Ingest(Intoxicant);
        }
        if (ChangesArmor)
        {
            actionable.ArmorManager.ChangeArmor(ArmorChangeAmount);
        }
        return true;
    }


    public override string GetExtendedDescription(ISettingsProvideable settings)
    {
        string toReturn = (settings.SettingsManager.NeedsSettings.ApplyNeeds ? (ChangesNeeds ? $"~n~{NeedChangeDescription}" : "") : (ChangesHealth ? $"~n~{HealthChangeDescription}" : "")) + (ChangesArmor ? $"~n~{ArmorChangeDescription}" : "");
        if(Intoxicant != null)
        {
            toReturn += $"~n~{Intoxicant.Description}";
        }
        return toReturn;
    }
    public override string PurchaseMenuDescription(ISettingsProvideable settings)
    {
        string description = "";
        if (ChangesHealth && !settings.SettingsManager.NeedsSettings.ApplyNeeds)
        {
            description += $"~n~{HealthChangeDescription}";
        }
        if (ChangesArmor)
        {
            description += $"~n~{ArmorChangeDescription}";
        }
        if (ChangesNeeds && settings.SettingsManager.NeedsSettings.ApplyNeeds)
        {
            description += $"~n~{NeedChangeDescription}";
        }
        if (Intoxicant != null)
        {
            description += $"~n~{Intoxicant.Description}";
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
        if (ChangesArmor)
        {
            description += $"~n~{ArmorChangeDescription}";
        }
        if (ChangesNeeds && settings.SettingsManager.NeedsSettings.ApplyNeeds)
        {
            description += $"~n~{NeedChangeDescription}";
        }
        if (Intoxicant != null)
        {
            description += $"~n~{Intoxicant.Description}";
        }
        return description;
    }
}

