﻿using LosSantosRED.lsr;
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
    public Interior GetInterior(string name)
    {
        return LocationsList.Where(x => x.Name == name).FirstOrDefault();
    }
    private void DefaultConfig()
    {
        LocationsList = new List<Interior>
        {
            new Interior(19458,"Sub Urban"),


            new Interior(29698,"Ammunation"),
            new Interior(59138,"Ammunation"),
            new Interior(80386,"Ammunation"),
            new Interior(48130,"Ammunation"),
            new Interior(115458,"Ammunation"),
            new Interior(94978,"Ammunation"),
            new Interior(37122,"Ammunation"),
            new Interior(49922,"Los Santos Tattoo"),     
            new Interior(93442,"Los Santos Customs"),
            new Interior(102146,"Herr Kutz Barber"),
            new Interior(35074,"The Pit"),
            new Interior(88066,"Discount Store"),
            new Interior(118018,"Vanilla Unicorn"),
            new Interior(113922,"Beachcombover Barber"),
            new Interior(17154,"Binco"),
            new Interior(74874,"LtD Gas"),
            new Interior(37890,"Los Santos Customs"),
            new Interior(22786,"Binco"),
            new Interior(58882,"FIB Downtown",new List<string>() { "FIBlobby" },new List<string>() { "FIBlobbyfake" },new List<InteriorDoor>() { new InteriorDoor(-1517873911, new Vector3(106.3793f, -742.6982f, 46.51962f)),new InteriorDoor(-90456267, new Vector3(105.7607f, -746.646f, 46.18266f))}),
            new Interior(104450,"Liquor Ace"),
            new Interior(78338,"Maze Bank Arena",new List<string>() { "sp1_10_real_interior" },new List<string>() { "sp1_10_fake_interior" }),
            new Interior(78338,"Pillbox Hill Hospital",new List<string>() { "RC12B_Default" },new List<string>() { "RC12B_Destroyed","RC12B_HospitalInterior","RC12B_Fixed" }),
            new Interior(60418,"Los Santos County Coroner Office",new List<string>() { "Coroner_Int_on","coronertrash" }),

            new Interior(3842,"Paleto Bay Police Station",new List<string>() { "v_sheriff2" },new List<string>() { "cs1_16_sheriff_cap" },new List<InteriorDoor>() { new InteriorDoor(-1501157055, new Vector3(-444.4985f, 6017.06f, 31.86633f)),new InteriorDoor(-1501157055, new Vector3(-442.66f, 6015.222f, 31.86633f))}) { DisabledInteriorCoords = new Vector3(-444.89068603515625f, 6013.5869140625f, 30.7164f) },

            /*        NativeFunction.CallByName<bool>("DISABLE_INTERIOR", NativeFunction.CallByName<int>("GET_INTERIOR_AT_COORDS", -444.89068603515625f, 6013.5869140625f, 30.7164f), false);
        NativeFunction.CallByName<bool>("CAP_INTERIOR", NativeFunction.CallByName<int>("GET_INTERIOR_AT_COORDS", -444.89068603515625f, 6013.5869140625f, 30.7164f), false);
        NativeFunction.CallByName<bool>("REQUEST_IPL", "v_sheriff2");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "cs1_16_sheriff_cap");
        NativeFunction.CallByHash<bool>(0x9B12F9A24FABEDB0, -1501157055, -444.4985f, 6017.06f, 31.86633f, false, 0.0f, 0.0f, 0.0f);
        NativeFunction.CallByHash<bool>(0x9B12F9A24FABEDB0, -1501157055, -442.66f, 6015.222f, 31.86633f, false, 0.0f, 0.0f, 0.0f);*/



            //new Interior(171777,"Apartment"){ InteriorSets = new List<string>() { "swap_clean_apt", "layer_debra_pic", "layer_whiskey", "swap_sofa_A","swap_mrJam_A" } },
            new Interior(81666,"LSCFD Fire Station 7"),
            new Interior(62722,"24/7"),
            new Interior(89602,"Yellow Jacket Inn"),
            new Interior(50178,"Rob's Liquors"),
            new Interior(10754,"Sub Urban"),
            new Interior(35586,"Ammunation"),
            new Interior(1282,"Ponsonby"),
            new Interior(37378,"Bob Mullet Hair & Beauty"),
            new Interior(14338,"Ponsonby"),
            new Interior(7170, "Premium Deluxe Motorsport",new List<string>() { "shr_int" },new List<string>() { "fakeint" },new List<string>() { "shutter_open","csr_beforeMission" }),
            new Interior(47874, "Ltd Gasoline") { IsSPOnly = true } ,
            new Interior(25090,"Mission Carpark") { IsSPOnly = true } ,
            new Interior(39682,"Torture Room") { IsSPOnly = true } ,
            new Interior(76290,"Dell Perro Heights, Apt 4") { IsSPOnly = true } ,
            new Interior(108290,"Low End Apartment") { IsSPOnly = true } ,
            new Interior(69890,"IAA Office") { IsSPOnly = true } ,
            new Interior(25602,"Dell Perro Heights, Apt 7") { IsSPOnly = true } ,
            new Interior(31490,"FIB Floor 47") { IsSPOnly = true } ,
            new Interior(135973,"FIB Floor 49") { IsSPOnly = true } ,
            new Interior(60162,"Motel") { IsSPOnly = true } ,
            new Interior(69122,"Lester's House") { IsSPOnly = true } ,
            new Interior(47362,"4 Integrity Way, Apt 30") { IsSPOnly = true } ,
            new Interior(28418,"FIB Top Floor") { IsSPOnly = true } ,
            new Interior(70146,"10 Car Garage") { IsSPOnly = true } ,
            new Interior(85250,"Omega's Garage") { IsSPOnly = true } ,
            new Interior(61186,"Eclipse Towers, Apt 3") { IsSPOnly = true } ,
            new Interior(94722,"Booking Room") { IsSPOnly = true } 
            ,new Interior(146433, "10 Car") { IsMPOnly = true }
            ,new Interior(149761, "Low End Apartment") { IsMPOnly = true }
            ,new Interior(141313, "4 Integrity Way, Apt 30") { IsMPOnly = true }
            ,new Interior(145921, "Dell Perro Heights, Apt 4") { IsMPOnly = true }
            ,new Interior(145665, "Dell Perro Heights, Apt 7") { IsMPOnly = true }
            ,new Interior(146945, "Eclipse Towers, Apt 3") { IsMPOnly = true }
            ,new Interior(94722, "CharCreator") { IsMPOnly = true }
            ,new Interior(25090, "Mission Carpark") { IsMPOnly = true }
            ,new Interior(156929, "Torture Room") { IsMPOnly = true }
            ,new Interior(178433, "Omega's Garage") { IsMPOnly = true }
            ,new Interior(149505, "Motel") { IsMPOnly = true }
            ,new Interior(250881, "Lester's House") { IsMPOnly = true }
            ,new Interior(136449, "FBI Top Floor") { IsMPOnly = true }
            ,new Interior(135937, "FBI Floor 47") { IsMPOnly = true }
            ,new Interior(135937, "FBI Floor 49") { IsMPOnly = true }
            ,new Interior(135681, "IAA Office") { IsMPOnly = true }

            ,new Interior(149249, "2 Car") { IsMPOnly = true }
            ,new Interior(148737, "6 Car") { IsMPOnly = true }
            ,new Interior(148225, "Medium End Apartment") { IsMPOnly = true }
            ,new Interior(147201, "4 Integrity Way, Apt 28") { IsMPOnly = true }
            ,new Interior(146177, "Richard Majestic, Apt 2") { IsMPOnly = true }
            ,new Interior(146689, "Tinsel Towers, Apt 42") { IsMPOnly = true }
            ,new Interior(207105, "3655 Wild Oats Drive") { IsMPOnly = true }
            ,new Interior(206081, "2044 North Conker Avenue") { IsMPOnly = true }
            ,new Interior(206337, "2045 North Conker Avenue") { IsMPOnly = true }
            ,new Interior(208129, "2862 Hillcrest Avenue") { IsMPOnly = true }
            ,new Interior(207617, "2868 Hillcrest Avenue") { IsMPOnly = true }
            ,new Interior(207361, "2874 Hillcrest Avenue") { IsMPOnly = true }
            ,new Interior(206593, "2677 Whispymound Drive") { IsMPOnly = true }
            ,new Interior(208385, "2133 Mad Wayne Thunder") { IsMPOnly = true }
            ,new Interior(258561, "Bunker Interior") { IsMPOnly = true }
            ,new Interior(164865, "Solomon's Office") { IsMPOnly = true }
            ,new Interior(170497, "Psychiatrist's Office") { IsMPOnly = true }
            ,new Interior(165889, "Movie Theatre") { IsMPOnly = true }
            ,new Interior(205825, "Madrazos Ranch") { IsMPOnly = true }
            //,new Interior(0, "Life Invader Office") { IsMPOnly = true }
            ,new Interior(260353, "Smuggler's Run Hangar") { IsMPOnly = true }
            ,new Interior(262145, "Avenger Interior") { IsMPOnly = true }
            ,new Interior(269313, "Facility") { IsMPOnly = true }
            ,new Interior(270337, "Server Farm") { IsMPOnly = true }
            ,new Interior(271105, "Submarine") { IsMPOnly = true }
            ,new Interior(270081, "IAA Facility") { IsMPOnly = true }
            ,new Interior(271617, "Nightclub") { IsMPOnly = true }
            ,new Interior(271873, "Nightclub Warehouse") { IsMPOnly = true }
            ,new Interior(272129, "Terrorbyte Interior") { IsMPOnly = true }

            //i went from the bottom right of the map up and left, still need to do soem hawick stuff and above LS


    };
    }
}

