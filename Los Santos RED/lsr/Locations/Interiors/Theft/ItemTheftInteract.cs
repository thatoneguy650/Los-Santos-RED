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
    private ModItem LastGivenItem;
    private TheftInteractItem nextItem;
    private Rage.Object CreatedObject;
    private bool prevIsVisible;
    private bool DoesItemExist;
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
    public List<AnimationPoint> RightHandAnimationPoints { get; set; } = new List<AnimationPoint>();
    public override void Setup(IModItems modItems, IClothesNames clothesNames)
    {
        base.Setup(modItems, clothesNames);
        if (PossibleItems == null || !PossibleItems.Any())
        {
            return;
        }
        foreach(TheftInteractItem item in PossibleItems)
        {
            item.Setup(modItems);
        }
        if (RightHandAnimationPoints == null || !RightHandAnimationPoints.Any())
        {
            RightHandAnimationPoints = new List<AnimationPoint>() { new AnimationPoint(0, 0.2f, true), new AnimationPoint(1, 0.4f, false), new AnimationPoint(2, 0.6f, true), new AnimationPoint(3, 0.8f, false) };
        }
    }
    public override void OnInteriorLoaded()
    {
        if (PossibleItems == null || !PossibleItems.Any())
        {
            return;
        }
        Items = new List<TheftInteractItem>();
        if (!RandomItems.RandomPercent(SpawnPercent))
        {
            EntryPoint.WriteToConsole($"ItemTheftInteract OnInteriorLoaded NOT SPAWNING (PERCENTAGE) ADDING NO ITEMS");
            return;
        }
        int ItemsToAdd = RandomItems.GetRandomNumberInt(MinItems, MaxItems);
        int ItemsAdded = 0;
        //EntryPoint.WriteToConsole($"ItemTheftInteract OnInteriorLoaded ItemsToAdd {ItemsToAdd}");
        for (int i = 0; i < ItemsToAdd; i++)
        {
            TheftInteractItem item = PickRandomItem();
            if (item != null)
            {
                Items.Add(item);
                //EntryPoint.WriteToConsole($"ItemTheftInteract OnInteriorLoaded ADDED ITEM {item.ModItemName}");
            }
            ItemsAdded++;
        }
        if(ItemsAdded > 0 && Items.Any())
        {
            nextItem = Items.PickRandom();
        }
        base.OnInteriorLoaded();
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
        if(nextItem != null)
        {
            Items.Remove(nextItem);
            int itemsToGive = RandomItems.GetRandomNumberInt(nextItem.MinItems, nextItem.MaxItems);
            if(itemsToGive > 0 && nextItem.ModItem != null)
            {
                LastGivenItem = nextItem.ModItem;
                Player.Inventory.Add(nextItem.ModItem, itemsToGive);
                NativeHelper.PlaySuccessSound();
                EntryPoint.WriteToConsole($"Got {nextItem.ModItemName} ({itemsToGive})");
                Game.DisplaySubtitle($"Got {nextItem.ModItemName} ({itemsToGive})");
            }
        }
        nextItem = Items.PickRandom();
        if (CreatedObject.Exists())
        {
            CreatedObject.Delete();
        }

        //CreateAndAttachItem();
    }
    protected override void HandlePreLoop()
    {
        //CreateAndAttachItem();
        base.HandlePreLoop();
    }
    protected override void HandleLoop(float animationTime)
    {
        foreach (AnimationPoint ap in RightHandAnimationPoints.OrderByDescending(x => x.Order))
        {
            if (animationTime >= ap.Position)
            {
                if(ap.Visible)
                {
                    if(!DoesItemExist)
                    {
                        CreateAndAttachItem();
                        DoesItemExist = true;
                    }
                }
                else
                {
                    if(DoesItemExist)
                    {
                        GiveReward();
                        DoesItemExist = false;
                    }
                }
                break;
            }
        }
//#if DEBUG
//        Game.DisplaySubtitle(Math.Round(animationTime, 2).ToString());
//#endif



    }
    protected override void HandlePostLoop()
    {
        if (CreatedObject.Exists())
        {
            CreatedObject.Delete();
        }
        base.HandlePostLoop();
    }
    private void CreateAndAttachItem()
    {
        if (CreatedObject.Exists())
        {
            CreatedObject.Delete();
        }
        if (nextItem != null && nextItem.ModItem != null)
        {
            CreatedObject = nextItem.ModItem.SpawnAndAttachItem(Player, true, true);
        }
    }
    protected override void HandleReward()
    {
       // base.HandleReward();
    }
}

