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

    public LicensesTab(IGangRelateable player, ITimeReportable time, TabView tabView)
    {
        Player = player;
        Time = time;
        TabView = tabView;
    }

    public void AddItems()
    {
        List<TabItem> items = new List<TabItem>();
        string dldesc = "";
        string ccwdesc = "";

        if (Player.Licenses.HasDriversLicense)
        {
            dldesc = "State: ~y~San Andreas~s~~n~";
            dldesc += $"~n~Status: " + (Player.Licenses.DriversLicense.IsValid(Time) ? "~g~Valid~s~" : "~r~Expired~s~");
            dldesc += Player.Licenses.DriversLicense.ExpirationDescription(Time);
        }
        else
        {
            dldesc = "~r~No Drivers License Issued~s~";
        }
        if (Player.Licenses.HasCCWLicense)
        {
            ccwdesc = "State: ~y~San Andreas~s~~n~";
            ccwdesc += $"~n~Status: " + (Player.Licenses.CCWLicense.IsValid(Time) ? "~g~Valid~s~" : "~r~Expired~s~");
            ccwdesc += Player.Licenses.CCWLicense.ExpirationDescription(Time);
        }
        else
        {
            ccwdesc = "~r~No CCW Issued~s~";
        }
        dldesc += "~n~~n~Description: A legal authorization for a specific individual to operate one or more types of motorized vehicles such as motorcycles, cars, trucks, or buses—on a public road. Vehicle Operators caught without one will be fined.";
        ccwdesc += "~n~~n~Description: Allows Carrying a weapon (such as a handgun) in public in a concealed manner, either on one's person or in close proximity. Legal weapons are returned to owners after medical/bail services.";

        TabItem dl = new TabTextItem("Drivers License", "Drivers License", dldesc);//TabItem tabItem = new TabTextItem($"{gr.Gang.ColorPrefix}{gr.Gang.FullName}~s~ {gr.ToBlip()}~s~", $"{gr.Gang.ColorPrefix}{gr.Gang.FullName}~s~", DescriptionText);
        items.Add(dl);

        TabItem ccw = new TabTextItem("CCW License", "CCW License", ccwdesc);//TabItem tabItem = new TabTextItem($"{gr.Gang.ColorPrefix}{gr.Gang.FullName}~s~ {gr.ToBlip()}~s~", $"{gr.Gang.ColorPrefix}{gr.Gang.FullName}~s~", DescriptionText);
        items.Add(ccw);

        TabView.AddTab(new TabSubmenuItem("Info", items));
    }
}

