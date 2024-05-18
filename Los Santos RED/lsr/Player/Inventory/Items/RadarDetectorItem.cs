using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Player;
using Rage;
using System;
using System.Xml.Serialization;

[Serializable()]
public class RadarDetectorItem : ModItem
{
    public RadarDetectorItem()
    {

    }
    public RadarDetectorItem(string name, string description) : base(name, description, ItemType.Equipment)
    {

    }
    public RadarDetectorItem(string name) : base(name, ItemType.Equipment)
    {

    }
}

