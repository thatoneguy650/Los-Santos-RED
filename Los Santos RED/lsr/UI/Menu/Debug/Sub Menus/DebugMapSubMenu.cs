using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Player.Activity;
using LSR.Vehicles;
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
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Xml.Serialization;

public class DebugMapSubMenu : DebugSubMenu
{
    private bool IsBigMapActive;
    private IEntityProvideable World;
    private IPlacesOfInterest PlacesOfInterest;
    private ISettingsProvideable Settings;
    private ITimeControllable Time;
    private IPoliceRespondable PoliceRespondable;
    private ModDataFileManager ModDataFileManager;
    private IGangs Gangs;
    private UIMenu MapMenuItem;

    public DebugMapSubMenu(UIMenu debug, MenuPool menuPool, IActionable player, IEntityProvideable world, IPlacesOfInterest placesOfInterest, ISettingsProvideable settings, ITimeControllable time, IPoliceRespondable policeRespondable, ModDataFileManager modDataFileManager, IGangs gangs) : base(debug, menuPool, player)
    {
        World = world;
        PlacesOfInterest = placesOfInterest;
        Settings = settings;
        Time = time;
        PoliceRespondable = policeRespondable;
        ModDataFileManager = modDataFileManager;
        Gangs = gangs;
    }
    public override void AddItems()
    {
        MapMenuItem = MenuPool.AddSubMenu(Debug, "Map Menu");
        MapMenuItem.SetBannerType(EntryPoint.LSRedColor);
        Debug.MenuItems[Debug.MenuItems.Count() - 1].Description = "Various map items";

        UIMenuItem SetBigMap = new UIMenuItem("Toggle Big MiniMap", "Toggles the big GTAO style mini map");
        SetBigMap.Activated += (menu, item) =>
        {
            IsBigMapActive = !IsBigMapActive;
            NativeFunction.Natives.SET_BIGMAP_ACTIVE(IsBigMapActive, false);
            //Game.DisplaySubtitle($"IsBigMapActive:{IsBigMapActive}"); 
            menu.Visible = false;
        };
        MapMenuItem.AddItem(SetBigMap);

    }
}

