﻿using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class FoodItem : ConsumableItem
{
    public override bool CanConsume { get; set; } = true;
    public int AnimationCycles { get; set; } = 15;
    public FoodItem()
    {
    }
    public FoodItem(string name, ItemType itemType) : base(name, itemType)
    {

    }
    public FoodItem(string name, string description, ItemType itemType) : base(name, description, itemType)
    {

    }
    [OnDeserialized()]
    private void SetValuesOnDeserialized(StreamingContext context)
    {
        AnimationCycles = 25;
    }
    public override bool UseItem(IActionable actionable, ISettingsProvideable settings, IEntityProvideable world, ICameraControllable cameraControllable, IIntoxicants intoxicants, ITimeControllable time)
    {
        EatingActivityNew activity = new EatingActivityNew(actionable, settings, this, intoxicants);
        if (activity.CanPerform(actionable))
        {
            base.UseItem(actionable, settings, world, cameraControllable, intoxicants, time);
            actionable.ActivityManager.StartUpperBodyActivity(activity);
            return true;
        }
        return false;
    }
    public override string PurchaseMenuDescription(ISettingsProvideable settings)
    {
        string description = base.PurchaseMenuDescription(settings);
        if (ConsumeOnPurchase)
        {
            description += $"~n~~r~Dine-In Only~s~";
        }
        return description;
    }
    public override void AddToList(PossibleItems possibleItems)
    {
        possibleItems?.FoodItems.RemoveAll(x => x.Name == Name);
        possibleItems?.FoodItems.Add(this);
        base.AddToList(possibleItems);
    }

}

