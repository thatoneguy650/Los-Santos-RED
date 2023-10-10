using LosSantosRED.lsr.Interface;
using Rage;
using RAGENativeUI.PauseMenu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class LicensesTab
{
    private IGangRelateable Player;
    private ITimeReportable Time;
    private TabView TabView;
    private ILocationTypes LocationTypes;

    public LicensesTab(IGangRelateable player, ITimeReportable time, TabView tabView, ILocationTypes locationTypes)
    {
        Player = player;
        Time = time;
        TabView = tabView;
        LocationTypes = locationTypes;
    }

    public void AddItems()
    {
        List<TabItem> items = new List<TabItem>();
        string dldesc = "";
        string ccwdesc = "";
        string pilotdesc = "";

        if (Player.Licenses.HasDriversLicense)
        {
            GameState issueState = LocationTypes.GetState(Player.Licenses.DriversLicense.IssueStateID);
            if (issueState == null)
            {
                LocationTypes.GetState(StaticStrings.SanAndreasStateID);
            }
            if(issueState != null)
            {
                dldesc = $"State: ~y~{issueState.StateName}~s~~n~";
            }
            else
            {
                dldesc = "State: ~y~San Andreas~s~~n~";
            }
            dldesc += $"~n~Status: " + (Player.Licenses.DriversLicense.IsValid(Time) ? "~g~Valid~s~" : "~r~Expired~s~");
            dldesc += Player.Licenses.DriversLicense.ExpirationDescription(Time);
        }
        else
        {
            dldesc = "~r~No Drivers License Issued~s~";
        }



        if (Player.Licenses.HasCCWLicense)
        {
            GameState issueState = LocationTypes.GetState(Player.Licenses.CCWLicense.IssueStateID);
            if (issueState == null)
            {
                LocationTypes.GetState(StaticStrings.SanAndreasStateID);
            }
            if (issueState != null)
            {
                ccwdesc = $"State: ~y~{issueState.StateName}~s~~n~";
            }
            else
            {
                ccwdesc = "State: ~y~San Andreas~s~~n~";
            }
            ccwdesc += $"~n~Status: " + (Player.Licenses.CCWLicense.IsValid(Time) ? "~g~Valid~s~" : "~r~Expired~s~");
            ccwdesc += Player.Licenses.CCWLicense.ExpirationDescription(Time);
        }
        else
        {
            ccwdesc = "~r~No CCW Issued~s~";
        }


        if (Player.Licenses.HasPilotsLicense)
        {


            GameState issueState = LocationTypes.GetState(Player.Licenses.PilotsLicense.IssueStateID);
            if (issueState == null)
            {
                LocationTypes.GetState(StaticStrings.SanAndreasStateID);
            }
            if (issueState != null)
            {
                ccwdesc = $"State: ~y~{issueState.StateName}~s~~n~";
            }
            else
            {
                ccwdesc = "State: ~y~San Andreas~s~~n~";
            }


            pilotdesc += "~n~Endorsements:";

            if(Player.Licenses.PilotsLicense.IsRotaryEndorsed)
            {
                pilotdesc += " ~y~Rotary Wing Aircraft~s~";
            }
            if (Player.Licenses.PilotsLicense.IsFixedWingEndorsed)
            {
                pilotdesc += " ~y~Fixed Wing Aircraft~s~";
            }
            if (Player.Licenses.PilotsLicense.IsLighterThanAirEndorsed)
            {
                pilotdesc += " ~y~Lighter Than Air~s~";
            }
            pilotdesc += $"~n~Status: " + (Player.Licenses.CCWLicense.IsValid(Time) ? "~g~Valid~s~" : "~r~Expired~s~");
            pilotdesc += Player.Licenses.CCWLicense.ExpirationDescription(Time);
        }
        else
        {
            pilotdesc = "~r~No Pilots License Issued~s~";
        }


        dldesc += "~n~~n~Description: A legal authorization for a specific individual to operate one or more types of motorized vehicles such as motorcycles, cars, trucks, or buses—on a public road. Vehicle Operators caught without one will be fined.";
        ccwdesc += "~n~~n~Description: Allows Carrying a weapon (such as a handgun) in public in a concealed manner, either on one's person or in close proximity. Legal weapons are returned to owners after medical/bail services.";
        pilotdesc += "~n~~n~Description: Allows flying of fixed wing, lighter than air, and rotary aircraft.";
        TabItem dl = new TabTextItem("Drivers License", "Drivers License", dldesc);//TabItem tabItem = new TabTextItem($"{gr.Gang.ColorPrefix}{gr.Gang.FullName}~s~ {gr.ToBlip()}~s~", $"{gr.Gang.ColorPrefix}{gr.Gang.FullName}~s~", DescriptionText);
        items.Add(dl);

        TabItem ccw = new TabTextItem("CCW License", "CCW License", ccwdesc);//TabItem tabItem = new TabTextItem($"{gr.Gang.ColorPrefix}{gr.Gang.FullName}~s~ {gr.ToBlip()}~s~", $"{gr.Gang.ColorPrefix}{gr.Gang.FullName}~s~", DescriptionText);
        items.Add(ccw);

        TabItem pilots = new TabTextItem("Pilots License", "Pilots License", pilotdesc);//TabItem tabItem = new TabTextItem($"{gr.Gang.ColorPrefix}{gr.Gang.FullName}~s~ {gr.ToBlip()}~s~", $"{gr.Gang.ColorPrefix}{gr.Gang.FullName}~s~", DescriptionText);
        items.Add(pilots);

        TabView.AddTab(new TabSubmenuItem("Info", items));
    }
}

