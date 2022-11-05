using LosSantosRED.lsr.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


[Serializable()]
public class LicensePlateItem : ModItem
{
    public LicensePlateItem()
    {
        Description = "License Plate";
    }

    public LicensePlateItem(string name) : base(name, ItemType.LicensePlates)
    {
        Description = "License Plate";
    }
    public LSR.Vehicles.LicensePlate LicensePlate { get; set; }
    public override bool UseItem(IActionable actionable, ISettingsProvideable settings, IEntityProvideable world, ICameraControllable cameraControllable, IIntoxicants intoxicants)
    {
        EntryPoint.WriteToConsole("I AM IN LicensePlateItem ACTIVITY!!!!!!!!!!");
        if (actionable.IsOnFoot && !actionable.ActivityManager.IsResting && actionable.ActivityManager.CanUseItemsBase)
        {
            actionable.ActivityManager.StartLowerBodyActivity(new PlateTheft(actionable, this, settings, world));
            return true;
        }
        return false;
    }
}

