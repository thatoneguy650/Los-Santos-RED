using ExtensionsMethods;
using iFruitAddon2;
using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Locations;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using RAGENativeUI.Elements;
using RAGENativeUI.PauseMenu;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class AboutMenu
{
    private TabView tabView;

    private TabItemSimpleList simpleListTab;
    private TabMissionSelectItem missionSelectTab;
    private TabTextItem textTab;
    private TabSubmenuItem submenuTab;
    private IGangRelateable Player;
    private ITimeReportable Time;
    private IPlacesOfInterest PlacesOfInterest;
    private IGangs Gangs;
    private IGangTerritories GangTerritories;
    private IZones Zones;
    private IStreets Streets;
    private IInteriors Interiors;
    private IEntityProvideable World;
    public AboutMenu(IGangRelateable player, ITimeReportable time, IPlacesOfInterest placesOfInterest, IGangs gangs, IGangTerritories gangTerritories, IZones zones, IStreets streets, IInteriors interiors, IEntityProvideable world)
    {
        Player = player;
        Time = time;
        PlacesOfInterest = placesOfInterest;
        Gangs = gangs;
        GangTerritories = gangTerritories;
        Zones = zones;
        Streets = streets;
        Interiors = interiors;
        World = world;
    }
    public void Setup()
    {
        tabView = new TabView("About Los Santos ~r~RED~s~");
        tabView.Tabs.Clear();
        tabView.OnMenuClose += TabView_OnMenuClose;
    }

    private void TabView_OnMenuClose(object sender, EventArgs e)
    {

        Game.IsPaused = false;
    }

    public void Update()
    {
        tabView.Update();
        if (tabView.Visible)
        {
            tabView.Money = Time.CurrentTime;
        }
    }
    public void Toggle()
    {
        if (!TabView.IsAnyPauseMenuVisible)
        {
            if (!tabView.Visible)
            {
                UpdateMenu();
                Game.IsPaused = true;
            }
            tabView.Visible = !tabView.Visible;
        }
    }
    private void UpdateMenu()
    {
        tabView.MoneySubtitle = Player.Money.ToString("C0");
        tabView.Name = Player.PlayerName;
        tabView.Money = Time.CurrentTime;
        tabView.Tabs.Clear();

        AddActivities();
        AddPedSwap();
        AddGangs();
        AddPolice();
        AddStores();
        tabView.RefreshIndex();
    }

    private void AddActivities()
    {
        List<TabItem> items = new List<TabItem>();
        TabItem Sitting = new TabTextItem($"Sitting", $"Sitting", "You now have the ability to sit in chairs/benches. Open the ~r~Main Menu~s~ and select ~r~Actions~s~ -> ~r~Sit Down~s~ when near a chair or bench. To sit anywhere use ~o~Here Backwards~s~ when you are not facing the place to sit or ~o~Here Forwards~s~ when you are.");
        TabItem Smoking = new TabTextItem($"Smoking", $"Smoking", "You now have the ability to smoke purchased items. Open the ~r~Main Menu~s~ and select ~r~Inventory~s~ and select a smokable item. The chracter will start smaoking the item. Certain items will cause intoxication.");
        TabItem Eating = new TabTextItem($"Eating", $"Eating", "You now have the ability to eat purchased items. Open the ~r~Main Menu~s~ and select ~r~Inventory~s~ and select a food item. The chracter will start consuming the food and you can gain health.");
        TabItem Drinking = new TabTextItem($"Drinking", $"Drinking", "You now have the ability to drink purchased items. Open the ~r~Main Menu~s~ and select ~r~Inventory~s~ and select a drink item. The chracter will start drinking the item and you can gain health. Certain items will cause intoxication.");


        TabItem Carjack = new TabTextItem($"Car Jacking", $"Car Jacking", "You can carjack vehicles with a weapon by tapping ~o~Vehicle Entry~s~ while holding a weapon instead of holding ~o~Vehicle Entry~s~. Can do regular entry by not having a weapon out or holding ~o~Vehicle Entry~s~.");
        TabItem CarLockPick = new TabTextItem($"Car Lock Picking", $"Car Lock Picking", "You can pick a vehicles lock instead of smashing the window by tapping ~o~Vehicle Entry~s~ instead of holding ~o~Vehicle Entry~s~. This will keep the vehicle from looking suspicious to law enforcement with the downside of taking longer. Optional setting to require you to purchase a screwdriver before allowing. Can do the regular smash enter by holding ~o~Vehicle Entry~s~.");
        TabItem PlateStealing = new TabTextItem($"Plate Swapping", $"Plate Swapping", "You now have the ability to change or remove a license plate from a vehicle. Open the ~r~Main Menu~s~ and select ~r~Actions~s~ -> ~r~Change Plate~s~ or ~r~Remove Plate~s~ when near a vehicle. Can be used to place clean plates on wanted vehicles.");

        items.Add(Sitting);
        items.Add(Smoking);
        items.Add(Eating);
        items.Add(Drinking);
        items.Add(PlateStealing);


        items.Add(Carjack);
        items.Add(CarLockPick);
        tabView.AddTab(submenuTab = new TabSubmenuItem("Actions", items));
    }
    private void AddPedSwap()
    {
        List<TabItem> items = new List<TabItem>();
        TabItem CustomPed = new TabTextItem($"Customize Ped", $"Customize Ped", "You can choose any model in the game and customize the variation from props and componenets to headblend data and overlays. Select 'Ped Swap' on the main menu then choose 'Become Custom Pedestrian' to get started");
        TabItem RandomPed = new TabTextItem($"Become Any/Random Ped", $"Random Ped", "You can become any civilian ped you see in the streets (or can exist) and get a generated name, vehicle, bank balance, weapons, criminal history, etc. Select 'Ped Swap' on the main menu then choose 'Takeover Random Pedestrian' or 'Become Random Pedestrian' to get started");
        TabItem RandomCop = new TabTextItem($"Become Cop", $"Become Cop", "Become a random cop around the world. Very basic, might be expanded. Might cause other issues.");
        items.Add(CustomPed);
        items.Add(RandomPed);
        items.Add(RandomCop);
        tabView.AddTab(submenuTab = new TabSubmenuItem("Ped Swap", items));
    }
    private void AddGangs()
    {
        List<TabItem> items = new List<TabItem>();
        TabItem One = new TabTextItem($"Reputation", $"Reputation", "Gangs will now remember interactions you have with them. Friendly gangs give access to perks. Hostile gangs can attack the player on sight. Increase rep by helping out the gang, talking with them, or purchasing their illegal wares. Decrease rep by injuring/killing memebers, bringing heat to their turf, or insulting members.");
        TabItem Two = new TabTextItem($"Contacts", $"Contacts", "You will get contacts added to your phone and the Player Information pause menu (accessed by the main menu). You can use these contacts to interact with the gang in a variety of ways depending on your reputation with them.");
        TabItem Three = new TabTextItem($"Messages", $"Messages", "You will get text messages added to your phone and the Player Information pause menu (accessed by the main menu).");
        TabItem Four = new TabTextItem($"Specialties", $"Specialties", "Each gang has one or many specialties which they will be happy to sell to you for cheap if you are on friendly terms. Likewise some gangs will have non-specialty items, but at drastically higher prices. Shop around to see what each gang has.");
        TabItem Five = new TabTextItem($"Safehouses/Dens", $"Safehouses/Dens", "If you are on friendly terms with a gang, they will invite you to visit thier safehouse/den. This location can be used to purchase illicit items more discreetly and for a better price.");
        TabItem Six = new TabTextItem($"Territory", $"Territory", "Each gang can control territory which determines where they spawn and patrol. A zone can also have multiple gangs while including a primary gang. The player can change any of these items in the ~o~GangTerritories.xml~s~ file.");
        items.Add(One);
        items.Add(Two);
        items.Add(Three);
        items.Add(Four);
        items.Add(Five);
        items.Add(Six);
        tabView.AddTab(submenuTab = new TabSubmenuItem("Gangs", items));
    }

    private void AddPolice()
    {
        List<TabItem> items = new List<TabItem>();
        TabItem One = new TabTextItem($"Dispatch/Jurisdiction", $"Dispatch/Jurisdiction", "The existing dispatch system has been completely replaced with a script based system. You can customize who responds in any location by making changes to the ~o~CountyJurisdictions.xml~s~, or ~o~ZoneJurisdictions.xml~s~ files.");
        TabItem Two = new TabTextItem($"Investigation", $"Investigation", "When civilians call in a crime, the police will first need to respond and determine if any crimes are taking place before you get a wanted level. The speed fo the response is based on the severity of the crime reported. If your description has been issued, the police will arrest you if you are in the general vicinity of the area. Otherwise you can avoid arrest by acting normal until they pass.");
        TabItem Three = new TabTextItem($"Criminal History", $"Criminal History", "Police will now remember you and your vehicle after a chase for a period of time. This is called a BOLO (be on the look out) in the mod and means that if you are seen near the last area or are recognized by the police for a period of time, the chase will pickup right where it left off. Use Hotels to quickly expire BOLOs from the safety of a hotel room.");
        TabItem Four = new TabTextItem($"Bribes/Fines", $"Bribes/Fines", "You can now pay fines for petty crimes and bribe police to let you go for more serious offences. The amount they expect is based on the severity of the crime. If you are too cheap, they will take the bribe AND still arrest you. =");
        TabItem Five = new TabTextItem($"Response", $"Response", "Police will now react more realistically to petty crimes. You can pay fines at one star or below without being arrested and transfered to the station. " +
            "If you choose to resist or flee, police will now chase you more realistically and will only escalte to deadly force/ramming your vehicle when they are threatened or attacked. Police will now properly use thier tasers when chasing you on foot and are much more difficult to get away from.");
        //TabItem Six = new TabTextItem($"Jurisdiction", $"Jurisdiction", "");
        items.Add(One);
        items.Add(Two);
        items.Add(Three);
        items.Add(Four);
        items.Add(Five);
        //items.Add(Six);
        tabView.AddTab(submenuTab = new TabSubmenuItem("Police", items));
    }
    private void AddCivilians()
    {
        List<TabItem> items = new List<TabItem>();
        TabItem One = new TabTextItem($"Interactions", $"Interactions", "You can interact with almost any ped you come across on the street. You will see a prompt to talk with a ped when you are close enough and looking at them. Be careful insulting certain peds as they can react violently.");
        //TabItem Two = new TabTextItem($"AI", $"AI", "");
        TabItem Three = new TabTextItem($"Reactions", $"Reactions", "Civlians will now react to more crimes committed by both you and other civilians around the world. They can either react by fleeing, fighting, calling the police, or any combination. Use the Crimes.xml file to make changes to which crimes civilians will react to and how they will react.");
        TabItem Four = new TabTextItem($"Transactions", $"Transactions", "You can buy or sell from peds that have or want items. You will see a prompt to transact with a ped when you are close enough and looking at them. Be careful buying illicit items as other civilians can call the police and report what they have seen.");
        //TabItem Five = new TabTextItem($"Response", $"Response", "");
        //TabItem Six = new TabTextItem($"Territory", $"Safehouses/Dens", "");
        items.Add(One);
       // items.Add(Two);
        items.Add(Three);
        items.Add(Four);
        //items.Add(Five);
        //items.Add(Six);
        tabView.AddTab(submenuTab = new TabSubmenuItem("Civilians", items));
    }
    private void AddStores()
    {
        List<TabItem> items = new List<TabItem>();
        TabItem One = new TabTextItem($"Restuarants", $"Restuarants", "Restaurants have been added around Los Santos that allow you to purchase food and drink. Some items you can take to go, others must be consumed on site. Items can allow you to regain health, or cause intoxication. Can change any Item/Price in ~o~ShopMenus.xml~s~");
        TabItem Two = new TabTextItem($"Liquor Stores", $"Liquor Stores", "Liquor stores and bars have been added. You can purchase and drink alcohol in many forms. Be careful as the police and civilians are aware of public intoxication now. Can change any Item/Price in ~o~ShopMenus.xml~s~");
        TabItem Three = new TabTextItem($"Hotels", $"Hotels", "Stay at any of the hotels in order to let your BOLO expire after a hectic chase. Will also regain some health.");
        TabItem Four = new TabTextItem($"Gun Shops", $"Gun Shops", "Underground gunshops have been added which will allow you to purchase items not available at AmmuNation. Can change any Item/Price in ~o~ShopMenus.xml~s~");
        TabItem Five = new TabTextItem($"Scrap Yards", $"Scrap Yards", "You can now scrap any vehicle at several scray yards. Will give you a way to make some quick money from all those stolen vehicles.");
        TabItem Six = new TabTextItem($"Car Dealerships", $"Car Dealerships", "Car dealerships have been added which allow you to purchase vehicles that will be added to your owned vehicle list. Can change any Item/Price in ~o~ShopMenus.xml~s~");
        items.Add(One);
        items.Add(Two);
        items.Add(Three);
        items.Add(Four);
        items.Add(Five);
        items.Add(Six);
        tabView.AddTab(submenuTab = new TabSubmenuItem("Stores", items));
    }
}

