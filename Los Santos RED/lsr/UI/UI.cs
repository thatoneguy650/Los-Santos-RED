using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using RAGENativeUI.PauseMenu;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

public class UI : IMenuProvideable
{
    private IDisplayable DisplayablePlayer;
    private IActionable ActionablePlayer;
    private IJurisdictions Jurisdictions;
    private ILocationTypes LocationTypes;
    private ITaskerable Tasker;
    private ITimeControllable Time;
    private IEntityProvideable World;
    private ISettingsProvideable Settings;

    private LowerRightDisplay LowerRightDisplay;
    private TopRightMenu TopRightMenu;
    private MarkerManager MarkerManager;
    private PopUpMenu ActionPopUpMenu;

    private TimerBarPool TimerBarPool;
    private TimerBarController TimerBarController;

    private BigMessageThread BigMessage;
    
    private MenuPool MenuPool;
    private List<ModUIMenu> MenuList;
    private MainMenu MainMenu;
    private BustedMenu BustedMenu;
    private DeathMenu DeathMenu;
    private DebugMenu DebugMenu;
    private CraftingMenu CraftingMenu;

    private List<ButtonPrompt> displayedButtonPrompts = new List<ButtonPrompt>();
    private InstructionalButtons instructional = new InstructionalButtons();
    
    private bool IsShowingCustomOverlay = false;
    private bool StartedBustedEffect = false;
    private bool StartedDeathEffect = false;
    private string debugString1;
    private bool ShowRadar;
    private uint SpriteUint;
    private uint SpriteUintCounter;
    private uint PreviousGameTime;
    private bool TimeOutSprite;
    private bool IsDisposed = false;
    private bool IsNotShowingFullScreenMenus;
    private bool DrawSpeedLimitTexture = true;
    private uint GameTimeLastBusted;
    private uint GameTimeLastDied;

    public AboutMenu AboutMenu { get; private set; }
    public PlayerInfoMenu PlayerInfoMenu { get; private set; }
    public SavePauseMenu SavePauseMenu { get; private set; }
    public MessagesMenu MessagesMenu { get; private set; }

    public bool IsDisplayingMenu => MenuPool.IsAnyMenuOpen();
    public string CurrentDefaultTextColor { get; private set; }= "~c~";
    public bool IsPressingActionWheelButton { get; set; }
    public bool IsDrawingWheelMenu => ActionPopUpMenu.IsActive;
    public bool IsNotShowingFrontEndMenus { get; private set; } = false;
    public int InstructionalsShowing => instructional.Buttons.Count;
    public int TimeBarsShowing => TimerBarController.ItemsDisplaying;
    public CraftableItems CraftableItems { get; set; }
    public Mod.Crafting Crafting { get; set; }

    public UI(IDisplayable displayablePlayer, ISettingsProvideable settings, IJurisdictions jurisdictions, IPedSwap pedSwap, IPlacesOfInterest placesOfInterest, IRespawning respawning, IActionable actionablePlayer, ISaveable saveablePlayer, IWeapons weapons, 
        RadioStations radioStations, IGameSaves gameSaves, IEntityProvideable world, IRespawnable player, IPoliceRespondable policeRespondable, ITaskerable tasker, IInventoryable playerinventory, IModItems modItems, ITimeControllable time, IGangRelateable gangRelateable, 
        IGangs gangs, IGangTerritories gangTerritories, IZones zones, IStreets streets, IInteriors interiors, Dispatcher dispatcher, IAgencies agencies, ILocationInteractable locationInteractableplayer, IDances dances, IGestures gestures, IShopMenus shopMenus, 
        IActivityPerformable activityPerformable, ICrimes crimes, ILocationTypes locationTypes, IIntoxicants intoxicants, IPlateTypes plateTypes, INameProvideable names, ModDataFileManager modDataFileManager, IInteractionable interactionable, Mod.Crafting crafting)
    {
        DisplayablePlayer = displayablePlayer;
        ActionablePlayer = actionablePlayer;
        Settings = settings;
        Jurisdictions = jurisdictions;
        Time = time;
        Tasker = tasker;
        World = world;
        LocationTypes = locationTypes;
        //BigMessage = new BigMessageThread(true);
        MenuPool = new MenuPool();
        TimerBarPool = new TimerBarPool();
        DeathMenu = new DeathMenu(MenuPool, pedSwap, respawning, placesOfInterest, Settings, player, gameSaves);
        BustedMenu = new BustedMenu(MenuPool, pedSwap, respawning, placesOfInterest, Settings, policeRespondable, time);
        MainMenu = new MainMenu(MenuPool,actionablePlayer, locationInteractableplayer, saveablePlayer, gameSaves, weapons, pedSwap, world, Settings, Tasker, playerinventory, modItems, this, gangs, time,placesOfInterest, dances, gestures, activityPerformable,agencies, crimes, intoxicants, shopMenus);
        DebugMenu = new DebugMenu(MenuPool, actionablePlayer, weapons, radioStations, placesOfInterest, Settings, Time, World, Tasker, dispatcher,agencies, gangs, modItems, crimes, plateTypes, names, modDataFileManager, policeRespondable, interactionable);
        CraftableItems = modDataFileManager.CraftableItems;
        Crafting = crafting;
        CraftingMenu = new CraftingMenu(MenuPool, CraftableItems, Crafting);
        MenuList = new List<ModUIMenu>() { DeathMenu, BustedMenu, MainMenu, DebugMenu, CraftingMenu };
        PlayerInfoMenu = new PlayerInfoMenu(gangRelateable, Time, placesOfInterest, gangs, gangTerritories, zones, streets, interiors, World, shopMenus,modItems, weapons, Settings, LocationTypes);
        SavePauseMenu = new SavePauseMenu(saveablePlayer, Time, placesOfInterest, gangs, gangTerritories, zones, streets, interiors, World, shopMenus, modItems, weapons, Settings, gameSaves, pedSwap,playerinventory, saveablePlayer, agencies, modDataFileManager.Contacts, interactionable);
        MessagesMenu = new MessagesMenu(gangRelateable, Time, placesOfInterest, gangs, gangTerritories, zones, streets, interiors, World, Settings, modDataFileManager.Contacts);
        AboutMenu = new AboutMenu(gangRelateable, Time, Settings);
        ActionPopUpMenu = new PopUpMenu(actionablePlayer, Settings, this, gestures, dances);
        TimerBarController = new TimerBarController(displayablePlayer, TimerBarPool, Settings);
        MarkerManager = new MarkerManager(locationInteractableplayer, World, Time, Settings);
        LowerRightDisplay = new LowerRightDisplay(DisplayablePlayer,Time,Settings,this);
        TopRightMenu = new TopRightMenu(DisplayablePlayer, Time, Settings, this);
    }
    public void Setup()
    {
        IsDisposed = false;
        BustedMenu.Setup();
        DeathMenu.Setup();
        MainMenu.Setup();
        PlayerInfoMenu.Setup();
        SavePauseMenu.Setup();
        MessagesMenu.Setup();
        AboutMenu.Setup();
        ActionPopUpMenu.Setup();
        TimerBarController.Setup();
        MarkerManager.Setup();
        LowerRightDisplay.Setup();
        TopRightMenu.Setup();
        CraftingMenu.Setup();
    }
    public void Dispose()
    {
        IsDisposed = true;
        ActionPopUpMenu.Dispose();
        TimerBarController.Dispose();
        MarkerManager.Dispose();
        LowerRightDisplay.Dispose();
        TopRightMenu.Dispose();
        NativeFunction.CallByName<bool>("DISPLAY_RADAR", true);
        NativeFunction.Natives.xB9EFD5C25018725A("DISPLAY_HUD", true);
    }
    public void SetupDebugMenu()
    {
        DebugMenu.Setup();
    }
    public void MenuOnly()
    {
        MenuPool.ProcessMenus();
    }
    public void Tick1()
    {
        IsNotShowingFrontEndMenus = !MenuPool.IsAnyMenuOpen() && !TabView.IsAnyPauseMenuVisible && !EntryPoint.ModController.IsDisplayingAlertScreen;
        IsNotShowingFullScreenMenus = !TabView.IsAnyPauseMenuVisible && !EntryPoint.ModController.IsDisplayingAlertScreen;

        if (DisplayablePlayer.IsNotShowingFrontEndMenus != IsNotShowingFrontEndMenus)
        {      
            DisplayablePlayer.IsNotShowingFrontEndMenus = IsNotShowingFrontEndMenus;
            DisplayablePlayer.ButtonPrompts.Update();
        }

        ProcessActionWheel();
        LowerRightDisplay.Display();
        if (!EntryPoint.ModController.IsDisplayingAlertScreen)
        {
            TopRightMenu.Display();
        }
        MenuUpdate();
        //MarkerManager.Update();
    }
    public void Tick2()
    {
        DisplayButtonPrompts();
        ForceVanillaUI();
        MarkerManager.Update();
    }
    public void Tick3()
    {
        GameFiber.Yield();
        ScreenEffectsUpdate();
        GameFiber.Yield();
        RadarUpdate();
        GameFiber.Yield();
    }
    public void CacheData()
    {
        if (DisplayablePlayer.IsAliveAndFree)
        {
            if (Time.IsNight && Settings.SettingsManager.UIGeneralSettings.GreyOutWhiteFontAtNight)
            {
                CurrentDefaultTextColor = Settings.SettingsManager.UIGeneralSettings.DefaultTextColorNight;
                //CurrentDefaultTextColor = "~c~";
            }
            else
            {
                CurrentDefaultTextColor = Settings.SettingsManager.UIGeneralSettings.DefaultTextColor;
                //CurrentDefaultTextColor = "~s~";
            }
            LowerRightDisplay.CacheData();
            TopRightMenu.CacheData();
        }
        TimerBarController.Update();
    }
    private void ProcessActionWheel()
    {
        if (IsPressingActionWheelButton)
        {
            if(!ActionPopUpMenu.IsActive && !ActionPopUpMenu.RecentlyClosed && DisplayablePlayer.IsAliveAndFree && !DisplayablePlayer.ActivityManager.IsInteractingWithLocation)
            {
                ActionPopUpMenu.ShowMenu();
            }
        }
        else
        {
            if (Settings.SettingsManager.ActionWheelSettings.RequireButtonHold)
            {
                if (ActionPopUpMenu.IsActive)
                {
                    ActionPopUpMenu.CloseMenu();
                }
            }
        }
        if (IsNotShowingFullScreenMenus && Settings.SettingsManager.UIGeneralSettings.IsEnabled)
        {
            ActionPopUpMenu.Draw();
        }
        DisplayablePlayer.IsShowingActionWheel = ActionPopUpMenu.IsActive;
    }
    private void DisplayButtonPrompts()
    {
        if (Settings.SettingsManager.UIGeneralSettings.DisplayButtonPrompts && !DisplayablePlayer.ButtonPrompts.IsSuspended)
        {
            instructional.Buttons.Clear();
            if (DisplayablePlayer.ButtonPrompts.Prompts.Any())
            {
                foreach (ButtonPrompt buttonPrompt in DisplayablePlayer.ButtonPrompts.Prompts.OrderByDescending(x => x.Order))
                {
                    if(buttonPrompt.HasGameControl && buttonPrompt.GameControl != GameControl.NextCamera)
                    {
                        InstructionalButton mybutt = new InstructionalButton(buttonPrompt.GameControl, buttonPrompt.Text);
                        instructional.Buttons.Add(mybutt);
                    }
                    else if (buttonPrompt.Key != Keys.None)
                    {
                        if (buttonPrompt.ModifierKey != Keys.None)
                        {
                            instructional.Buttons.Add(new InstructionalButtonGroup(buttonPrompt.Text, buttonPrompt.ModifierKey.GetInstructionalKey(), InstructionalKey.SymbolPlus, buttonPrompt.Key.GetInstructionalKey()));
                        }
                        else
                        {
                            instructional.Buttons.Add(new InstructionalButtonGroup(buttonPrompt.Text, buttonPrompt.Key.GetInstructionalKey()));
                        }
                    }
                    else if (buttonPrompt.ModifierKey != Keys.None)
                    {
                        instructional.Buttons.Add(new InstructionalButtonGroup(buttonPrompt.Text, buttonPrompt.ModifierKey.GetInstructionalKey()));
                    }
                }
            }
            instructional.Update();
            if (DisplayablePlayer.ButtonPrompts.Prompts.Any())
            {
                instructional.Draw();
            }
        }
    }   
    private void MenuUpdate()
    {
        TimerBarPool.Draw();
        MenuPool.ProcessMenus();
        PlayerInfoMenu.Update();
        SavePauseMenu.Update();
        MessagesMenu.Update();
        AboutMenu.Update();
    }
    private void RadarUpdate()
    {
        if (Settings.SettingsManager.UIGeneralSettings.SetRadarZoomDistance)
        {
            if (DisplayablePlayer.IsWanted)
            {
                NativeFunction.Natives.SET_RADAR_ZOOM_TO_DISTANCE(Settings.SettingsManager.UIGeneralSettings.RadarZoomDistance_Wanted);
            }
            else if (DisplayablePlayer.Investigation.IsActive)
            {
                NativeFunction.Natives.SET_RADAR_ZOOM_TO_DISTANCE(Settings.SettingsManager.UIGeneralSettings.RadarZoomDistance_Investigation);
            }
            else
            {
                NativeFunction.Natives.SET_RADAR_ZOOM_TO_DISTANCE(Settings.SettingsManager.UIGeneralSettings.RadarZoomDistance_Default);
            }
        }
    }
    private void ScreenEffectsUpdate()
    {
        if (DisplayablePlayer.IsDead)
        {
            if (!StartedDeathEffect)
            {
                GameTimeLastDied = Game.GameTime;

                if (Settings.SettingsManager.UIGeneralSettings.SetDeathEffect)
                {
                    NativeFunction.Natives.x80C8B1846639BB19(1);
                    NativeFunction.Natives.x2206BF9A37B7F724("DeathFailMPIn", 0, 0);
                    IsShowingCustomOverlay = true;
                }
                if (Settings.SettingsManager.UIGeneralSettings.PlayWastedSounds)
                {
                    NativeFunction.CallByName<bool>("PLAY_SOUND_FRONTEND", -1, "Bed", "WastedSounds", true);
                }
                if (Settings.SettingsManager.UIGeneralSettings.DisplayWastedMessage)
                {
                    BigMessage?.Fiber?.Abort();
                    BigMessage = new BigMessageThread(true);
                    BigMessage.MessageInstance.ShowColoredShard(Settings.SettingsManager.UIGeneralSettings.WastedMessageText, "", HudColor.Black, HudColor.RedDark, 2000);
                }
                StartedDeathEffect = true;
            }
            if (GameTimeLastDied != 0 && Game.GameTime - GameTimeLastDied >= (Settings.SettingsManager.PlayerOtherSettings.SetSlowMoOnDeath ? 1000 : 2000))
            {
                GameTimeLastDied = 0;
                Show(DeathMenu);
                if(Settings.SettingsManager.PlayerOtherSettings.SetCutToBlackDeath && Game.IsScreenFadedOut)
                {
                    Game.FadeScreenIn(2000, false);
                }
            }
        }
        else if (DisplayablePlayer.IsBusted)
        {
            if (!StartedBustedEffect)
            {
                GameTimeLastBusted = Game.GameTime;
                if (Settings.SettingsManager.UIGeneralSettings.SetBustedEffect)
                {
                    NativeFunction.Natives.x80C8B1846639BB19(1);
                    NativeFunction.Natives.x2206BF9A37B7F724("DeathFailMPDark", 0, 0);
                    IsShowingCustomOverlay = true;
                }
                if (Settings.SettingsManager.UIGeneralSettings.PlayWastedSounds)
                {
                    NativeFunction.CallByName<bool>("PLAY_SOUND_FRONTEND", -1, "TextHit", "WastedSounds", true);
                }
                if (Settings.SettingsManager.UIGeneralSettings.DisplayBustedMessage)
                {
                    BigMessage?.Fiber?.Abort();
                    BigMessage = new BigMessageThread(true);
                    BigMessage.MessageInstance.ShowColoredShard(DisplayablePlayer.IsNotWanted ? Settings.SettingsManager.UIGeneralSettings.DetainedMessageText : Settings.SettingsManager.UIGeneralSettings.BustedMessageText, "", HudColor.Black, HudColor.Blue, 2000);
                }
                StartedBustedEffect = true;
            }
            if (GameTimeLastBusted != 0 && Game.GameTime - GameTimeLastBusted >= (Settings.SettingsManager.PlayerOtherSettings.SetSlowMoOnBusted ? 1000 : 2000))
            {
                GameTimeLastBusted = 0;
                Show(BustedMenu);
            }
        }
        else
        {
            GameTimeLastDied = 0;
            GameTimeLastBusted = 0;

            //if (StartedBustedEffect || StartedDeathEffect)
            //{
            //    BigMessage.MessageInstance.Dispose();
            //}


            if(StartedBustedEffect || StartedDeathEffect)
            {
                BigMessage?.Fiber?.Abort();
                EntryPoint.WriteToConsole("REMOVE FIBER RAN!");
            }
            StartedBustedEffect = false;
            StartedDeathEffect = false;
            if (Settings.SettingsManager.UIGeneralSettings.AllowScreenEffectReset && IsShowingCustomOverlay)
            {
                NativeFunction.Natives.xB4EDDC19532BFB85();
                NativeFunction.Natives.x80C8B1846639BB19(0);
                IsShowingCustomOverlay = false;
            }
        }
    }
    private void ForceVanillaUI()
    {
        if (Settings.SettingsManager.UIGeneralSettings.AlwaysShowHUD)
        {
            NativeFunction.Natives.xB9EFD5C25018725A("DISPLAY_HUD", true);
        }
        if (DisplayablePlayer.ActivityManager.IsInteractingWithLocation)
        {
            ShowRadar = false;
        }
        else if (DisplayablePlayer.IsCustomizingPed)
        {
            ShowRadar = false;
        }
        else if (Settings.SettingsManager.UIGeneralSettings.AlwaysShowRadar)
        {
            ShowRadar = true;
        }
        else if (Settings.SettingsManager.UIGeneralSettings.NeverShowRadar)
        {
            ShowRadar = false;
        }
        else if (Settings.SettingsManager.UIGeneralSettings.HideRadarUnlessActionWheelActive)
        {
            if (IsDrawingWheelMenu)
            {
                ShowRadar = true;
            }
            else
            {
                ShowRadar = false;
            }
        }
        else if (DisplayablePlayer.IsInVehicle)
        {
            if (Settings.SettingsManager.UIGeneralSettings.ShowRadarInVehicleOnly)
            {
                if (DisplayablePlayer.IsInVehicle)
                {
                    ShowRadar = true;
                }
                else
                {
                    ShowRadar = false;
                }
            }
            else if (Settings.SettingsManager.UIGeneralSettings.ShowRadarWhenCellPhoneActiveOnly)
            {
                if (DisplayablePlayer.CellPhone.IsActive)
                {
                    ShowRadar = true;
                }
                else
                {
                    ShowRadar = false;
                }
            }
        }
        else if (!DisplayablePlayer.IsInVehicle)
        {
            if (Settings.SettingsManager.UIGeneralSettings.ShowRadarOnFootWhenCellPhoneActiveOnly)
            {
                if (DisplayablePlayer.CellPhone.IsActive)
                {
                    ShowRadar = true;
                }
                else
                {
                    ShowRadar = false;
                }
            }
            else if (Settings.SettingsManager.UIGeneralSettings.ShowRadarWhenCellPhoneActiveOnly)
            {
                if (DisplayablePlayer.CellPhone.IsActive)
                {
                    ShowRadar = true;
                }
                else
                {
                    ShowRadar = false;
                }
            }
            else if (Settings.SettingsManager.UIGeneralSettings.ShowRadarInVehicleOnly)
            {
                ShowRadar = false;
            }
        }
        NativeFunction.CallByName<bool>("DISPLAY_RADAR", ShowRadar);
        if (Settings.SettingsManager.PoliceSettings.AttachBlipsToAmbientPeds)
        {
            NativeFunction.CallByName<bool>("SET_POLICE_RADAR_BLIPS", true);
        }
        else
        {
            NativeFunction.CallByName<bool>("SET_POLICE_RADAR_BLIPS", false);
        }
        if (Settings.SettingsManager.UIGeneralSettings.DisableVanillaCashDisplay)
        {
            NativeFunction.CallByName<bool>("DISPLAY_CASH", false);
        }
        if (!Settings.SettingsManager.UIGeneralSettings.ShowVanillaAreaUI)
        {
            NativeFunction.CallByName<bool>("HIDE_HUD_COMPONENT_THIS_FRAME", (int)GTAHudComponent.HUD_AREA_NAME);
            NativeFunction.CallByName<bool>("HIDE_HUD_COMPONENT_THIS_FRAME", (int)GTAHudComponent.HUD_STREET_NAME);
            NativeFunction.CallByName<bool>("HIDE_HUD_COMPONENT_THIS_FRAME", (int)GTAHudComponent.HUD_VEHICLE_CLASS);
        }
        if (!Settings.SettingsManager.UIGeneralSettings.ShowVanillaVehicleUI)
        {
            NativeFunction.CallByName<bool>("HIDE_HUD_COMPONENT_THIS_FRAME", (int)GTAHudComponent.HUD_VEHICLE_NAME);
            NativeFunction.CallByName<bool>("HIDE_HUD_COMPONENT_THIS_FRAME", (int)GTAHudComponent.HUD_VEHICLE_CLASS);
        }


        NativeFunction.Natives.HIDE_HUD_COMPONENT_THIS_FRAME((int)GTAHudComponent.HUD_WANTED_STARS);
        NativeFunction.Natives.FLASH_WANTED_DISPLAY(DisplayablePlayer.IsWanted && DisplayablePlayer.IsInSearchMode);
    }
    private void Show(ModUIMenu toShow)
    {
        foreach (ModUIMenu menu in MenuList)
        {
            if (menu != toShow)
            {
                menu.Hide();
            }
        }
        toShow.Show();
    }
    private void Toggle(ModUIMenu toToggle)
    {
        foreach (ModUIMenu menu in MenuList)
        {
            if (menu != toToggle)
            {
                menu.Hide();
            }
        }
        toToggle.Toggle();
    }
    public void ToggleDebugMenu()
    {
        Toggle(DebugMenu);
    }
    public void ToggleMenu()
    {
        if (DisplayablePlayer.IsDead)
        {
            Toggle(DeathMenu);
        }
        else if (DisplayablePlayer.IsBusted)
        {
            Toggle(BustedMenu);
        }
        else
        {
            Toggle(MainMenu);
        }
    }
    public void ToggleCraftingMenu()
    {
        Toggle(CraftingMenu);
    }
    public void ToggleAltMenu()
    {
        if (DisplayablePlayer.IsDead)
        {
            Toggle(DeathMenu);
        }
        else if (DisplayablePlayer.IsBusted)
        {
            Toggle(BustedMenu);
        }
    }
}
