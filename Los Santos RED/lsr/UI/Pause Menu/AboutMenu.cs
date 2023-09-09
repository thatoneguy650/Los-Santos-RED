
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using RAGENativeUI.PauseMenu;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

public class AboutMenu
{
    private IGangRelateable Player;
    private ISettingsProvideable Settings;
    private TabView tabView;
    private ITimeReportable Time;
    public AboutMenu(IGangRelateable player, ITimeReportable time, ISettingsProvideable settings)
    {
        Player = player;
        Time = time;
        Settings = settings;
    }
    public void Setup()
    {
        tabView = new TabView("About Los Santos ~r~RED~s~");
        tabView.Tabs.Clear();
        tabView.OnMenuClose += (s, e) =>
        {
            Game.IsPaused = false;
        };
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
    public void Update()
    {
        tabView.Update();
        if (tabView.Visible)
        {
            tabView.Money = Time.CurrentTime;
        }
    }
    private void UpdateMenu()
    {
        tabView.MoneySubtitle = Player.BankAccounts.TotalAccountMoney.ToString("C0");
        tabView.Name = Player.PlayerName;
        tabView.Money = Time.CurrentTime;
        tabView.Tabs.Clear();
        tabView.ScrollTabs = true;

        AddActions();
        AddContacts();
        AddPedSwap();
        AddGangs();
        AddRespawn();
        AddPolice();
        AddCivilians();
        AddStores();
        tabView.RefreshIndex();
    }
    private void AddActions()
    {
        List<TabItem> items = new List<TabItem>();
        items.Add(new TabTextItem($"About", $"About", $"Many new actions have been added to allow the player to interact with the world in a variety of new ways. The majority of these actions are accessed either through the ~r~Main Menu~s~ -> ~r~Actions~s~ (general activites), through ~r~Main Menu~s~ -> ~r~Inventory~s~ (consumable items) or through the Action Wheel (quick access)."));
        TabItem ActionWheel = new TabTextItem($"Action Wheel", $"Action Wheel", $"A quick action wheel based on the radio/weapon wheel has been added allowing you quick access to a variety of items. Press {NativeHelper.FormatControls(Settings.SettingsManager.KeySettings.ActionPopUpDisplayKeyModifier, Settings.SettingsManager.KeySettings.ActionPopUpDisplayKey)} or {NativeHelper.FormatControls(Settings.SettingsManager.KeySettings.AltActionPopUpDisplayKeyModifier, Settings.SettingsManager.KeySettings.AltActionPopUpDisplayKey)} to open it and ~o~Attack Control~s~ to select an item");
        items.Add(ActionWheel);

        TabItem LootBody = new TabTextItem($"Search Bodies", $"Search Bodies", $"You can now search the bodies of dead or unconscious peds. They can have items and cash. A prompt will appear when looking at a searchable dbody");
        items.Add(LootBody);

        TabItem DragBody = new TabTextItem($"Drag Bodies", $"Drag Bodies", $"You can now drag the bodies of dead or unconscious peds. A prompt will appear when looking at a draggable dbody");
        items.Add(DragBody);

        TabItem Suicide = new TabTextItem($"Suicide", $"Suicide", "You now have the ability to commit suicide to quickly end a chase. Open the ~r~Main Menu~s~ -> ~r~Actions~s~ -> ~r~Suicide~s~ to commit suicide. If you have a pistol out, you will use the pistol, otherwise you will take a suicide pill.");
        TabItem Sitting = new TabTextItem($"Sitting", $"Sitting", "You now have the ability to sit in chairs/benches. Open the ~r~Main Menu~s~ -> ~r~Actions~s~ -> ~r~Sit Down~s~ when near a chair or bench. To sit anywhere use ~o~Here Backwards~s~ when you are not facing the place to sit or ~o~Here Forwards~s~ when you are.");
        TabItem Smoking = new TabTextItem($"Consuming Items", $"Consuming Items", "You now have the ability to smoke/eat/drink purchased items. Open the ~r~Main Menu~s~ -> ~r~Inventory~s~ and select a smokable/edible/drinkable item. The character will start consuming the item. Certain items will cause intoxication, others will regain health. Intoxication is displayed in the added ~b~blue bar~s~ under the armor.");
        items.Add(Sitting);
        items.Add(Smoking);
        TabItem Carjack = new TabTextItem($"Car Jacking", $"Car Jacking", "You can carjack vehicles with a weapon by tapping ~o~Vehicle Entry~s~ while holding a weapon instead of holding ~o~Vehicle Entry~s~. Can do regular entry by not having a weapon out or holding ~o~Vehicle Entry~s~.");
        TabItem CarLockPick = new TabTextItem($"Car Lock Picking", $"Car Lock Picking", "You can pick a vehicles lock instead of smashing the window by tapping ~o~Vehicle Entry~s~ instead of holding ~o~Vehicle Entry~s~. This will keep the vehicle from looking suspicious to law enforcement with the downside of taking longer. Optional setting to require you to purchase a screwdriver before allowing. Can do the regular smash enter by holding ~o~Vehicle Entry~s~.");
        TabItem PlateStealing = new TabTextItem($"Plate Swapping", $"Plate Swapping", "You now have the ability to change or remove a license plate from a vehicle. Open the ~r~Main Menu~s~ -> ~r~Actions~s~ -> ~r~Change Plate~s~/~r~Remove Plate~s~ when near a vehicle. Can be used to place clean plates on wanted vehicles.");
        items.Add(new TabTextItem($"Sprinting", $"Sprinting", $"The player can get an extra speed boost while jogging by pressing {NativeHelper.FormatControls(Settings.SettingsManager.KeySettings.SprintKeyModifier, Settings.SettingsManager.KeySettings.SprintKey)}. The added ~r~red bar~s~ under the health shows the current sprint stamina level."));
        items.Add(new TabTextItem($"Surrendering", $"Surrendering", $"The player can surrender to the police by pressing {NativeHelper.FormatControls(Settings.SettingsManager.KeySettings.SurrenderKeyModifier, Settings.SettingsManager.KeySettings.SurrenderKey)}"));
        items.Add(new TabTextItem($"Vehicle Controls", $"Vehicle Controls", $"The player can manually start and stop the vehicles engine with {NativeHelper.FormatControls(Settings.SettingsManager.KeySettings.EngineToggleModifier, Settings.SettingsManager.KeySettings.EngineToggle)}." +
            $"You can close the drive door with {NativeHelper.FormatControls(Settings.SettingsManager.KeySettings.ManualDriverDoorCloseModifier, Settings.SettingsManager.KeySettings.ManualDriverDoorClose)}. Indicators are used with " +
            $"Left: {NativeHelper.FormatControls(Settings.SettingsManager.KeySettings.LeftIndicatorKeyModifer, Settings.SettingsManager.KeySettings.LeftIndicatorKey)}, " +
            $"Right: {NativeHelper.FormatControls(Settings.SettingsManager.KeySettings.RightIndicatorKeyModifer, Settings.SettingsManager.KeySettings.RightIndicatorKey)}, " +
            $"Hazards: {NativeHelper.FormatControls(Settings.SettingsManager.KeySettings.HazardKeyModifer, Settings.SettingsManager.KeySettings.HazardKey)}. It can also be opened using the Action Wheel."));
        items.Add(new TabTextItem($"Fire Selection", $"Fire Selection", $"The player choose between a weapons firing modes (safe, semi, burst, auto) with {NativeHelper.FormatControls(Settings.SettingsManager.KeySettings.SelectorKeyModifier, Settings.SettingsManager.KeySettings.SelectorKey)}. It can also be opened using the Action Wheel."));
        items.Add(PlateStealing);
        items.Add(Carjack);
        items.Add(CarLockPick);
        items.Add(Suicide);
        tabView.AddTab(new TabSubmenuItem("Actions", items));
    }
    private void AddCivilians()
    {
        List<TabItem> items = new List<TabItem>();
        TabItem One = new TabTextItem($"Interactions", $"Interactions", "You can interact with almost any ped you come across on the street. You will see a prompt to talk with a ped when you are close enough and looking at them. Be careful insulting certain peds as they can react violently.");
        TabItem Three = new TabTextItem($"Reactions", $"Reactions", "Civlians will now react to more crimes committed by both you and other civilians around the world. They can either react by fleeing, fighting, calling the police, or any combination. Use the ~o~Crimes.xml~s~ file to make changes to which crimes civilians will react to and how they will react.");
        TabItem Four = new TabTextItem($"Transactions", $"Transactions", "You can buy or sell from peds that have or want items. You will see a prompt to transact with a ped when you are close enough and looking at them. Be careful buying illicit items as other civilians can call the police and report what they have seen.");
        items.Add(One);
        items.Add(Three);
        items.Add(Four);
        tabView.AddTab(new TabSubmenuItem("Civilians", items));
    }
    private void AddContacts()
    {
        List<TabItem> items = new List<TabItem>();
        TabItem About = new TabTextItem($"About", $"About", "To interact with NPCs around the world, you will need to use your burner phone. Contacts will be automatically added as they are discovered. Try interacting with Cop, Gang Members, and Shops to discover possible contacts.");
        TabItem One = new TabTextItem($"Burner Phone", $"Burner Phone", $"You can interact with people in the world using your burner cellphone. It can be activated with {NativeHelper.FormatControls(Settings.SettingsManager.KeySettings.SimplePhoneKeyModifer, Settings.SettingsManager.KeySettings.SimplePhoneKey)}. It can also be opened using the Action Wheel.");
        TabItem Three = new TabTextItem($"Pause Menu", $"Pause Menu", "There is a pause menu that allows you to interact with contacts and view messages you have received. It is accessed with the Action Wheel menu.");
        items.Add(About);
        items.Add(One);
        items.Add(Three);
        //
        tabView.AddTab(new TabSubmenuItem("Contacts", items));
    }
    private void AddGangs()
    {
        List<TabItem> items = new List<TabItem>();
        items.Add(new TabTextItem($"About", $"About", $"Gangs are now customizeable by the player including territory, peds, vehicles, safehouses, and more. Gangs now remember you and will react to you both positively or negatively based on your rep. To see gang information select ~r~Main Menu~s~ -> ~r~Player Information~s~. Customize gangs in the ~o~Gangs.xml~s~ file."));
        TabItem One = new TabTextItem($"Reputation", $"Reputation", "Gangs will now remember interactions you have with them. Friendly gangs give access to perks. Hostile gangs can attack the player on sight. Increase rep by helping out the gang, talking with them, or purchasing their illegal wares. Decrease rep by injuring/killing memebers, bringing heat to their turf, or insulting members.");
        TabItem Two = new TabTextItem($"Contacts", $"Contacts", "You will get contacts added to your phone and the player information pause menu (~r~Main Menu~s~ -> ~r~Player Information~s~). You can use these contacts to interact with the gang in a variety of ways depending on your reputation with them.");
        TabItem Three = new TabTextItem($"Messages", $"Messages", "You will get text messages added to your phone and the player information pause menu (~r~Main Menu~s~ -> ~r~Player Information~s~).");
        TabItem Four = new TabTextItem($"Specialties", $"Specialties", "Each gang has one or many specialties which they will be happy to sell to you for cheap if you are on friendly terms. Likewise some gangs will have non-specialty items, but at drastically higher prices. Shop around to see what each gang has.");
        TabItem Five = new TabTextItem($"Safehouses/Dens", $"Safehouses/Dens", "If you are on friendly terms with a gang, they will invite you to visit thier safehouse/den. This location can be used to purchase illicit items more discreetly and for a better price.");
        TabItem Six = new TabTextItem($"Territory", $"Territory", "Each gang can control territory which determines where they spawn and patrol. A zone can also have multiple gangs while including a primary gang. The player can change any of these items in the ~o~GangTerritories.xml~s~ file.");
        items.Add(One);
        items.Add(Two);
        items.Add(Three);
        items.Add(Four);
        items.Add(Five);
        items.Add(Six);
        tabView.AddTab(new TabSubmenuItem("Gangs", items));
    }
    private void AddPedSwap()
    {
        List<TabItem> items = new List<TabItem>();
        items.Add(new TabTextItem($"About", $"About", $"You can now choose to play as any ped in the game including the freemode chracters. The mod uses model aliasing to allow you to spend money and have the game treat random peds the same as the main characters and spend money, get random encounters, etc. The chracter you create or take over can have a customized name, weapons, vehicles, criminal records, and more."));
        TabItem CustomPed = new TabTextItem($"Customize Ped", $"Customize Ped", "You can choose any model in the game and customize the variation from props and componenets to headblend data and overlays. Accessed by ~r~Main Menu~s~ -> ~r~Ped Swap~s~ -> ~r~Become Custom Pedestrian~s~.");
        TabItem RandomPed = new TabTextItem($"Become Any/Random Ped", $"Random Ped", "You can become any civilian ped you see in the streets (or can exist) and get a generated name, vehicle, bank balance, weapons, criminal history, and more. Accessed by ~r~Main Menu~s~ -> ~r~Ped Swap~s~ -> ~r~Takeover Random Pedestrian~s~/~r~Become Random Pedestrian~s~.");
        TabItem RandomCop = new TabTextItem($"Become Cop", $"Become Cop", "Become a random cop around the world. Very basic, might be expanded. Might cause other issues. Accessed by ~r~Main Menu~s~ -> ~r~Ped Swap~s~ -> ~r~Become Random Cop~s~.");
        items.Add(CustomPed);
        items.Add(RandomPed);
        //items.Add(RandomCop);

        tabView.AddTab(new TabSubmenuItem("Ped Swap", items));
    }
    private void AddPolice()
    {
        List<TabItem> items = new List<TabItem>();
        items.Add(new TabTextItem($"About", $"About", $"Police are now customizeable by the player including jurisdiction, peds, vehicles, and more. The player can customize the police in the ~o~Agencies.xml~s~ file."));
        TabItem One = new TabTextItem($"Dispatch/Jurisdiction", $"Dispatch/Jurisdiction", "The existing dispatch system has been completely replaced with a script based system. You can customize who responds in any location by making changes to the ~o~CountyJurisdictions.xml~s~, or ~o~ZoneJurisdictions.xml~s~ files.");
        TabItem Two = new TabTextItem($"Investigation", $"Investigation", "When civilians call in a crime, the police will first need to respond and determine if any crimes are taking place before you get a wanted level. The speed of the response is based on the severity of the crime reported. If your description has been issued, the police will arrest you if you are in the general vicinity of the area. Otherwise you can avoid arrest by acting normal until they pass.");
        TabItem Three = new TabTextItem($"Criminal History", $"Criminal History", "Police will now remember you and your vehicle after a chase for a period of time. This is called a BOLO (be on the look out) in the mod and means that if you are seen near the last area or are recognized by the police for a period of time, the chase will pickup right where it left off. Use Hotels to quickly expire BOLOs from the safety of a hotel room.");
        TabItem Four = new TabTextItem($"Bribes/Fines", $"Bribes/Fines", "You can now pay fines for petty crimes and bribe police to let you go for more serious offences. The amount they expect is based on the severity of the crime. If you are too cheap, they will take the bribe AND still arrest you.");
        TabItem Five = new TabTextItem($"Response", $"Response", "Police will now chase you more realistically and will only escalate to deadly force when they are threatened or attacked. Tasers are used properly when being chased on foot." +
            "");
        items.Add(One);
        items.Add(Two);
        items.Add(Three);
        items.Add(Four);
        items.Add(Five);
        tabView.AddTab(new TabSubmenuItem("Police", items));
    }
    private void AddRespawn()
    {
        List<TabItem> items = new List<TabItem>();
        items.Add(new TabTextItem($"About", $"About", $"The existing respawn system has been removed and replaced with a choose-your-respawn menu system. When you die or are busted, a menu will open allowing you to choose how you respawn. This menu can be re-opened by pressing ~o~{Settings.SettingsManager.KeySettings.MenuKey}~s~ when you are dead or busted. It includes vanilla like options in addition to several more choices."));
        items.Add(new TabTextItem($"Resist Arrest", $"Resist Arrest", "Restarts the chase right where it left off and tasks the cops with arresting you again. When you have more than one or two cops near you this option almost always fails. If they get distracted by other criminals or civilians, you can use this option to slip away."));
        items.Add(new TabTextItem($"Bribe Police", $"Bribe Police", "Gives the player a prompt to choose how much money to bribe the police with. Too little and they will confiscate it and still arrest you. Too much and you are wasting money. If you are successful, you will get a short grace period to get out of their sight before they will attempt to recapture you."));
        items.Add(new TabTextItem($"Pay Citation", $"Pay Citation", "When you are caught for low level crimes (One Star) you will have the option to pay a small fine and be on your way. You will also get a small grace period like paying a bribe."));
        items.Add(new TabTextItem($"Surrender", $"Surrender", "The police will take you back to the station before letting you out on bail (with bail fees). Similar to vanilla behavior however you can choose which station to get booked at (the closest station is selected by default)."));
        items.Add(new TabTextItem($"Takeover Random Pedestrian", $"Takeover Rnd Ped", "Instead of facing the music, you can swap from your current persona to another ped on the street and forget all about your past life. If you are curious you might be able to see you being carted away or killed."));
        items.Add(new TabTextItem($"Un-Die", $"Un-Die", "Will call a mulligan on the chase and allow you to respawn instantly as your existing chracter in the exact same position. Useful to keep an interesting chase going until YOU decide it is over."));
        items.Add(new TabTextItem($"Give Up", $"Give Up", "The medics will take you back to the hospital where you will be billed and discharged. Similar to vanilla behavior however you can choose which hospital you get treated at (the closest hospital is selected by default)."));
        tabView.AddTab(new TabSubmenuItem("Respawn", items));
    }
    private void AddStores()
    {
        List<TabItem> items = new List<TabItem>();
        TabItem AboutStores = new TabTextItem($"About", $"About", "Interactable locations including stores, hotels, drive-thus, and more have been added around Los Santos. Locations and Menus can be customized or added by making edits to the ~o~Locations.xml~s~ and ~o~ShopMenus.xml~s~ files.");

        TabItem One = new TabTextItem($"Restuarants", $"Restuarants", "Allows you to purchase food and drink. Some items you can take to go, others must be consumed on site. Items can allow you to regain health or cause intoxication.");
        TabItem Two = new TabTextItem($"Liquor Stores", $"Liquor Stores", "Allows you to purchase a variety of intoxicating drinks for both consuming on site and taking to go. Be careful as the police and civilians are aware of public intoxication now.");
        TabItem Three = new TabTextItem($"Hotels", $"Hotels", "Allows you to get off the street for the night to let your BOLO expire after a hectic chase. Will also regain some health.");
        TabItem Four = new TabTextItem($"Gun Shops", $"Gun Shops", "Allows you to purchase items not available at AmmuNation and in some cases for a better price.");
        TabItem Five = new TabTextItem($"Scrap Yards", $"Scrap Yards", "Allows you to scrap any vehicle to make some quick money from all those stolen vehicles.");
        TabItem Six = new TabTextItem($"Car Dealerships", $"Car Dealerships", "Allows you to purchase vehicles (including DLC) that will be added to your owned vehicle list.");
        items.Add(AboutStores);
        items.Add(One);
        items.Add(Two);
        items.Add(Three);
        items.Add(Four);
        items.Add(Five);
        items.Add(Six);
        tabView.AddTab(new TabSubmenuItem("Locations", items));
    }
}

