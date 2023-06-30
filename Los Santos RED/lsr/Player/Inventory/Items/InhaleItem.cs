using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class InhaleItem : ConsumableItem
{
    public override bool CanConsume { get; set; } = true;
    public InhaleItem()
    {
    }
    public InhaleItem(string name, ItemType itemType) : base(name, itemType)
    {

    }
    public InhaleItem(string name, string description, ItemType itemType) : base(name, description, itemType)
    {

    }
    public override bool UseItem(IActionable actionable, ISettingsProvideable settings, IEntityProvideable world, ICameraControllable cameraControllable, IIntoxicants intoxicants, ITimeControllable time)
    {
        InhaleActivity activity = new InhaleActivity(actionable, settings, this, intoxicants);
        if (activity.CanPerform(actionable))
        {
            base.UseItem(actionable, settings, world, cameraControllable, intoxicants, time);
            actionable.ActivityManager.StartUpperBodyActivity(activity);
            return true;
        }
        return false;
    }

}

