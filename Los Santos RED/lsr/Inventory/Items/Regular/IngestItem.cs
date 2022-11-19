using LosSantosRED.lsr.Interface;
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
    public override bool UseItem(IActionable actionable, ISettingsProvideable settings, IEntityProvideable world, ICameraControllable cameraControllable, IIntoxicants intoxicants)
    {
        EntryPoint.WriteToConsole("I AM IN InjectItem ACTIVITY!!!!!!!!!!");
        if (!actionable.ActivityManager.IsLayingDown && actionable.ActivityManager.CanUseItemsBase)
        {
            base.UseItem(actionable, settings, world, cameraControllable, intoxicants);
            actionable.ActivityManager.StartUpperBodyActivity(new IngestActivity(actionable, settings, this, intoxicants));
            return true;
        }
        return false;
    }

}

