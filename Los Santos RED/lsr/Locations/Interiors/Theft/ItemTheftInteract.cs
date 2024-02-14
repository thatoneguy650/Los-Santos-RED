using ExtensionsMethods;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class ItemTheftInteract : TheftInteract
{
    public ItemTheftInteract()
    {

    }

    public ItemTheftInteract(string name, Vector3 position, float heading, string buttonPromptText) : base(name, position, heading, buttonPromptText)
    {

    }
    protected override bool CanInteract => Items != null && Items.Any();
    private List<TheftInteractItem> Items;
    public List<TheftInteractItem> PossibleItems { get; set; }
    public int MinItems { get; set; }
    public int MaxItems { get; set; }
    public override void Setup(IModItems modItems)
    {
        if(PossibleItems == null || !PossibleItems.Any())
        {
            return;
        }
        foreach(TheftInteractItem item in PossibleItems)
        {
            item.Setup(modItems);
        }
    }
    public override void OnInteriorLoaded()
    {
        if (PossibleItems == null || !PossibleItems.Any())
        {
            return;
        }
        Items = new List<TheftInteractItem>();
        int ItemsToAdd = RandomItems.GetRandomNumberInt(MinItems, MaxItems);
        int ItemsAdded = 0;
        EntryPoint.WriteToConsole($"ItemTheftInteract OnInteriorLoaded ItemsToAdd {ItemsToAdd}");
        for (int i = 0; i < ItemsToAdd; i++)
        {
            TheftInteractItem item = PickRandomItem();
            if (item != null)
            {
                Items.Add(item);
                EntryPoint.WriteToConsole($"ItemTheftInteract OnInteriorLoaded ADDED ITEM {item.ModItemName}");
            }
            ItemsAdded++;
        }
    }
    private TheftInteractItem PickRandomItem()
    {
        List<TheftInteractItem> ToPickFrom = PossibleItems.ToList();
        int Total = ToPickFrom.Sum(x => x.Percentage);
        int RandomPick = RandomItems.MyRand.Next(0, Total);
        foreach (TheftInteractItem theftItem in ToPickFrom)
        {
            int SpawnChance = theftItem.Percentage;
            if (RandomPick < SpawnChance)
            {
                return theftItem;
            }
            RandomPick -= SpawnChance;
        }
        return null;
    }
    protected override void GiveReward()
    {
        if(Items == null || !Items.Any())
        {
            return;
        }
        TheftInteractItem item = Items.PickRandom();
        if(item != null)
        {
            Items.Remove(item);
            int itemsToGive = RandomItems.GetRandomNumberInt(item.MinItems, item.MaxItems);
            if(itemsToGive > 0 && item.ModItem != null)
            {
                Player.Inventory.Add(item.ModItem, itemsToGive);
                NativeHelper.PlaySuccessSound();
                EntryPoint.WriteToConsole($"Got {item.ModItemName} ({itemsToGive})");
                Game.DisplaySubtitle($"Got {item.ModItemName} ({itemsToGive})");
            }
        }

    }
}

