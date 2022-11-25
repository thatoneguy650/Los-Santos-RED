﻿using LosSantosRED.lsr.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

[Serializable()]
public class InventoryItem
{
    private ISettingsProvideable Settings;
    public InventoryItem(ModItem modItem, ISettingsProvideable settings)
    {
        ModItem = modItem;
        Settings = settings;
        //ItemName = modItem?.Name;
    }
    public InventoryItem()
    {

    }
    public string Description => ModItem.FullDescription(Settings) + $"~n~Amount: ~b~{Amount}~s~" + (ModItem.PercentLostOnUse > 0.0f ? $" (~b~{Math.Round(100f * RemainingPercent, 0)}%~s~)" : "");
    public string RightLabel => $"~b~{Amount}~s~" + (ModItem.MeasurementName != "Item" ? " " + ModItem.MeasurementName + "(s)" : "");
   // public string ItemName { get; set; }

    [XmlIgnore]
    //[XML]
    public ModItem ModItem { get; set; }


    public int Amount => (int)Math.Ceiling(RemainingPercent);
    public float RemainingPercent { get; set; }
    public void AddAmount(int toadd)
    {
        RemainingPercent += toadd;
    }
    public bool RemovePercent(float percentToRemove)
    {
        if(RemainingPercent >= percentToRemove)
        {
            RemainingPercent -= percentToRemove;
            return true;
        }
        else
        {
            return false;
        }
    }
}
