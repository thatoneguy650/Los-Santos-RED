using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Player;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class TrophyItem : ModItem
{
    public TrophyItem()
    {
    }
    public TrophyItem(string name, ItemType itemType) : base(name, itemType)
    {

    }
    public TrophyItem(string name, string description, ItemType itemType) : base(name, description, itemType)
    {

    }
    public override bool UseItem(IActionable actionable, ISettingsProvideable settings, IEntityProvideable world, ICameraControllable cameraControllable, IIntoxicants intoxicants, ITimeControllable time)
    {
        return true;
    }
    public override void AddToList(PossibleItems possibleItems)
    {
        possibleItems?.TrophyItems.RemoveAll(x => x.Name == Name);
        possibleItems?.TrophyItems.Add(this);
    }

}

