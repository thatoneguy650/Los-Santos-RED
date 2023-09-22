using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class CellphoneItem : FlashlightItem
{
    public bool HasFlashlight { get; set; } = true;

    public CellphoneItem()
    {

    }
    public CellphoneItem(string name, string description) : base(name, description)
    {

    }
    public CellphoneItem(string name) : base(name)
    {

    }
    public override bool UseItem(IActionable actionable, ISettingsProvideable settings, IEntityProvideable world, ICameraControllable cameraControllable, IIntoxicants intoxicants, ITimeControllable time)
    {
        if(!HasFlashlight)
        {
            return false;
        }
        IsCellphone = true;
        FlashlightActivity activity = new FlashlightActivity(actionable, settings, this);
        if (activity.CanPerform(actionable))
        {
            actionable.ActivityManager.StartUpperBodyActivity(activity);
            return true;
        }
        return false;
    }
}

