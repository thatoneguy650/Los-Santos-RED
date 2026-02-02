using ExtensionsMethods;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Player.Activity;
using LSR.Vehicles;
using Microsoft.VisualBasic.CompilerServices;
using Rage;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Windows.Forms;
using System.Windows.Media;
using System.Xml.Serialization;

public class DebugMenu : ModUIMenu
{
    private UIMenu Debug;
    private IActionable Player;
    private RadioStations RadioStations;
    private IWeapons Weapons;
    private IPlacesOfInterest PlacesOfInterest;
    private ISettingsProvideable Settings;
    private ITimeControllable Time;
    private IEntityProvideable World;
    private ITaskerable Tasker;
    private MenuPool MenuPool;
    private Dispatcher Dispatcher;
    private IAgencies Agencies;
    private IGangs Gangs;
    private IModItems ModItems;
    private ICrimes Crimes;
    private INameProvideable Names;
    private IPoliceRespondable PoliceRespondable;
    private IPlateTypes PlateTypes;
    private ModDataFileManager ModDataFileManager;
    private List<DebugSubMenu> DebugSubMenus = new List<DebugSubMenu>();



    public DebugMenu(MenuPool menuPool, IActionable player, IWeapons weapons, RadioStations radioStations, IPlacesOfInterest placesOfInterest, ISettingsProvideable settings, ITimeControllable time, 
        IEntityProvideable world, ITaskerable tasker, Dispatcher dispatcher, IAgencies agencies, IGangs gangs, IModItems modItems, ICrimes crimes, IPlateTypes plateTypes, INameProvideable names, ModDataFileManager modDataFileManager, 
        IPoliceRespondable policeRespondable, IInteractionable interactionable)
    {
        Gangs = gangs;
        Dispatcher = dispatcher;
        Agencies = agencies;
        MenuPool = menuPool;
        Player = player;
        Weapons = weapons;
        RadioStations = radioStations;
        PlacesOfInterest = placesOfInterest;
        Settings = settings;
        Time = time;
        World = world;
        Tasker = tasker;
        ModItems = modItems;
        Crimes = crimes;
        PlateTypes = plateTypes;
        Names = names;
        ModDataFileManager = modDataFileManager;
        PoliceRespondable = policeRespondable;

        Debug = new UIMenu("Debug", "Debug Settings");
        Debug.SetBannerType(EntryPoint.LSRedColor);
        menuPool.Add(Debug);


        DebugSubMenus.Add(new DebugPlayerStateSubMenu(Debug, MenuPool, Player, Settings, Crimes, Tasker, World, Weapons, ModItems, Time, RadioStations, Names));
        DebugSubMenus.Add(new DebugTeleportSubMenu(Debug, MenuPool, Player, PlacesOfInterest, World, interactionable));
        DebugSubMenus.Add(new DebugInventorySubMenu(Debug, MenuPool, Player, Settings, Crimes, Tasker, World, Weapons, ModItems, Time, RadioStations, Names));
        DebugSubMenus.Add(new DebugWeaponsSubMenu(Debug, MenuPool, Player, Settings, Crimes, Tasker, World, Weapons, ModItems, Time, RadioStations, Names));
        DebugSubMenus.Add(new DebugLocationSubMenu(Debug, MenuPool, Player, World, Settings, ModDataFileManager.Streets, ModDataFileManager.PlacesOfInterest));
        DebugSubMenus.Add(new DebugVehicleSubMenu(Debug, MenuPool, Player, PlateTypes));
        DebugSubMenus.Add(new DebugMoneySubMenu(Debug, MenuPool, Player, Settings, Crimes, Tasker, World, Weapons, ModItems, Time, RadioStations, Names, ModDataFileManager));
        DebugSubMenus.Add(new DebugDispatcherSubMenu(Debug, MenuPool, Player, Agencies, Dispatcher, World, Gangs, ModDataFileManager.Organizations, settings));
        DebugSubMenus.Add(new DebugGangSubMenu(Debug, MenuPool, Player, Gangs, Dispatcher));
        DebugSubMenus.Add(new DebugMapSubMenu(Debug, MenuPool, Player, World, PlacesOfInterest, Settings, Time, PoliceRespondable, ModDataFileManager, Gangs));
        DebugSubMenus.Add(new DebugRelationshipSubMenu(Debug, MenuPool, Player, ModDataFileManager));
        DebugSubMenus.Add(new DebugOutfitSubMenu(Debug, MenuPool, Player));
        DebugSubMenus.Add(new DebugCrimeSubMenu(Debug, MenuPool, Player, Settings, Crimes, Tasker, World, Weapons));

        DebugSubMenus.Add(new DebugMovementSubMenu(Debug, MenuPool, Player));
        DebugSubMenus.Add(new DebugTimeSubMenu(Debug, MenuPool, Player, Time));

        DebugSubMenus.Add(new DebugAnimationSubMenu(Debug, MenuPool, Player, ModDataFileManager, this));
        DebugSubMenus.Add(new DebugHelperSubMenu(Debug, MenuPool, Player, World, PlacesOfInterest, Settings, Time, PoliceRespondable, ModDataFileManager, Gangs));
        DebugSubMenus.Add(new DebugTrunkSubMenu(Debug, MenuPool, Player, ModDataFileManager, World));
        DebugSubMenus.Add(new DebugPerformanceSubMenu(Debug, MenuPool, Player));
        DebugSubMenus.Add(new DebugPropAttachSubMenu(Debug, MenuPool, Player, ModDataFileManager, this));
        DebugSubMenus.Add(new DebugPositionLoggingSubMenu(Debug, MenuPool, Player, ModDataFileManager));
#if DEBUG
        DebugSubMenus.Add(new DebugLCYMAPSubMenu(Debug, MenuPool, Player));
#endif

        DebugSubMenus.Add(new DebugVehicleRaceSubMenu(Debug, MenuPool, Player, modDataFileManager.VehicleRaces, world, ModDataFileManager, interactionable));
    }
    public override void Hide()
    {
        Debug.Visible = false;
    }
    public override void Show()
    {
        if (!Debug.Visible)
        {
            DebugSubMenus.ForEach(x => x.Update());
            Debug.Visible = true;
        }
    }
    public override void Toggle()
    {
        if (!Debug.Visible)
        {
            DebugSubMenus.ForEach(x => x.Update());
            Debug.Visible = true;
        }
        else
        {
            Debug.Visible = false;
        }
    }
    public void Setup()
    {
        foreach (DebugSubMenu debugSubMenu in DebugSubMenus)
        {
            debugSubMenu.AddItems();
        }
    } 
    public string SelectedAnimationDictionary { get; set; }
    public string SelectedAnimationName { get; set; }
}