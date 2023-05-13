using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Player;
using Rage;
using System;
using System.Xml.Serialization;

[Serializable()]
public class BongItem : ModItem
{
    public BongItem()
    {

    }
    public BongItem(string name, string description) : base(name, description, ItemType.Equipment)
    {

    }
    public BongItem(string name) : base(name, ItemType.Equipment)
    {

    }
    public override bool UseItem(IActionable actionable, ISettingsProvideable settings, IEntityProvideable world, ICameraControllable cameraControllable, IIntoxicants intoxicants)
    {
        //EntryPoint.WriteToConsoleTestLong("I AM IN BongItem ACTIVITY!!!!!!!!!!");
        Game.DisplayHelp($"Item: {Name} is currently unused");
        return false;
    }
}

