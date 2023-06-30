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
        ItemSubType = ItemSubType.LicensePlates;
    }

    public LicensePlateItem(string name) : base(name, ItemType.LicensePlates)
    {
        Description = "License Plate";
        ItemSubType = ItemSubType.LicensePlates;
    }
    public LSR.Vehicles.LicensePlate LicensePlate { get; set; }
    public override string DisplayName => LicensePlate == null ? Name : LicensePlate.ToString();
    public override bool UseItem(IActionable actionable, ISettingsProvideable settings, IEntityProvideable world, ICameraControllable cameraControllable, IIntoxicants intoxicants, ITimeControllable time)
    {
        PlateTheft plateTheft = new PlateTheft(actionable, this, settings, world);
        if(plateTheft.CanPerform(actionable))
        {
            actionable.ActivityManager.StartLowerBodyActivity(plateTheft);
            return true;
        }
        return false;
    }
    public override void AddNewItem(IModItems modItems)
    {
        LicensePlateItem existingItem = modItems.PossibleItems.LicensePlateItems.FirstOrDefault(x => x.Name == Name);
        if (existingItem != null)
        {
            existingItem.LicensePlate.IsWanted = LicensePlate.IsWanted;
        }
        else
        {
            modItems.PossibleItems.LicensePlateItems.Add(this);
        }
    }

}

