using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Player;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class BodyArmorItem : ConsumableItem
{
    public override bool CanConsume { get; set; } = true;
    public BodyArmorItem()
    {
    }
    public BodyArmorItem(string name, ItemType itemType) : base(name, itemType)
    {

    }
    public BodyArmorItem(string name, string description, ItemType itemType) : base(name, description, itemType)
    {

    }
    public override bool UseItem(IActionable actionable, ISettingsProvideable settings, IEntityProvideable world, ICameraControllable cameraControllable, IIntoxicants intoxicants, ITimeControllable time)
    {
        actionable.Inventory.Use(this);
        actionable.ArmorManager.UseArmor(this);
        return true;
    }

}

