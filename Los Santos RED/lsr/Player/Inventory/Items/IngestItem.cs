﻿using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class IngestItem : ConsumableItem
{
    public override bool CanConsume { get; set; } = true;
    public IngestItem()
    {
    }
    public IngestItem(string name, ItemType itemType) : base(name, itemType)
    {

    }
    public IngestItem(string name, string description, ItemType itemType) : base(name, description, itemType)
    {

    }
    public override bool UseItem(IActionable actionable, ISettingsProvideable settings, IEntityProvideable world, ICameraControllable cameraControllable, IIntoxicants intoxicants, ITimeControllable time)
    {
        IngestActivityNew activity = new IngestActivityNew(actionable, settings, this, intoxicants);
        if (activity.CanPerform(actionable))
        {
            //base.UseItem(actionable, settings, world, cameraControllable, intoxicants, time);

            //consumned within the activity now!
            actionable.ActivityManager.StartUpperBodyActivity(activity);
            return true;
        }
        return false;
    }
    public override void AddToList(PossibleItems possibleItems)
    {
        possibleItems?.IngestItems.RemoveAll(x => x.Name == Name);
        possibleItems?.IngestItems.Add(this);
        base.AddToList(possibleItems);
    }

}

