using LosSantosRED.lsr;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Interiors : IInteriors
{
    private readonly string ConfigFileName = "Plugins\\LosSantosRED\\Interiors.xml";
    private List<Interior> LocationsList;
    public void ReadConfig()
    {
        if (File.Exists(ConfigFileName))
        {
            LocationsList = Serialization.DeserializeParams<Interior>(ConfigFileName);
        }
        else
        {
            DefaultConfig();
            Serialization.SerializeParams(LocationsList, ConfigFileName);
        }
    }
    public List<Interior> GetAllPlaces()
    {
        return LocationsList;
    }
    public Interior GetInterior(int id)
    {
        return LocationsList.Where(x => x.ID == id).FirstOrDefault();
    }
    private void DefaultConfig()
    {
        LocationsList = new List<Interior>
        {
            new Interior(19458,"Sub Urban"),
            new Interior(29698,"Ammunation"),
            new Interior(49922,"Los Santos Tattoo"),
            new Interior(80386,"Ammunation"),
            new Interior(93442,"Los Santos Customs"),
            new Interior(102146,"Herr Kutz Barber"),
            new Interior(35074,"The Pit"),
            new Interior(88066,"Discount Store"),
            new Interior(118018,"Vanilla Unicorn"),
            new Interior(48130,"Ammunation"),
            new Interior(113922,"Beachcombover Barber"),
            new Interior(17154,"Binco"),
            new Interior(74874,"LtD Gas"),
            new Interior(115458,"Ammunation"),
            new Interior(37890,"Los Santos Customs"),
            new Interior(59138,"Ammunation"),
            new Interior(22786,"Binco"),


            new Interior(50178,"Rob's Liquors"),
            new Interior(10754,"Sub Urban"),
            new Interior(35586,"Ammunation"),
            new Interior(1282,"Ponsonby"),
            new Interior(37378,"Bob Mullet Hair & Beauty"),
            new Interior(14338,"Ponsonby"),

            //i went from the bottom right of the map up and left, still need to do soem hawick stuff and above LS


    };
    }
}

