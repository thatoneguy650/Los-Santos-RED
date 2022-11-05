using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Player;
using Rage;
using System;
using System.Xml.Serialization;

[Serializable()]
public class DrillItem : ModItem
{
    public DrillItem()
    {

    }
    public DrillItem(string name, string description) : base(name, description, ItemType.Tools)
    {

    }
    public DrillItem(string name) : base(name, ItemType.Tools)
    {

    }
    public override bool UseItem(IActionable actionable, ISettingsProvideable settings, IEntityProvideable world, ICameraControllable cameraControllable, IIntoxicants intoxicants)
    {
        EntryPoint.WriteToConsole("I AM IN DrillItem ACTIVITY!!!!!!!!!!");
        Game.DisplayHelp($"Item: {Name} is currently unused");
        return false;
    }
}

