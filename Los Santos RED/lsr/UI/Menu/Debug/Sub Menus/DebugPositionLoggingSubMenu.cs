using LosSantosRED.lsr;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Animation;


public class DebugPositionLoggingSubMenu : DebugSubMenu
{
    private Vector3 Offset;
    private Rotator Rotation;
    private bool isPrecise;
    private bool isRunning;
    private string FilterString;
    private UIMenu ModelSearchResultSubMenu;
    private ModDataFileManager ModDataFileManager;
    private TestAnimation SelectedAnimation;

    private float BlendIn = 8.0f;
    private float BlendOut = -8.0f;
    private int Time = -1;
    private int Flags = 0;
    private UIMenuItem playAnimationMenu;
    private UIMenuCheckboxItem IsFacialMenu;



    private Vector3 CurrentPosition;
    private float CurrentHeading;
    private UIMenuItem setPlayerMenu;
    private float HomeHeading;
    private Vector3 HomePosition;

    public UIMenuItem setPositionMenu { get; private set; }

    public DebugPositionLoggingSubMenu(UIMenu debug, MenuPool menuPool, IActionable player, ModDataFileManager modDataFileManager) : base(debug, menuPool, player)
    {
        ModDataFileManager = modDataFileManager;
    }
    public override void AddItems()
    {
        SubMenu = MenuPool.AddSubMenu(Debug, "Position Logging Menu");
        SubMenu.SetBannerType(EntryPoint.LSRedColor);
        Debug.MenuItems[Debug.MenuItems.Count() - 1].Description = "Position logging for anims.";
        SubMenu.Clear();
        SubMenu.Width = 0.5f;
        CreateMenu();

    }

    private void CreateMenu()
    {
        //Set position

        UIMenuItem setHomePositionMenu = new UIMenuItem("Set Home Position", "Set Home Position");
        setHomePositionMenu.Activated += (sender, selectedItem) =>
        {
            HomePosition = Game.LocalPlayer.Character.Position;
            HomeHeading = Game.LocalPlayer.Character.Heading;
        };
        SubMenu.AddItem(setHomePositionMenu);



        setPositionMenu = new UIMenuItem("Set Test Position", "Set test position");
        setPositionMenu.Activated += (sender, selectedItem) =>
        {
            CurrentPosition = Game.LocalPlayer.Character.Position;
            CurrentHeading = Game.LocalPlayer.Character.Heading;
        };
        SubMenu.AddItem(setPositionMenu);


        //Set Player
        UIMenuItem setPlayerHomeMenu = new UIMenuItem("Move Player Home", "Move Player to the current position");
        setPlayerHomeMenu.Activated += (sender, selectedItem) =>
        {
            SetPlayerAtHome();
        };
        SubMenu.AddItem(setPlayerHomeMenu);

        setPlayerMenu = new UIMenuItem("Move Player Test", "Move Player to the current position");
        setPlayerMenu.Activated += (sender, selectedItem) =>
        {
            SetPlayerAtCurrent();
        };
        SubMenu.AddItem(setPlayerMenu);


        UIMenuItem printPositionMenu = new UIMenuItem("Print Position", "Print the current position");
        printPositionMenu.Activated += (sender, selectedItem) =>
        {
            EntryPoint.WriteToConsole($"CURRENT POSITION {CurrentPosition} {CurrentHeading}");
        };
        SubMenu.AddItem(printPositionMenu);

        //Add Position X Y AND Z



        UIMenuNumericScrollerItem<float> xpositionChanger = new UIMenuNumericScrollerItem<float>("Change X Position", "", -1.0f, 1.0f, 0.1f) { Value = 0f };
        xpositionChanger.Activated += (sender,selectedItem) =>
        {
            CurrentPosition.X += xpositionChanger.Value;
            SetPlayerAtCurrent();
        };
        SubMenu.AddItem(xpositionChanger);


        UIMenuNumericScrollerItem<float> ypositionChanger = new UIMenuNumericScrollerItem<float>("Change Y Position", "", -1.0f, 1.0f, 0.1f) { Value = 0f };
        ypositionChanger.Activated += (sender, selectedItem) =>
        {
            CurrentPosition.Y += ypositionChanger.Value;
            SetPlayerAtCurrent();
        };
        SubMenu.AddItem(ypositionChanger);

        UIMenuNumericScrollerItem<float> zpositionChanger = new UIMenuNumericScrollerItem<float>("Change Z Position", "", -1.0f, 1.0f, 0.1f) { Value = 0f };
        zpositionChanger.Activated += (sender, selectedItem) =>
        {
            CurrentPosition.Z += zpositionChanger.Value;
            SetPlayerAtCurrent();
        };
        SubMenu.AddItem(zpositionChanger);


        //Add Heading
        UIMenuNumericScrollerItem<float> headingChanger = new UIMenuNumericScrollerItem<float>("Change Heading", "", -5.0f, 5.0f, 1f) { Value = 0f };
        headingChanger.Activated += (sender, selectedItem) =>
        {
            CurrentHeading += headingChanger.Value;
            SetPlayerAtCurrent();
        };
        SubMenu.AddItem(headingChanger);

        

    }
    private void SetPlayerAtCurrent()
    {
        Game.LocalPlayer.Character.Position = CurrentPosition;
        Game.LocalPlayer.Character.Heading = CurrentHeading;

        EntryPoint.WriteToConsole($"CURRENT POSITION {CurrentPosition} {CurrentHeading}");

    }
    private void SetPlayerAtHome()
    {
        Game.LocalPlayer.Character.Position = HomePosition;
        Game.LocalPlayer.Character.Heading = HomeHeading;

        EntryPoint.WriteToConsole($"CURRENT POSITION {HomePosition} {HomeHeading}");

    }

}

