﻿using LosSantosRED.lsr.Interface;
using System;
using System.Xml.Serialization;

[Serializable()]
[XmlInclude(typeof(BinocularsItem))]
[XmlInclude(typeof(BongItem))]
[XmlInclude(typeof(ConsumableItem))]

[XmlInclude(typeof(DrillItem))]
[XmlInclude(typeof(DrinkItem))]
[XmlInclude(typeof(FlashlightItem))]
[XmlInclude(typeof(FoodItem))]
[XmlInclude(typeof(HammerItem))]
[XmlInclude(typeof(HotelStayItem))]
[XmlInclude(typeof(IngestItem))]
[XmlInclude(typeof(InhaleItem))]
[XmlInclude(typeof(InjectItem))]
[XmlInclude(typeof(LicensePlateItem))]
[XmlInclude(typeof(PipeSmokeItem))]
[XmlInclude(typeof(PliersItem))]
[XmlInclude(typeof(ScrewdriverItem))]
[XmlInclude(typeof(ShovelItem))]
[XmlInclude(typeof(SmokeItem))]


[XmlInclude(typeof(TapeItem))]
[XmlInclude(typeof(UmbrellaItem))]
[XmlInclude(typeof(VehicleItem))]
[XmlInclude(typeof(WeaponItem))]


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
    //public ModItem(string name, bool requiresDLC, ItemType itemType)
    //{
    //    Name = name;
    //    RequiresDLC = requiresDLC;
    //    ItemType = itemType;
    //}
    public ModItem(string name, string description, ItemType itemType)
    {
        Name = name;
        Description = description;
        ItemType = itemType;
    }
    //public ModItem(string name, string description, bool requiresDLC, ItemType itemType)
    //{
    //    Name = name;
    //    Description = description;
    //    RequiresDLC = requiresDLC;
    //    ItemType = itemType;
    //}


    [XmlIgnore]
    public PhysicalItem ModelItem { get; set; }



    [XmlIgnore]
    public PhysicalItem PackageItem { get; set; }







    public string Name { get; set; }
    public string Description { get; set; } = "";
    public string MeasurementName { get; set; } = "Item";
    public bool CleanupItemImmediately { get; set; } = false;//should be at the prop level?
    public bool IsPossessionIllicit { get; set; } = false;
    public bool ConsumeOnPurchase { get; set; } = false;
    public int AmountPerPackage { get; set; } = 1;

    public string ModelItemID { get; set; }
    public string PackageItemID { get; set; }




    public virtual bool CanConsume { get; set; } = false;//no no




    public ItemType ItemType { get; set; } = ItemType.None;
    public ItemSubType ItemSubType { get; set; } = ItemSubType.None;

   // public ToolTypes ToolType { get; set; } = ToolTypes.None;
    //public bool RequiresTool => RequiredToolType != ToolTypes.None;
    //public ToolTypes RequiredToolType { get; set; } = ToolTypes.None;





    public float PercentLostOnUse { get; set; } = 0.0f;









    ////maybe?
    //public bool RequiresDLC { get; set; } = false;




    public virtual string FullDescription(ISettingsProvideable Settings)
    {
        return $"{Description}~n~" 
            + GetTypeDescription(Settings)
            + GetExtendedDescription(Settings)
            + (MeasurementName != "Item" ? " " + MeasurementName + "(s)" : "");
        
    }


    public virtual bool UseItem(IActionable actionable, ISettingsProvideable settings, IEntityProvideable world, ICameraControllable cameraControllable, IIntoxicants intoxicants)
    {
        return false;
    }
    public virtual bool ConsumeItem(IActionable actionable, bool applyNeeds)
    {
        return false;
    }
    public virtual string GetTypeDescription(ISettingsProvideable settings)
    {
        return $"~n~Type: ~p~{ItemType}~s~" + (ItemSubType != ItemSubType.None ? $" - ~p~{ItemSubType}~s~" : "");
    }
    public virtual string GetExtendedDescription(ISettingsProvideable settings)
    {
        return "";
    }
    public virtual string PurchaseMenuDescription(ISettingsProvideable settings)
    {
        return "";
    }
    public virtual string SellMenuDescription(ISettingsProvideable settings)
    {
        return "";
    }
    //public virtual void AddItemToInventory(IActionable actionable, float remainingPercent)
    //{
    //    //actionable.Inventory.Add(this, remainingPercent);
    //}


    public virtual void AddNewItem(IModItems modItems)
    {

    }


}
