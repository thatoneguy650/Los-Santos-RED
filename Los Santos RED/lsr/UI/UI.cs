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
    private AboutMenu AboutMenu;
    private BarDisplay BarDisplay;
    private BigMessageThread BigMessage;
    private BustedMenu BustedMenu;
    private string CurrentDefaultTextColor = "~c~";
    private DeathMenu DeathMenu;
    private DebugMenu DebugMenu;
    private IDisplayable DisplayablePlayer;
    private List<ButtonPrompt> displayedButtonPrompts = new List<ButtonPrompt>();
    private bool DrawSpeedLimitTexture = true;
    private uint GameTimeLastBusted;
    private uint GameTimeLastDied;
    private uint GameTimeLastDrawnUI;
    private InstructionalButtons instructional = new InstructionalButtons();
    private bool IsShowingCustomOverlay = false;
    private IJurisdictions Jurisdictions;
    private string lastCrimesDisplay;
    private string lastPlayerDisplay;
    private string lastStreetDisplay;
    private string lastVehicleStatusDisplay;
    private string lastWeaponDisplay;
    private string lastZoneDisplay;
    private MainMenu MainMenu;
    private List<Menu> MenuList;
    private MenuPool menuPool;
    private string overrideTimeDisplay = "";
    private Fader PlayerFader;
    private bool playerIsInVehicle = false;
    private PlayerInfoMenu PlayerInfoMenu;
    private ISettingsProvideable Settings;
    private Texture Sign10;
    private Texture Sign15;
    private Texture Sign20;
    private Texture Sign25;
    private Texture Sign30;
    private Texture Sign35;
    private Texture Sign40;
    private Texture Sign45;
    private Texture Sign50;
    private Texture Sign55;
    private Texture Sign60;
    private Texture Sign65;
    private Texture Sign70;
    private Texture Sign75;
    private Texture Sign80;
    private Texture SpeedLimitToDraw;
    private bool StartedBustedEffect = false;
    private bool StartedDeathEffect = false;
    private Fader StreetFader;
    private ITaskerable Tasker;
    private ITimeControllable Time;
    private Fader VehicleFader;
    private Fader WeaponFader;
    private IEntityProvideable World;
    private Fader ZoneFader;
    private MessagesMenu MessagesMenu;
    private string debugString1;
    private bool ShowRadar;


    private uint SpriteUint;
    private uint SpriteUintCounter;
    private uint PreviousGameTime;
    private bool TimeOutSprite;
    private bool IsDisposed = false;
    private uint GameTimeStartedLowHealthUI;
    private PopUpMenu ActionPopUpMenu;
    private bool IsDrawingWheelMenu;
    private IActionable ActionablePlayer;

    private bool ShouldShowSpeedLimitSign => DisplayablePlayer.CurrentVehicle != null && DisplayablePlayer.CurrentLocation.CurrentStreet != null && DisplayablePlayer.IsAliveAndFree;

    public UI(IDisplayable displayablePlayer, ISettingsProvideable settings, IJurisdictions jurisdictions, IPedSwap pedSwap, IPlacesOfInterest placesOfInterest, IRespawning respawning, IActionable actionablePlayer, ISaveable saveablePlayer, IWeapons weapons, RadioStations radioStations, IGameSaves gameSaves, IEntityProvideable world, IRespawnable player, IPoliceRespondable policeRespondable, ITaskerable tasker, IInventoryable playerinventory, IModItems modItems, ITimeControllable time, IGangRelateable gangRelateable, IGangs gangs, IGangTerritories gangTerritories, IZones zones, IStreets streets, IInteriors interiors, Dispatcher dispatcher, IAgencies agencies, ILocationInteractable locationInteractableplayer, IDances dances, IGestures gestures)
    {
        DisplayablePlayer = displayablePlayer;
        ActionablePlayer = actionablePlayer;
        Settings = settings;
        Jurisdictions = jurisdictions;
        Time = time;
        Tasker = tasker;
        World = world;
        BigMessage = new BigMessageThread(true);
        menuPool = new MenuPool();
        DeathMenu = new DeathMenu(menuPool, pedSwap, respawning, placesOfInterest, Settings, player, gameSaves);
        BustedMenu = new BustedMenu(menuPool, pedSwap, respawning, placesOfInterest, Settings, policeRespondable, time);
        MainMenu = new MainMenu(menuPool, locationInteractableplayer, saveablePlayer, gameSaves, weapons, pedSwap, world, Settings, Tasker, playerinventory, modItems, this, gangs, time,placesOfInterest, dances, gestures);
        DebugMenu = new DebugMenu(menuPool, actionablePlayer, weapons, radioStations, placesOfInterest, Settings, Time, World, Tasker, dispatcher,agencies, gangs, modItems);
        MenuList = new List<Menu>() { DeathMenu, BustedMenu, MainMenu, DebugMenu };
        StreetFader = new Fader(Settings.SettingsManager.LSRHUDSettings.StreetDisplayTimeToShow, Settings.SettingsManager.LSRHUDSettings.StreetDisplayTimeToFade, "StreetFader");
        ZoneFader = new Fader(Settings.SettingsManager.LSRHUDSettings.ZoneDisplayTimeToShow, Settings.SettingsManager.LSRHUDSettings.ZoneDisplayTimeToFade, "ZoneFader");
        VehicleFader = new Fader(Settings.SettingsManager.LSRHUDSettings.VehicleDisplayTimeToShow, Settings.SettingsManager.LSRHUDSettings.VehicleDisplayTimeToFade, "VehicleFader");
        PlayerFader = new Fader(Settings.SettingsManager.LSRHUDSettings.PlayerDisplayTimeToShow, Settings.SettingsManager.LSRHUDSettings.PlayerDisplayTimeToFade, "PlayerFader");
        WeaponFader = new Fader(Settings.SettingsManager.LSRHUDSettings.WeaponDisplayTimeToShow, Settings.SettingsManager.LSRHUDSettings.WeaponDisplayTimeToFade, "WeaponFader");
        PlayerInfoMenu = new PlayerInfoMenu(gangRelateable, Time, placesOfInterest, gangs, gangTerritories, zones, streets, interiors, World);
        MessagesMenu = new MessagesMenu(gangRelateable, Time, placesOfInterest, gangs, gangTerritories, zones, streets, interiors, World);
        AboutMenu = new AboutMenu(gangRelateable, Time, Settings);
        BarDisplay = new BarDisplay(DisplayablePlayer, Settings);
        ActionPopUpMenu = new PopUpMenu(actionablePlayer, Settings, this, gestures, dances);
        
    }
    private enum GTAHudComponent
    {
        HUD = 0,
        HUD_WANTED_STARS = 1,
        HUD_WEAPON_ICON = 2,
        HUD_CASH = 3,
        HUD_MP_CASH = 4,
        HUD_MP_MESSAGE = 5,
        HUD_VEHICLE_NAME = 6,
        HUD_AREA_NAME = 7,
        HUD_VEHICLE_CLASS = 8,
        HUD_STREET_NAME = 9,
        HUD_HELP_TEXT = 10,
        HUD_FLOATING_HELP_TEXT_1 = 11,
        HUD_FLOATING_HELP_TEXT_2 = 12,
        HUD_CASH_CHANGE = 13,
        HUD_RETICLE = 14,
        HUD_SUBTITLE_TEXT = 15,
        HUD_RADIO_STATIONS = 16,
        HUD_SAVING_GAME = 17,
        HUD_GAME_STREAM = 18,
        HUD_WEAPON_WHEEL = 19,
        HUD_WEAPON_WHEEL_STATS = 20,
        MAX_HUD_COMPONENTS = 21,
        MAX_HUD_WEAPONS = 22,
        MAX_SCRIPTED_HUD_COMPONENTS = 141,
    }
    private enum GTATextJustification
    {
        Center = 0,
        Left = 1,
        Right = 2,
    };
    public void Dispose()
    {
        IsDisposed = true;
        ActionPopUpMenu.Dispose();
        NativeFunction.CallByName<bool>("DISPLAY_RADAR", true);
        NativeFunction.Natives.xB9EFD5C25018725A("DISPLAY_HUD", true);
    }
    public void DrawSprites(object sender, GraphicsEventArgs args)
    {
        try
        {

            if (!TimeOutSprite && DrawSpeedLimitTexture && Game.Resolution != null && !Game.IsPaused && DisplayablePlayer.IsAliveAndFree && !menuPool.IsAnyMenuOpen() && !TabView.IsAnyPauseMenuVisible && !Game.IsScreenFadingOut && !Game.IsScreenFadedOut)
            {
                if (SpeedLimitToDraw != null && SpeedLimitToDraw.Size != null)
                {
                    float ConsistencyScale = (float)Game.Resolution.Width / 2160f;
                    float Scale = Settings.SettingsManager.LSRHUDSettings.SpeedLimitScale * ConsistencyScale;
                    float posX = (Game.Resolution.Height - (SpeedLimitToDraw.Size.Height * Scale)) * Settings.SettingsManager.LSRHUDSettings.SpeedLimitPositionX;
                    float posY = (Game.Resolution.Width - (SpeedLimitToDraw.Size.Width * Scale)) * Settings.SettingsManager.LSRHUDSettings.SpeedLimitPositionY;
                    args.Graphics.DrawTexture(SpeedLimitToDraw, new RectangleF(posY, posX, SpeedLimitToDraw.Size.Width * Scale, SpeedLimitToDraw.Size.Height * Scale));
                }
            }
        }
        catch (Exception ex)
        {
            EntryPoint.WriteToConsole($"UI: Draw ERROR {ex.Message} {ex.StackTrace} ", 0);
        }
    }
    public void DrawText()
    {
        GameTimeLastDrawnUI = Game.GameTime;
        if (Settings.SettingsManager.UIGeneralSettings.IsEnabled && DisplayablePlayer.IsAliveAndFree)
        {
            if(!Settings.SettingsManager.UIGeneralSettings.HideLSRUIUnlessActionWheelActive || IsDrawingWheelMenu)
            {
                DisplayCurrentCrimes();
                DisplayVehicleStatus();
                DisplayWeaponStatus();
                DisplayPlayerInfo();
                DisplayStreets();
                DisplayZones();
                DisplayCash();
            }
            if (IsDrawingWheelMenu)
            {
                ActionPopUpMenu.Draw();
            }
        }
    }
    private void DisplayCurrentCrimes()
    {
        if (Settings.SettingsManager.LSRHUDSettings.CrimesDisplayEnabled)
        {
            DisplayTextOnScreen(lastCrimesDisplay, Settings.SettingsManager.LSRHUDSettings.CrimesDisplayPositionX, Settings.SettingsManager.LSRHUDSettings.CrimesDisplayPositionY, Settings.SettingsManager.LSRHUDSettings.CrimesDisplayScale, Color.White, Settings.SettingsManager.LSRHUDSettings.CrimesDisplayFont, (GTATextJustification)Settings.SettingsManager.LSRHUDSettings.CrimesDisplayJustificationID);
        }
    }
    private void DisplayVehicleStatus()
    {
        if (Settings.SettingsManager.LSRHUDSettings.VehicleDisplayEnabled)
        {
            DisplayTextOnScreen(lastVehicleStatusDisplay, Settings.SettingsManager.LSRHUDSettings.VehicleDisplayPositionX, Settings.SettingsManager.LSRHUDSettings.VehicleDisplayPositionY, Settings.SettingsManager.LSRHUDSettings.VehicleDisplayScale, Color.White, Settings.SettingsManager.LSRHUDSettings.VehicleDisplayFont, (GTATextJustification)Settings.SettingsManager.LSRHUDSettings.VehicleDisplayJustificationID);
        }
    }
    private void DisplayWeaponStatus()
    {
        if (Settings.SettingsManager.LSRHUDSettings.ShowWeaponDisplay)
        {
            WeaponFader.Update(lastWeaponDisplay);
            if (Settings.SettingsManager.LSRHUDSettings.FadeWeaponDisplay && ((!DisplayablePlayer.IsWanted && !DisplayablePlayer.Investigation.IsActive) || Settings.SettingsManager.LSRHUDSettings.FadeWeaponDisplayDuringWantedAndInvestigation) && !IsDrawingWheelMenu)
            {
                DisplayTextOnScreen(WeaponFader.TextToShow, Settings.SettingsManager.LSRHUDSettings.WeaponDisplayPositionX, Settings.SettingsManager.LSRHUDSettings.WeaponDisplayPositionY, Settings.SettingsManager.LSRHUDSettings.WeaponDisplayScale, Color.White, Settings.SettingsManager.LSRHUDSettings.WeaponDisplayFont, (GTATextJustification)Settings.SettingsManager.LSRHUDSettings.WeaponDisplayJustificationID, WeaponFader.AlphaValue);
            }
            else
            {
                DisplayTextOnScreen(lastWeaponDisplay, Settings.SettingsManager.LSRHUDSettings.WeaponDisplayPositionX, Settings.SettingsManager.LSRHUDSettings.WeaponDisplayPositionY, Settings.SettingsManager.LSRHUDSettings.WeaponDisplayScale, Color.White, Settings.SettingsManager.LSRHUDSettings.WeaponDisplayFont, (GTATextJustification)Settings.SettingsManager.LSRHUDSettings.WeaponDisplayJustificationID);
            }
        }
    }
    private void DisplayPlayerInfo()
    {
        if (Settings.SettingsManager.LSRHUDSettings.ShowPlayerDisplay)
        {
            PlayerFader.Update(lastPlayerDisplay);
            if (Settings.SettingsManager.LSRHUDSettings.FadePlayerDisplay && ((!DisplayablePlayer.IsWanted && !DisplayablePlayer.Investigation.IsActive) || Settings.SettingsManager.LSRHUDSettings.FadePlayerDisplayDuringWantedAndInvestigation) && !IsDrawingWheelMenu)
            {
                DisplayTextOnScreen(PlayerFader.TextToShow, Settings.SettingsManager.LSRHUDSettings.PlayerStatusPositionX, Settings.SettingsManager.LSRHUDSettings.PlayerStatusPositionY, Settings.SettingsManager.LSRHUDSettings.PlayerStatusScale, Color.White, Settings.SettingsManager.LSRHUDSettings.PlayerStatusFont, (GTATextJustification)Settings.SettingsManager.LSRHUDSettings.PlayerStatusJustificationID, PlayerFader.AlphaValue);
            }
            else
            {
                DisplayTextOnScreen(lastPlayerDisplay, Settings.SettingsManager.LSRHUDSettings.PlayerStatusPositionX, Settings.SettingsManager.LSRHUDSettings.PlayerStatusPositionY, Settings.SettingsManager.LSRHUDSettings.PlayerStatusScale, Color.White, Settings.SettingsManager.LSRHUDSettings.PlayerStatusFont, (GTATextJustification)Settings.SettingsManager.LSRHUDSettings.PlayerStatusJustificationID);
            }
        }
    }
    private void DisplayStreets()
    {
        if (Settings.SettingsManager.LSRHUDSettings.ShowStreetDisplay)
        {
            StreetFader.Update(lastStreetDisplay);
            if (Settings.SettingsManager.LSRHUDSettings.FadeStreetDisplay && ((!DisplayablePlayer.IsWanted && !DisplayablePlayer.Investigation.IsActive) || Settings.SettingsManager.LSRHUDSettings.FadeStreetDisplayDuringWantedAndInvestigation) && !IsDrawingWheelMenu)
            {
                DisplayTextOnScreen(StreetFader.TextToShow, Settings.SettingsManager.LSRHUDSettings.StreetPositionX, Settings.SettingsManager.LSRHUDSettings.StreetPositionY, Settings.SettingsManager.LSRHUDSettings.StreetScale, Color.White, Settings.SettingsManager.LSRHUDSettings.StreetFont, (GTATextJustification)Settings.SettingsManager.LSRHUDSettings.StreetJustificationID, StreetFader.AlphaValue);
            }
            else
            {
                DisplayTextOnScreen(lastStreetDisplay, Settings.SettingsManager.LSRHUDSettings.StreetPositionX, Settings.SettingsManager.LSRHUDSettings.StreetPositionY, Settings.SettingsManager.LSRHUDSettings.StreetScale, Color.White, Settings.SettingsManager.LSRHUDSettings.StreetFont, (GTATextJustification)Settings.SettingsManager.LSRHUDSettings.StreetJustificationID);
            }
        }
    }
    private void DisplayZones()
    {
        if (Settings.SettingsManager.LSRHUDSettings.ShowZoneDisplay)
        {
            ZoneFader.Update(lastZoneDisplay);
            if (Settings.SettingsManager.LSRHUDSettings.FadeZoneDisplay && ((!DisplayablePlayer.IsWanted && !DisplayablePlayer.Investigation.IsActive) || Settings.SettingsManager.LSRHUDSettings.FadeZoneDisplayDuringWantedAndInvestigation) && !IsDrawingWheelMenu)
            {
                DisplayTextOnScreen(ZoneFader.TextToShow, Settings.SettingsManager.LSRHUDSettings.ZonePositionX, Settings.SettingsManager.LSRHUDSettings.ZonePositionY, Settings.SettingsManager.LSRHUDSettings.ZoneScale, Color.White, Settings.SettingsManager.LSRHUDSettings.ZoneFont, (GTATextJustification)Settings.SettingsManager.LSRHUDSettings.ZoneJustificationID, ZoneFader.AlphaValue);
            }
            else
            {
                DisplayTextOnScreen(lastZoneDisplay, Settings.SettingsManager.LSRHUDSettings.ZonePositionX, Settings.SettingsManager.LSRHUDSettings.ZonePositionY, Settings.SettingsManager.LSRHUDSettings.ZoneScale, Color.White, Settings.SettingsManager.LSRHUDSettings.ZoneFont, (GTATextJustification)Settings.SettingsManager.LSRHUDSettings.ZoneJustificationID);
            }
        }
    }
    private void DisplayCash()
    {
        if(!Settings.SettingsManager.PedSwapSettings.AliasPedAsMainCharacter && !DisplayablePlayer.CharacterModelIsPrimaryCharacter && (DisplayablePlayer.IsTransacting || DisplayablePlayer.RecentlyChangedMoney || DisplayablePlayer.IsBusted || IsDrawingWheelMenu))
        {
            DisplayTextOnScreen(DisplayablePlayer.Money.ToString("C0"), Settings.SettingsManager.LSRHUDSettings.AltCashPositionX, Settings.SettingsManager.LSRHUDSettings.AltCashPositionY, Settings.SettingsManager.LSRHUDSettings.AltCashScale, Color.White, GTAFont.FontPricedown, (GTATextJustification)2);
        }
    }
    public void PrimaryTick()
    {
        if (!menuPool.IsAnyMenuOpen() && !TabView.IsAnyPauseMenuVisible)
        {
            DrawText();
            if (ShowRadar || DisplayablePlayer.Sprinting.StaminaPercentage < 1.0f || DisplayablePlayer.IntoxicatedIntensityPercent > 0.0f)
            {
                BarDisplay.Draw(DisplayablePlayer.Sprinting.StaminaPercentage, DisplayablePlayer.IntoxicatedIntensityPercent, 0.0f);
            }
            DrawDebug();
        }
        MenuUpdate();
    }
    public void SecondaryTick()
    {
        DisplayButtonPrompts();
        ForceVanillaUI();
    }
    public void Setup()
    {
        IsDisposed = false;
        PlayerInfoMenu.Setup();
        MessagesMenu.Setup();
        AboutMenu.Setup();
        ActionPopUpMenu.Setup();
    }
    public void TertiaryTick()
    {
        GameFiber.Yield();
        ScreenEffectsUpdate();
        GameFiber.Yield();
        RadarUpdate();
        GameFiber.Yield();
    }
    public void ToggleAboutMenu()
    {
        AboutMenu.Toggle();
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
    public void TogglePlayerInfoMenu()
    {
        PlayerInfoMenu.Toggle();
    }
    public void ToggleMessagesMenu()
    {
        MessagesMenu.Toggle();
    }
    public void Update()
    {
        if (DisplayablePlayer.IsAliveAndFree)
        {
            if (Time.IsNight && Settings.SettingsManager.UIGeneralSettings.GreyOutWhiteFontAtNight)
            {
                CurrentDefaultTextColor = "~c~";
            }
            else
            {
                CurrentDefaultTextColor = "~s~";
            }
            if (Settings.SettingsManager.LSRHUDSettings.CrimesDisplayEnabled)
            {
                lastCrimesDisplay = GetViolationsText();
            }
            if (Settings.SettingsManager.LSRHUDSettings.VehicleDisplayEnabled)
            {
                lastVehicleStatusDisplay = GetVehicleStatusDisplay();
            }
            if (Settings.SettingsManager.LSRHUDSettings.ShowPlayerDisplay)
            {
                lastPlayerDisplay = GetPlayerDisplay();
            }
            if (Settings.SettingsManager.LSRHUDSettings.ShowWeaponDisplay)
            {
                lastWeaponDisplay = GetWeaponDisplay();
            }
            if (playerIsInVehicle != DisplayablePlayer.IsInVehicle)
            {
                if (Settings.SettingsManager.LSRHUDSettings.FadeZoneDisplay)
                {
                    ZoneFader.UpdateTimeChanged();
                }
                if (Settings.SettingsManager.LSRHUDSettings.FadeStreetDisplay)
                {
                    StreetFader.UpdateTimeChanged();
                }
                playerIsInVehicle = DisplayablePlayer.IsInVehicle;
            }
            if (Settings.SettingsManager.LSRHUDSettings.ShowStreetDisplay)
            {
                lastStreetDisplay = GetStreetDisplay();
                if (Settings.SettingsManager.LSRHUDSettings.FadeStreetDisplay && ((!DisplayablePlayer.IsWanted && !DisplayablePlayer.Investigation.IsActive) || Settings.SettingsManager.LSRHUDSettings.FadeStreetDisplayDuringWantedAndInvestigation))
                {
                    if (Settings.SettingsManager.LSRHUDSettings.FadeZoneDisplay && ZoneFader.TextChangedLastUpdate)
                    {
                        StreetFader.UpdateTimeChanged();
                    }
                }
            }
            if (Settings.SettingsManager.LSRHUDSettings.ShowZoneDisplay)
            {
                lastZoneDisplay = GetZoneDisplay();
                if (Settings.SettingsManager.LSRHUDSettings.FadeZoneDisplay && ((!DisplayablePlayer.IsWanted && !DisplayablePlayer.Investigation.IsActive) || Settings.SettingsManager.LSRHUDSettings.FadeZoneDisplayDuringWantedAndInvestigation))
                {
                    if (Settings.SettingsManager.LSRHUDSettings.FadeStreetDisplay && StreetFader.TextChangedLastUpdate)
                    {
                        ZoneFader.UpdateTimeChanged();
                    }
                }
            }
            debugString1 = $"Heading: {Math.Round(DisplayablePlayer.Character.Heading,1)}";
            DisplayablePlayer.DebugLine4 = debugString1;
        }
    }
    private void DisplayButtonPrompts()
    {
        if (Settings.SettingsManager.UIGeneralSettings.DisplayButtonPrompts)
        {
            instructional.Buttons.Clear();

            if (DisplayablePlayer.ButtonPromptList.Any())
            {
                foreach (ButtonPrompt buttonPrompt in DisplayablePlayer.ButtonPromptList.OrderByDescending(x => x.Order))
                {
                    if (buttonPrompt.Key != Keys.None)
                    {
                        if (buttonPrompt.Modifier != Keys.None)
                        {
                            instructional.Buttons.Add(new InstructionalButtonGroup(buttonPrompt.Text, buttonPrompt.Modifier.GetInstructionalKey(), InstructionalKey.SymbolPlus, buttonPrompt.Key.GetInstructionalKey()));
                        }
                        else
                        {
                            instructional.Buttons.Add(new InstructionalButtonGroup(buttonPrompt.Text, buttonPrompt.Key.GetInstructionalKey()));
                        }
                    }
                    else if (buttonPrompt.Modifier != Keys.None)
                    {
                        instructional.Buttons.Add(new InstructionalButtonGroup(buttonPrompt.Text, buttonPrompt.Modifier.GetInstructionalKey()));
                    }
                    //else
                    //{
                    //    if (buttonPrompt.Modifier != Keys.None)
                    //    {
                    //        instructional.Buttons.Add(new InstructionalButtonGroup(buttonPrompt.Text, buttonPrompt.Modifier.GetInstructionalKey(), InstructionalKey.SymbolPlus, InstructionalKey.MouseLeft));
                    //    }
                    //    else
                    //    {
                    //        instructional.Buttons.Add(new InstructionalButtonGroup(buttonPrompt.Text, InstructionalKey.MouseLeft));
                    //    }
                    //}
                }
            }
            instructional.Update();
            if (DisplayablePlayer.ButtonPromptList.Any())
            {
                instructional.Draw();
            }
        }
    }
    private void DisplayTextOnScreen(string TextToShow, float X, float Y, float Scale, Color TextColor, GTAFont Font, GTATextJustification Justification, int alpha)
    {
        try
        {
            if (TextToShow == "" || alpha == 0 || TextToShow is null)
            {
                return;
            }
            NativeFunction.Natives.SET_TEXT_FONT((int)Font);
            NativeFunction.Natives.SET_TEXT_SCALE(Scale, Scale);
            NativeFunction.Natives.SET_TEXT_COLOUR((int)TextColor.R, (int)TextColor.G, (int)TextColor.B, alpha);

            NativeFunction.Natives.SetTextJustification((int)Justification);
            NativeFunction.Natives.SetTextDropshadow(10, 255, 0, 255, 255);//NativeFunction.Natives.SetTextDropshadow(2, 2, 0, 0, 0);

            if (Justification == GTATextJustification.Right)
            {
                NativeFunction.Natives.SET_TEXT_WRAP(0f, Y);
            }
            else
            {
                NativeFunction.Natives.SET_TEXT_WRAP(0f, 1f);
            }
            NativeFunction.Natives.x25fbb336df1804cb("STRING"); //NativeFunction.Natives.x25fbb336df1804cb("STRING");
            NativeFunction.Natives.x25FBB336DF1804CB(TextToShow);
            NativeFunction.Natives.x6C188BE134E074AA(TextToShow);
            NativeFunction.Natives.xCD015E5BB0D96A57(Y, X);
        }
        catch (Exception ex)
        {
            EntryPoint.WriteToConsole($"UI ERROR {ex.Message} {ex.StackTrace}", 0);
        }
        //return;
    }
    private void DisplayTextOnScreen(string TextToShow, float X, float Y, float Scale, Color TextColor, GTAFont Font, GTATextJustification Justification)
    {
        DisplayTextOnScreen(TextToShow, X, Y, Scale, TextColor, Font, Justification, 255);
    }
    private void DrawDebug()
    {
        if (Settings.SettingsManager.UIGeneralSettings.ShowDebug)
        {
            //float StartingPoint = 0.1f;
            float StartingPoint = 0.5f;
            // DisplayTextOnScreen($"{DisplayablePlayer.DebugLine1}", StartingPoint + 0.01f, 0f, 0.2f, Color.White, GTAFont.FontChaletComprimeCologne, GTATextJustification.Left);
            //DisplayTextOnScreen($"{DisplayablePlayer.DebugLine2}", StartingPoint + 0.02f, 0f, 0.2f, Color.White, GTAFont.FontChaletComprimeCologne, GTATextJustification.Left);
            //DisplayTextOnScreen($"{DisplayablePlayer.DebugLine3}", StartingPoint + 0.03f, 0f, 0.2f, Color.White, GTAFont.FontChaletComprimeCologne, GTATextJustification.Left);
            DisplayTextOnScreen($"{DisplayablePlayer.DebugLine4}", StartingPoint + 0.04f, 0f, 0.2f, Color.White, GTAFont.FontChaletComprimeCologne, GTATextJustification.Left);
            DisplayTextOnScreen($"{DisplayablePlayer.DebugLine5}", StartingPoint + 0.05f, 0f, 0.2f, Color.White, GTAFont.FontChaletComprimeCologne, GTATextJustification.Left);
            DisplayTextOnScreen($"{DisplayablePlayer.DebugLine6}", StartingPoint + 0.06f, 0f, 0.2f, Color.White, GTAFont.FontChaletComprimeCologne, GTATextJustification.Left);
            //DisplayTextOnScreen($"{DisplayablePlayer.DebugLine7}", StartingPoint + 0.07f, 0f, 0.2f, Color.White, GTAFont.FontChaletComprimeCologne, GTATextJustification.Left);
            //DisplayTextOnScreen($"{DisplayablePlayer.DebugLine8}", StartingPoint + 0.08f, 0f, 0.2f, Color.White, GTAFont.FontChaletComprimeCologne, GTATextJustification.Left);
            //DisplayTextOnScreen($"{DisplayablePlayer.DebugLine9}", StartingPoint + 0.09f, 0f, 0.2f, Color.White, GTAFont.FontChaletComprimeCologne, GTATextJustification.Left);
            //DisplayTextOnScreen(World.DebugString, StartingPoint + 0.10f, 0f, 0.2f, Color.White, GTAFont.FontChaletComprimeCologne, GTATextJustification.Left);
            //DisplayTextOnScreen(Tasker.TaskerDebug, StartingPoint + 0.12f, 0f, 0.2f, Color.White, GTAFont.FontChaletComprimeCologne, GTATextJustification.Left);
        }
    }
    private void ForceVanillaUI()
    {
        if (Settings.SettingsManager.UIGeneralSettings.AlwaysShowHUD)
        {
            NativeFunction.Natives.xB9EFD5C25018725A("DISPLAY_HUD", true);
        }
        if (Settings.SettingsManager.UIGeneralSettings.AlwaysShowRadar)
        {
            ShowRadar = true;
        }
        else if (Settings.SettingsManager.UIGeneralSettings.NeverShowRadar)
        {
            ShowRadar = false;
        }
        else if (Settings.SettingsManager.UIGeneralSettings.HideRadarUnlessActionWheelActive)
        {
            if(IsDrawingWheelMenu)
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
            else if (Settings.SettingsManager.UIGeneralSettings.ShowRadarInVehicleOnly)
            {
                ShowRadar = false;
            }
        }

        NativeFunction.CallByName<bool>("DISPLAY_RADAR", ShowRadar);


        if (Settings.SettingsManager.PoliceSettings.ShowVanillaBlips)
        {
            NativeFunction.CallByName<bool>("SET_POLICE_RADAR_BLIPS", true);
        }
        else
        {
            NativeFunction.CallByName<bool>("SET_POLICE_RADAR_BLIPS", false);
        }
        if (Settings.SettingsManager.UIGeneralSettings.AlwaysShowCash)
        {
            NativeFunction.CallByName<bool>("DISPLAY_CASH", true);
        }
        else if ((Settings.SettingsManager.PedSwapSettings.AliasPedAsMainCharacter || DisplayablePlayer.CharacterModelIsPrimaryCharacter) && (DisplayablePlayer.IsTransacting || DisplayablePlayer.RecentlyChangedMoney || DisplayablePlayer.IsBusted || IsDrawingWheelMenu))
        {
            NativeFunction.CallByName<bool>("DISPLAY_CASH", true);
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
    }
    private string GetFormattedTime()
    {
        if (overrideTimeDisplay != "")
        {
            return overrideTimeDisplay;
        }
        string TimeDisplay = "";
        if (Time.CurrentDay != 0)
        {
            TimeDisplay = Time.CurrentTime;
        }
        return TimeDisplay;
    }
    private string GetPlayerDisplay()
    {
        string PlayerDisplay = "";
        if (DisplayablePlayer.IsWanted)
        {
            if (DisplayablePlayer.IsInSearchMode)
            {
                PlayerDisplay += $"~o~ Attempting To Locate";
            }
            else
            {
                if (DisplayablePlayer.PoliceResponse.IsWeaponsFree)
                {
                    PlayerDisplay += $"~r~ Weapons Free";
                }
                else if (DisplayablePlayer.PoliceResponse.IsDeadlyChase)
                {
                    PlayerDisplay += $"~r~ Lethal Force Authorized";
                }
                else
                {
                    PlayerDisplay += $"~r~ Active Pursuit";
                }
            }
            PlayerDisplay += $" ({DisplayablePlayer.PoliceResponse.CurrentRespondingPoliceCount}) {CurrentDefaultTextColor}";
        }
        else if (DisplayablePlayer.Investigation != null && DisplayablePlayer.Investigation.IsActive)
        {
            if (DisplayablePlayer.Investigation.RequiresPolice)
            {
                if (DisplayablePlayer.Investigation.IsSuspicious)
                {
                    PlayerDisplay += $"~r~ Police Responding with Description";
                    PlayerDisplay += $" ({DisplayablePlayer.Investigation.CurrentRespondingPoliceCount}){CurrentDefaultTextColor}";
                }
                else if (DisplayablePlayer.Investigation.IsNearPosition)
                {
                    PlayerDisplay += $"~o~ Police Responding";
                    PlayerDisplay += $" ({DisplayablePlayer.Investigation.CurrentRespondingPoliceCount}){CurrentDefaultTextColor}";
                }
                
            }
            else if (DisplayablePlayer.Investigation.RequiresEMS || DisplayablePlayer.Investigation.RequiresFirefighters)
            {
                PlayerDisplay += $"~o~ Emergency Services Responding{CurrentDefaultTextColor}";
            }
        }
        else if (DisplayablePlayer.HasCriminalHistory)
        {
            if (DisplayablePlayer.HasDeadlyCriminalHistory)
            {
                PlayerDisplay += $"~r~ APB Issued{CurrentDefaultTextColor}";
            }
            else
            {
                PlayerDisplay += $"~o~ BOLO Issued{CurrentDefaultTextColor}";
            }
        }
        if (DisplayablePlayer.IsNotWanted && Settings.SettingsManager.LSRHUDSettings.PlayerStatusIncludeTime)
        {
            if (PlayerDisplay == "")
            {
                PlayerDisplay = $"{CurrentDefaultTextColor}" + GetFormattedTime();
            }
            else
            {
                PlayerDisplay += $" {CurrentDefaultTextColor}" + GetFormattedTime();
            }
        }
        return PlayerDisplay;
    }
    private string GetSelectorText()
    {
        if (DisplayablePlayer.CurrentWeapon != null && DisplayablePlayer.CurrentWeapon.Category != WeaponCategory.Melee && DisplayablePlayer.CurrentWeapon.Category != WeaponCategory.Throwable)
        {
            if (Settings.SettingsManager.LSRHUDSettings.WeaponDisplaySimpleSelector)
            {
                if (DisplayablePlayer.CurrentSelectorSetting == SelectorOptions.Safe)
                {
                    return "~w~S~s~";
                }
                else if (DisplayablePlayer.CurrentSelectorSetting == SelectorOptions.SemiAuto)
                {
                    return "~r~1~s~";
                }
                else if (DisplayablePlayer.CurrentSelectorSetting == SelectorOptions.TwoRoundBurst)
                {
                    return "~r~2~s~";
                }
                else if (DisplayablePlayer.CurrentSelectorSetting == SelectorOptions.ThreeRoundBurst)
                {
                    return "~r~3~s~";
                }
                else if (DisplayablePlayer.CurrentSelectorSetting == SelectorOptions.FourRoundBurst)
                {
                    return "~r~4~s~";
                }
                else if (DisplayablePlayer.CurrentSelectorSetting == SelectorOptions.FiveRoundBurst)
                {
                    return "~r~5~s~";
                }
                else if (DisplayablePlayer.CurrentSelectorSetting == SelectorOptions.FullAuto)
                {
                    if (DisplayablePlayer.CurrentWeaponMagazineSize == 0)
                    {
                        return $"~r~FULL AUTO~s~";
                    }
                    else
                    {
                        return $"~r~{DisplayablePlayer.CurrentWeaponMagazineSize}~s~";
                    }
                }
                else
                {
                    return "";
                }
            }
            else
            {
                if (DisplayablePlayer.CurrentSelectorSetting == SelectorOptions.Safe)
                {
                    return "~s~SAFE~s~";
                }
                else if (DisplayablePlayer.CurrentSelectorSetting == SelectorOptions.SemiAuto)
                {
                    return "~r~Semi-Auto~s~";
                }
                else if (DisplayablePlayer.CurrentSelectorSetting == SelectorOptions.TwoRoundBurst)
                {
                    return "~r~2 Round Burst~s~";
                }
                else if (DisplayablePlayer.CurrentSelectorSetting == SelectorOptions.ThreeRoundBurst)
                {
                    return "~r~3 Round Burst~s~";
                }
                else if (DisplayablePlayer.CurrentSelectorSetting == SelectorOptions.FourRoundBurst)
                {
                    return "~r~4 Round Burst~s~";
                }
                else if (DisplayablePlayer.CurrentSelectorSetting == SelectorOptions.FiveRoundBurst)
                {
                    return "~r~5 Round Burst~s~";
                }
                else if (DisplayablePlayer.CurrentSelectorSetting == SelectorOptions.FullAuto)
                {
                    if (DisplayablePlayer.CurrentWeaponMagazineSize == 0)
                    {
                        return $"~r~FULL AUTO~s~";
                    }
                    else
                    {
                        return $"~r~FULL AUTO ~s~(~r~{DisplayablePlayer.CurrentWeaponMagazineSize}~s~)~s~";
                    }
                }
                else
                {
                    return "";
                }
            }
        }
        else
        {
            return "";
        }
    }
    private void GetSpeedLimitSign()
    {
        if (ShouldShowSpeedLimitSign)
        {
            float speedLimit = 60f;
            if (Settings.SettingsManager.LSRHUDSettings.SpeedDisplayUnits == "MPH")
            {
                speedLimit = DisplayablePlayer.CurrentLocation.CurrentStreet.SpeedLimitMPH;
            }
            else if (Settings.SettingsManager.LSRHUDSettings.SpeedDisplayUnits == "KM/H")
            {
                speedLimit = DisplayablePlayer.CurrentLocation.CurrentStreet.SpeedLimitKMH;
            }
            if (speedLimit <= 10f)
            {
                SpeedLimitToDraw = Sign10;
            }
            else if (speedLimit <= 15f)
            {
                SpeedLimitToDraw = Sign15;
            }
            else if (speedLimit <= 20f)
            {
                SpeedLimitToDraw = Sign20;
            }
            else if (speedLimit <= 25f)
            {
                SpeedLimitToDraw = Sign25;
            }
            else if (speedLimit <= 30f)
            {
                SpeedLimitToDraw = Sign30;
            }
            else if (speedLimit <= 35f)
            {
                SpeedLimitToDraw = Sign35;
            }
            else if (speedLimit <= 40f)
            {
                SpeedLimitToDraw = Sign40;
            }
            else if (speedLimit <= 45f)
            {
                SpeedLimitToDraw = Sign45;
            }
            else if (speedLimit <= 50f)
            {
                SpeedLimitToDraw = Sign50;
            }
            else if (speedLimit <= 55f)
            {
                SpeedLimitToDraw = Sign55;
            }
            else if (speedLimit <= 60f)
            {
                SpeedLimitToDraw = Sign60;
            }
            else if (speedLimit <= 65f)
            {
                SpeedLimitToDraw = Sign65;
            }
            else if (speedLimit <= 70f)
            {
                SpeedLimitToDraw = Sign70;
            }
            else if (speedLimit <= 75f)
            {
                SpeedLimitToDraw = Sign75;
            }
            else if (speedLimit <= 80f)
            {
                SpeedLimitToDraw = Sign80;
            }
            else
            {
                SpeedLimitToDraw = null;
            }
        }
        else
        {
            SpeedLimitToDraw = null;
        }
        if (SpeedLimitToDraw != null)
        {
            DrawSpeedLimitTexture = true;
        }
        else
        {
            DrawSpeedLimitTexture = false;
        }
    }
    private string GetStreetDisplay()
    {
        string StreetDisplay = "";
        if (1==0)//DisplayablePlayer.CurrentShop != null)//add back in some capacity?
        {
            //StreetDisplay += $"{CurrentDefaultTextColor}Inside ~p~{DisplayablePlayer.CurrentShop.Name}{CurrentDefaultTextColor}";
        }
        else
        {
            if (DisplayablePlayer.CurrentLocation.CurrentStreet != null)
            {

                string StreetNumber = NativeHelper.CellToStreetNumber(DisplayablePlayer.CellX, DisplayablePlayer.CellY);
                StreetDisplay = $"{CurrentDefaultTextColor}";
                StreetDisplay += $"{StreetNumber}";
                if (DisplayablePlayer.CurrentLocation.CurrentStreet.IsHighway)
                {
                    StreetDisplay += "~y~";
                }
                StreetDisplay += $" {DisplayablePlayer.CurrentLocation.CurrentStreet.Name}{CurrentDefaultTextColor}";

                if (DisplayablePlayer.CurrentLocation.CurrentCrossStreet != null)
                {
                    StreetDisplay += $" at {CurrentDefaultTextColor}{DisplayablePlayer.CurrentLocation.CurrentCrossStreet.Name} {CurrentDefaultTextColor}";
                }
            }
            else if (DisplayablePlayer.CurrentLocation.IsInside)
            {
                if (DisplayablePlayer.CurrentLocation.CurrentInterior?.Name == "")
                {
#if DEBUG
                    StreetDisplay += $"{CurrentDefaultTextColor} {DisplayablePlayer.CurrentLocation.CurrentInterior?.Name} ({DisplayablePlayer.CurrentLocation.CurrentInterior?.ID}) {CurrentDefaultTextColor}";
#endif
                }
                else
                {
                    StreetDisplay += $"{CurrentDefaultTextColor} {DisplayablePlayer.CurrentLocation.CurrentInterior?.Name}{CurrentDefaultTextColor}";
                }
            }
        }
        return StreetDisplay;
    }
    private string GetVehicleStatusDisplay()
    {
        string CurrentSpeedDisplay = "";
        if (DisplayablePlayer.CurrentVehicle != null)//was game.localpalyer.character.isinanyvehicle(false)
        {
            CurrentSpeedDisplay = $" {CurrentDefaultTextColor}" + "";
            if (DisplayablePlayer.CurrentVehicle.Vehicle.Exists() && DisplayablePlayer.CurrentVehicle.IsCar && !DisplayablePlayer.CurrentVehicle.Engine.IsRunning)
            {
                CurrentSpeedDisplay = $" {CurrentDefaultTextColor}" + "ENGINE OFF";
            }
            else
            {
                string ColorPrefx = CurrentDefaultTextColor;
                if (DisplayablePlayer.IsSpeeding)
                {
                    ColorPrefx = "~r~";
                }

                if (DisplayablePlayer.CurrentLocation.CurrentStreet != null)
                {
                    if (Settings.SettingsManager.LSRHUDSettings.SpeedDisplayUnits == "MPH")
                    {
                        if (Settings.SettingsManager.LSRHUDSettings.VehicleDisplayIncludeTextSpeedLimit)
                        {
                            if (Settings.SettingsManager.LSRHUDSettings.VehicleDisplayIncludeCurrentSpeed)
                            {
                                CurrentSpeedDisplay = $"{ColorPrefx}{Math.Round(DisplayablePlayer.VehicleSpeedMPH, MidpointRounding.AwayFromZero)} ({Math.Round(DisplayablePlayer.CurrentLocation.CurrentStreet.SpeedLimitMPH, MidpointRounding.AwayFromZero)} {CurrentDefaultTextColor}MPH)";
                            }
                            else
                            {
                                CurrentSpeedDisplay = $"({Math.Round(DisplayablePlayer.CurrentLocation.CurrentStreet.SpeedLimitMPH, MidpointRounding.AwayFromZero)} {CurrentDefaultTextColor}MPH)";
                            }
                        }
                        else
                        {
                            if (Settings.SettingsManager.LSRHUDSettings.VehicleDisplayIncludeCurrentSpeed)
                            {
                                CurrentSpeedDisplay = $"{ColorPrefx}{Math.Round(DisplayablePlayer.VehicleSpeedMPH, MidpointRounding.AwayFromZero)} {CurrentDefaultTextColor}MPH";
                            }
                            else
                            {
                                CurrentSpeedDisplay = "";
                            }
                        }
                    }
                    else if (Settings.SettingsManager.LSRHUDSettings.SpeedDisplayUnits == "KM/H")
                    {
                        if (Settings.SettingsManager.LSRHUDSettings.VehicleDisplayIncludeTextSpeedLimit)
                        {
                            if (Settings.SettingsManager.LSRHUDSettings.VehicleDisplayIncludeCurrentSpeed)
                            {
                                CurrentSpeedDisplay = $"{ColorPrefx}{Math.Round(DisplayablePlayer.VehicleSpeedKMH, MidpointRounding.AwayFromZero)} ({Math.Round(DisplayablePlayer.CurrentLocation.CurrentStreet.SpeedLimitKMH, MidpointRounding.AwayFromZero)} {CurrentDefaultTextColor}KM/H)";
                            }
                            else
                            {
                                CurrentSpeedDisplay = $"({Math.Round(DisplayablePlayer.CurrentLocation.CurrentStreet.SpeedLimitKMH, MidpointRounding.AwayFromZero)} {CurrentDefaultTextColor}KM/H)";
                            }
                        }
                        else
                        {
                            if (Settings.SettingsManager.LSRHUDSettings.VehicleDisplayIncludeCurrentSpeed)
                            {
                                CurrentSpeedDisplay = $"{ColorPrefx}{Math.Round(DisplayablePlayer.VehicleSpeedKMH, MidpointRounding.AwayFromZero)} {CurrentDefaultTextColor}KM/H";
                            }
                            else
                            {
                                CurrentSpeedDisplay = "";
                            }
                        }
                    }
                }
            }
            if (DisplayablePlayer.Violations.IsViolatingAnyTrafficLaws)
            {
                CurrentSpeedDisplay += " ~r~!";
            }
            if (DisplayablePlayer.CurrentVehicle.Indicators.HazardsOn)
            {
                CurrentSpeedDisplay += " ~o~(HAZ)";
            }
            else if (DisplayablePlayer.CurrentVehicle.Indicators.RightBlinkerOn)
            {
                CurrentSpeedDisplay += " ~y~(RI)";
            }
            else if (DisplayablePlayer.CurrentVehicle.Indicators.LeftBlinkerOn)
            {
                CurrentSpeedDisplay += " ~y~(LI)";
            }
            if (Settings.SettingsManager.VehicleSettings.UseCustomFuelSystem)
            {
                CurrentSpeedDisplay += $"{CurrentDefaultTextColor} " + DisplayablePlayer.CurrentVehicle.FuelTank.UIText;
            }
            if (Settings.SettingsManager.LSRHUDSettings.VehicleDisplayIncludeCompass)
            {
                string Heading = NativeHelper.GetSimpleCompassHeading(DisplayablePlayer.Character.Heading);
                CurrentSpeedDisplay += $" {Heading}";
            }
        }
        return CurrentSpeedDisplay;
    }
    private string GetViolationsText()
    {
        string CrimeDisplay = "";
        if (DisplayablePlayer.Violations.LawsViolatingDisplay != "")
        {
            CrimeDisplay += "Violating: " + DisplayablePlayer.Violations.LawsViolatingDisplay;
        }
        return CrimeDisplay;
    }
    private string GetWeaponDisplay()
    {
        string WeaponDisplay = "";
        if (Settings.SettingsManager.LSRHUDSettings.ShowWeaponDisplay)
        {
            if (WeaponDisplay == "")
            {
                WeaponDisplay = $"{CurrentDefaultTextColor}" + GetSelectorText();
            }
            else
            {
                WeaponDisplay += $" {CurrentDefaultTextColor}" + GetSelectorText();
            }
        }
        return WeaponDisplay;
    }
    private string GetZoneDisplay()
    {
        string toDisplay = "";
        if (DisplayablePlayer.CurrentLocation.CurrentZone != null)
        {
            toDisplay = $"{CurrentDefaultTextColor}" + DisplayablePlayer.CurrentLocation.CurrentZone.FullDisplayName;
            if (Settings.SettingsManager.LSRHUDSettings.ZoneDisplayShowPrimaryAgency && DisplayablePlayer.CurrentLocation.CurrentZone.AssignedLEAgencyInitials != "")
            {
                toDisplay += $"{CurrentDefaultTextColor} / " + DisplayablePlayer.CurrentLocation.CurrentZone.AssignedLEAgencyInitials;
            }
            if (Settings.SettingsManager.LSRHUDSettings.ZoneDisplayShowPrimaryGang && DisplayablePlayer.CurrentLocation.CurrentZone.AssignedGangInitials != string.Empty)
            {
                toDisplay += $"{CurrentDefaultTextColor} - " + DisplayablePlayer.CurrentLocation.CurrentZone.AssignedGangInitials;
            }
            else if (Settings.SettingsManager.LSRHUDSettings.ZoneDisplayShowSecondaryAgency && DisplayablePlayer.CurrentLocation.CurrentZone.AssignedSecondLEAgencyInitials != string.Empty)
            {
                toDisplay += $"{CurrentDefaultTextColor} - " + DisplayablePlayer.CurrentLocation.CurrentZone.AssignedSecondLEAgencyInitials;
            }
        }
        return toDisplay;
    }
    private void MenuUpdate()
    {
        menuPool.ProcessMenus();
        PlayerInfoMenu.Update();
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
                    BigMessage.MessageInstance.ShowColoredShard(Settings.SettingsManager.UIGeneralSettings.WastedMessageText, "", HudColor.Black, HudColor.RedDark, 2000);
                }
                StartedDeathEffect = true;
            }
            if (GameTimeLastDied != 0 && Game.GameTime - GameTimeLastDied >= (Settings.SettingsManager.PlayerOtherSettings.SetSlowMoOnDeath ? 1000 : 2000))
            {
                GameTimeLastDied = 0;
                Show(DeathMenu);
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
                    BigMessage.MessageInstance.ShowColoredShard(Settings.SettingsManager.UIGeneralSettings.BustedMessageText, "", HudColor.Black, HudColor.Blue, 2000);
                }
                StartedBustedEffect = true;
            }
            if (GameTimeLastBusted != 0 && Game.GameTime - GameTimeLastBusted >= (Settings.SettingsManager.PlayerOtherSettings.SetSlowMoOnBusted ? 1000 : 2000))
            {
                GameTimeLastBusted = 0;
                Show(BustedMenu);
            }
        }
        //else if (DisplayablePlayer.Character.Health <= 130)
        //{
        //    if(Game.GameTime - GameTimeStartedLowHealthUI >= 5000)
        //    {
        //        NativeFunction.Natives.x80C8B1846639BB19(1);
        //        NativeFunction.Natives.x2206BF9A37B7F724("DeathFailMPDark", 0, 0);
        //        IsShowingCustomOverlay = true;
        //        GameTimeStartedLowHealthUI = Game.GameTime;
        //    }
        //}
        else
        {
            GameTimeLastDied = 0;
            GameTimeLastBusted = 0;
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
    private void Show(Menu toShow)
    {
        foreach (Menu menu in MenuList)
        {
            if (menu != toShow)
            {
                menu.Hide();
            }
        }
        toShow.Show();
    }
    private void Toggle(Menu toToggle)
    {
        foreach (Menu menu in MenuList)
        {
            if (menu != toToggle)
            {
                menu.Hide();
            }
        }
        toToggle.Toggle();
    }
    public void UpdateWheelMenu(bool isPressingActionWheelMenu)
    {
        if(isPressingActionWheelMenu)
        {
            if(ActionPopUpMenu.HasRanItem)
            {
                if(IsDrawingWheelMenu)
                {
                    ActionPopUpMenu.OnStopDisplaying();
                    IsDrawingWheelMenu = false;
                }   
            }
            else
            {
                if (!IsDrawingWheelMenu)
                {
                    ActionPopUpMenu.OnStartDisplaying();
                    IsDrawingWheelMenu = true;
                }
            }
        }
        else
        {
            if (IsDrawingWheelMenu)
            {
                ActionPopUpMenu.OnStopDisplaying();
                ActionPopUpMenu.Dispose();
                IsDrawingWheelMenu = false;
            }
            ActionPopUpMenu.Reset();
        }
    }
}