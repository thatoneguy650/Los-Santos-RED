using System;

[Serializable()]
public class ModItem
{
    public ModItem()
    {

    }
    public ModItem(string name)
    {
        Name = name;
    }
    public ModItem(string name, bool requiresDLC)
    {
        Name = name;
        RequiresDLC = requiresDLC;
    }
    public ModItem(string name, string description)
    {
        Name = name;
        Description = description;
    }
    public ModItem(string name, string description, bool requiresDLC)
    {
        Name = name;
        Description = description;
        RequiresDLC = requiresDLC;
    }
    public ModItem(string name, eConsumableType type)
    {
        Name = name;
        Type = type;
    }
    public ModItem(string name, string description, eConsumableType type)
    {
        Name = name;
        Description = description;
        Type = type;
    }
    public PhysicalItem ModelItem { get; set; }
    public PhysicalItem PackageItem { get; set; }
    public string Name { get; set; }
    public string Description { get; set; } = "";


    public string MeasurementName { get; set; } = "Item";


    public int AmountPerPackage { get; set; } = 1;



    public bool CanConsume => Type == eConsumableType.Drink || Type == eConsumableType.Eat || Type == eConsumableType.Smoke || Type == eConsumableType.Ingest;
    public eConsumableType Type { get; set; } = eConsumableType.None;
    public string IntoxicantName { get; set; } = "";
    public bool IsIntoxicating => IntoxicantName != "";
    public bool RestoresHealth => HealthGained > 0;
    public int HealthGained { get; set; } = 0;
    public bool ConsumeOnPurchase { get; set; } = false;



    public bool RequiresDLC { get; set; } = false;


    public bool IsTool => ToolType != ToolTypes.None;
    public ToolTypes ToolType { get; set; } = ToolTypes.None;
    public bool RequiresTool => RequiredToolType != ToolTypes.None;
    public ToolTypes RequiredToolType { get; set; } = ToolTypes.None;
    public float PercentLostOnUse { get; set; } = 0.0f;



    public string FormattedItemType
    {
        get
        {
            if(IsTool)
            {
                return ToolType.ToString();
            }
            if(Type == eConsumableType.Drink)
            {
                return "Drinkable";
            }
            else if (Type == eConsumableType.Eat)
            {
                return "Edible";
            }
            else if (Type == eConsumableType.Smoke)
            {
                return "Smokeable";
            }
            else if (Type == eConsumableType.Ingest)
            {
                return "Ingestable";
            }
            else if (Type == eConsumableType.Service)
            {
                return "Service";
            }
            else if (Type == eConsumableType.None)
            {
                return "Other";
            }
            else
            {
                return Type.ToString();
            }
        }
    }
}

