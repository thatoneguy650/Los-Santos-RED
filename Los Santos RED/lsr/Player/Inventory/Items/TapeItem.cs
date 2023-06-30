using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Player;
using Rage;
using System;
using System.Xml.Serialization;

[Serializable()]
public class TapeItem : ModItem
{
    public TapeItem()
    {

    }
    public TapeItem(string name, string description) : base(name, description, ItemType.Equipment)
    {

    }
    public TapeItem(string name) : base(name, ItemType.Equipment)
    {

    }
    public override bool UseItem(IActionable actionable, ISettingsProvideable settings, IEntityProvideable world, ICameraControllable cameraControllable, IIntoxicants intoxicants, ITimeControllable time)
    {
        //EntryPoint.WriteToConsoleTestLong("I AM IN TapeItem ACTIVITY!!!!!!!!!!");
        Game.DisplayHelp($"Item: {Name} is currently unused");
        return false;
    }
}

