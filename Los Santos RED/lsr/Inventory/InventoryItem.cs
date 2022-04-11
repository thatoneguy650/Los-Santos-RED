using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable()]
public class InventoryItem
{
    public InventoryItem(ModItem modItem)
    {
        ModItem = modItem;
    }
    public InventoryItem(ModItem modItem, int amount)
    {
        ModItem = modItem;
        RemainingPercent.Clear();
        for (int i = 0; i < amount; i++)
        {
            RemainingPercent.Add(1.0f);
        }
    }
    public InventoryItem()
    {

    }
    public string Description => $"{ModItem.Description}~n~~n~Type: ~p~{ModItem.ItemType}~s~" + (ModItem.ItemSubType != ItemSubType.None ? $" - ~p~{ModItem.ItemSubType}~s~" : "") 
                                                    + (ModItem.ChangesHealth ? $"~n~{ModItem.HealthChangeDescription}" : "")
                                                    + $"~n~Amount: ~b~{Amount}~s~" + (ModItem.PercentLostOnUse > 0.0f ? $" (~b~{Math.Round(100f * RemainingPercent.Sum(),0)}%~s~)" : "") 
                                                    + (ModItem.MeasurementName != "Item" ? " " + ModItem.MeasurementName + "(s)" : "") 
                                                    + (ModItem.RequiresTool? $"~n~Requires: ~r~{ModItem.RequiredToolType}" : "");
    public string RightLabel => $"~b~{Amount}~s~" + (ModItem.MeasurementName != "Item" ? " " + ModItem.MeasurementName + "(s)" : "");


    public ModItem ModItem { get; set; }
    public int Amount => RemainingPercent.Count();
    public List<float> RemainingPercent { get; set; } = new List<float>() { 1.0f };
    public void AddAmount(int toadd)
    {
        for (int i = 0; i < toadd; i++)
        {
            RemainingPercent.Add(1.0f);
        }
    }
    public bool RemoveAmount(int toRemove)
    {
        if(toRemove > Amount)
        {
            return false;
        }
        else
        {
            for (int i = toRemove; i > 0; i--)
            {
                RemainingPercent.RemoveAt(i);
            }
            return true;
        }
    }

    public bool RemovePercent(float percentToRemove)
    {
        bool hasSubtracted = false;//this is bad, two loops, this shouldnt need any, w/e
        List<float> newPercent = new List<float>();
        foreach (float currentPercents in RemainingPercent.OrderBy(x=>x))
        {
            if(currentPercents >= percentToRemove && !hasSubtracted)
            {
                newPercent.Add(currentPercents-percentToRemove);
                hasSubtracted = true;
                
            }
            else
            {
                newPercent.Add(currentPercents);
            }
            EntryPoint.WriteToConsole($"RemovePercent hasSubtracted {hasSubtracted} Was {currentPercents} Remove {percentToRemove} Current {currentPercents - percentToRemove}", 5);
        }
        if(hasSubtracted)
        {
            newPercent.RemoveAll(x => x <= 0.01f);
            RemainingPercent = newPercent;
            return true;
        }
        else
        {
            return false;
        }

    }

}

