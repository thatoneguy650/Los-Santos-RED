using LosSantosRED.lsr.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable()]
public class InventoryItem
{
    private ISettingsProvideable Settings;
    public InventoryItem(ModItem modItem, ISettingsProvideable settings)
    {
        ModItem = modItem;
        Settings = settings;
    }
    //public InventoryItem(ModItem modItem, int amount)
    //{
    //    ModItem = modItem;
    //    RemainingPercent = amount;
    //}
    public InventoryItem()
    {

    }
    public string Description => $"{ModItem.Description}~n~" + ModItem.GetTypeDescription(Settings)
                                                    + ModItem.GetExtendedDescription(Settings)// + (Settings.SettingsManager.NeedsSettings.ApplyNeeds ? (ModItem.ChangesNeeds ? $"~n~{ModItem.NeedChangeDescription}" : "") : (ModItem.ChangesHealth ? $"~n~{ModItem.HealthChangeDescription}" : ""))
                                                    
                                                    + $"~n~Amount: ~b~{Amount}~s~" + (ModItem.PercentLostOnUse > 0.0f ? $" (~b~{Math.Round(100f * RemainingPercent,0)}%~s~)" : "") 
                                                    + (ModItem.MeasurementName != "Item" ? " " + ModItem.MeasurementName + "(s)" : "") 
                                                    + (ModItem.RequiresTool? $"~n~Requires: ~r~{ModItem.RequiredToolType}" : "");
    public string RightLabel => $"~b~{Amount}~s~" + (ModItem.MeasurementName != "Item" ? " " + ModItem.MeasurementName + "(s)" : "");
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

