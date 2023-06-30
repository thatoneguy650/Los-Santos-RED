using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Player;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class EquipmentItem : ConsumableItem
{
    public override bool CanConsume { get; set; } = true;
    public EquipmentItem()
    {
    }
    public EquipmentItem(string name, ItemType itemType) : base(name, itemType)
    {

    }
    public EquipmentItem(string name, string description, ItemType itemType) : base(name, description, itemType)
    {

    }
    public override bool UseItem(IActionable actionable, ISettingsProvideable settings, IEntityProvideable world, ICameraControllable cameraControllable, IIntoxicants intoxicants, ITimeControllable time)
    {
        actionable.Inventory.Use(this);
        if (ChangesArmor)
        {
            actionable.ArmorManager.ChangeArmor(ArmorChangeAmount);
            Game.DisplaySubtitle($"Armor Changed {ArmorChangeAmount}");
        }
        if(ChangesHealth)
        {
            actionable.HealthManager.ChangeHealth(HealthChangeAmount);
            Game.DisplaySubtitle($"Health Changed {ArmorChangeAmount}");
        }
        return true;
    }

}

