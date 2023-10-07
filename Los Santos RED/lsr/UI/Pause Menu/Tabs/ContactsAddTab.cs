using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using RAGENativeUI.PauseMenu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class ContactsAddTab : ITabbableMenu
{
    private IPlacesOfInterest PlacesOfInterest;
    private IGangRelateable Player;
    private ITimeReportable Time;
    private ISettingsProvideable Settings;
    private IContacts Contacts;
    private MessagesMenu MessagesMenu;

    private TabSubmenuItem dynamicLocationsTabSubmenuITem;
    private string NumpadString = "";
    private TabTextItem ttx;
    private TabView TabView;
    private TabTextItem removeGPSTTI;

    public ContactsAddTab(IGangRelateable player, IPlacesOfInterest placesOfInterest, ITimeReportable time, ISettingsProvideable settings, TabView tabView, IContacts contacts, MessagesMenu messagesMenu)
    {
        Player = player;
        PlacesOfInterest = placesOfInterest;
        Time = time;
        Settings = settings;
        TabView = tabView;
        Contacts = contacts;
        MessagesMenu = messagesMenu;
    }

    public void AddItems()
    {
        NumpadString = "";
        List<TabItem> items = new List<TabItem>();
        ttx = new TabTextItem("Add Contact", "Add a contact by number", $"Add a contact by number. Select to enter the number. Include only numbers.");
        ttx.Activated += (s, e) =>
        {
            NumpadString = NativeHelper.GetKeyboardInput("");
            PhoneContact tocall = Contacts.GetContactByNumber(NumpadString);
            if (tocall == null)
            {
                Game.DisplaySubtitle($"No Contact Found for number: {NumpadString}");
            }
            else
            {
                Game.DisplaySubtitle($"Added Contact {tocall.Name} from number {NumpadString}.");
                Player.CellPhone.AddContact(tocall, false);
                MessagesMenu.RefreshMenu();
            }
            NumpadString = "";
        };
        items.Add(ttx);
        dynamicLocationsTabSubmenuITem = new TabSubmenuItem("Add Contact", items);
        TabView.AddTab(dynamicLocationsTabSubmenuITem);
    }
}

