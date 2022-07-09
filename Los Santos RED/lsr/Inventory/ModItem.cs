using System;

[Serializable()]
public class ModItem
{
    public ModItem()
    {

    }
    public ModItem(string name, ItemType itemType)
    {
        Name = name;
        ItemType = itemType;
    }
    public ModItem(string name, bool requiresDLC, ItemType itemType)
    {
        Name = name;
        RequiresDLC = requiresDLC;
        ItemType = itemType;
    }
    public ModItem(string name, string description, ItemType itemType)
    {
        Name = name;
        Description = description;
        ItemType = itemType;
    }
    public ModItem(string name, string description, bool requiresDLC, ItemType itemType)
    {
        Name = name;
        Description = description;
        RequiresDLC = requiresDLC;
        ItemType = itemType;
    }
    public ModItem(string name, eConsumableType type, ItemType itemType)
    {
        Name = name;
        Type = type;
        ItemType = itemType;
    }
    public ModItem(string name, string description, eConsumableType type, ItemType itemType)
    {
        Name = name;
        Description = description;
        Type = type;
        ItemType = itemType;
    }
    public PhysicalItem ModelItem { get; set; }
    public PhysicalItem PackageItem { get; set; }
    public string Name { get; set; }
    public string Description { get; set; } = "";
    public ItemType ItemType { get; set; } = ItemType.None;
    public ItemSubType ItemSubType { get; set; } = ItemSubType.None;
    public string MeasurementName { get; set; } = "Item";


    public int AmountPerPackage { get; set; } = 1;
    public bool CleanupItemImmediately { get; set; } = false;


    public bool CanConsume => Type == eConsumableType.Drink || Type == eConsumableType.Eat || Type == eConsumableType.Smoke || Type == eConsumableType.Ingest || Type == eConsumableType.AltSmoke || Type == eConsumableType.Snort || Type == eConsumableType.Inject;
    public eConsumableType Type { get; set; } = eConsumableType.None;
    public string IntoxicantName { get; set; } = "";
    public bool IsIntoxicating => IntoxicantName != "";
    public bool ChangesHealth => HealthChangeAmount != 0;
    public int HealthChangeAmount { get; set; } = 0;
    public string HealthChangeDescription => HealthChangeAmount > 0 ? $"~g~+{HealthChangeAmount} ~s~HP" : $"~r~{HealthChangeAmount} ~s~HP";
    public bool ConsumeOnPurchase { get; set; } = false;

    public string NeedChangeDescription => (ChangesHunger ? HungerChangeDescription + " " : "") + (ChangesThirst ? ThirstChangeDescription + " " : "") + (ChangesSleep ? SleepChangeDescription : "").Trim();
    public bool ChangesNeeds => ChangesHunger || ChangesThirst || ChangesSleep;
    public bool ChangesHunger => HungerChangeAmount != 0.0f;
    public string HungerChangeDescription => ChangesHunger ? $"{(HungerChangeAmount > 0.0f ? "~g~+" : "~r~") + HungerChangeAmount.ToString() + "~s~ Hunger"}" : "";
    public float HungerChangeAmount { get; set; } = 0.0f;
    public bool ChangesThirst => ThirstChangeAmount != 0.0f;
    public string ThirstChangeDescription => ChangesThirst ? $"{(ThirstChangeAmount > 0.0f ? "~g~+" : "~r~") + ThirstChangeAmount.ToString() + "~s~ Thirst"}" : "";
    public float ThirstChangeAmount { get; set; } = 0.0f;
    public bool ChangesSleep => SleepChangeAmount != 0.0f;
    public string SleepChangeDescription => ChangesSleep ? $"{(SleepChangeAmount > 0.0f ? "~g~+" : "~r~") + SleepChangeAmount.ToString() + "~s~ Sleep"}" : "";
    public float SleepChangeAmount { get; set; } = 0.0f;
    public bool RequiresDLC { get; set; } = false;
    public bool IsTool => ToolType != ToolTypes.None;
    public ToolTypes ToolType { get; set; } = ToolTypes.None;
    public bool RequiresTool => RequiredToolType != ToolTypes.None;
    public ToolTypes RequiredToolType { get; set; } = ToolTypes.None;
    public float PercentLostOnUse { get; set; } = 0.0f;
    public bool IsPossessionIllicit { get; set; } = false;
}

